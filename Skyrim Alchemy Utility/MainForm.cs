using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Skyrim_Alchemy_Utility
{
    public partial class MainForm : Form
    {
        public const string URL_EFFECTS = "http://www.uesp.net/wiki/Skyrim:Alchemy_Effects";
        public const string URL_INGREDIENTS_MAIN = "http://www.uesp.net/wiki/Skyrim:Ingredients";
        public const string URL_INGREDIENTS_DRAGONBORN = "http://www.uesp.net/wiki/Dragonborn:Ingredients";
        public static Dictionary<string, Effect> Effects = new Dictionary<string, Effect>();
        public static Dictionary<string, Ingredient> Ingredients = new Dictionary<string, Ingredient>();
        public static List<Property> Properties = new List<Property>();

        private const string XML_DB = "alchemy.xml";
        private readonly string AppDataDir;

        public enum DLC
        {
            Skyrim,
            Dawnguard,
            Hearthfire,
            Dragonborn,
        }

        public MainForm()
        {
            InitializeComponent();

            AppDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SkyrimAlchemy");

            LoadDB();

            DrawGUI();

            drpIng[0].Select();
        }

        #region Build Database

        public void LoadDB(bool rebuildDb = false)
        {
            string btnText = null;
            if (btnUpdateDb != null)
            {
                btnText = btnUpdateDb.Text;
                btnUpdateDb.Text = "Updating...";
                btnUpdateDb.Enabled = false;
            }

            var dbFilePath = Path.Combine(AppDataDir, XML_DB);
            var serializer = new XmlSerializer(typeof(DB));

            var buildDbFromWeb = rebuildDb || !Directory.Exists(AppDataDir) || !File.Exists(dbFilePath);
            if (buildDbFromWeb)
            {
                Dictionary<string, Effect> tmpEffects = null;
                Effect[] _Effects = null;
                Ingredient[] _Ingredients = null;
                List<Property> tmpProps = null;
                try
                {
                    // Save current lists in case of connection error
                    tmpProps = Properties.ToList();
                    tmpEffects = Effects.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                    Properties.Clear();
                    _Effects = BuildEffects();
                    Effects = _Effects.ToDictionary(ef => ef.ID, ef => ef);
                    _Ingredients = BuildIngredients(URL_INGREDIENTS_MAIN).Concat(BuildIngredients(URL_INGREDIENTS_DRAGONBORN)).ToArray();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred attempting to retrieve data from the internet. Please check your connection and try again.");
                    Effects = tmpEffects;
                    Properties = tmpProps;
                    goto EndUpdate;
                }

                Cursor.Current = Cursors.WaitCursor;
                Application.DoEvents();

                Ingredients = _Ingredients.ToDictionary(ing => ing.ID, ing => ing);

                if (!Directory.Exists(AppDataDir)) Directory.CreateDirectory(AppDataDir);
                if (File.Exists(dbFilePath))
                {
                    try
                    {
                        File.Delete(dbFilePath);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show("An error occurred while attempting to delete the old database file. Please close all instances of Skyrim Alchemy Utility and try again.");
                        this.Close();
                    }
                }

                var db = new DB { Effects = _Effects, Ingredients = _Ingredients, Properties = Properties.ToArray() };
                var ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var settings = new XmlWriterSettings() { Indent = true, OmitXmlDeclaration = true };
                XmlWriter writer = XmlWriter.Create(dbFilePath, settings);
                serializer.Serialize(writer, db, ns);

                writer.Close();

                Cursor.Current = Cursors.Default;
            }
            else
            {
                var reader = new StreamReader(dbFilePath);
                var db = (DB)serializer.Deserialize(reader);

                Effects = db.Effects.ToDictionary(ef => ef.ID, ef => ef);
                Ingredients = db.Ingredients.ToDictionary(ing => ing.ID, ing => ing);
                Properties = db.Properties.ToList();

                reader.Close();
            }
        EndUpdate:

            if (btnUpdateDb != null)
            {
                btnUpdateDb.Text = btnText;
                btnUpdateDb.Enabled = true;
            }
        }

        public Effect[] BuildEffects()
        {
            var web = new HtmlWeb();
            var doc = web.Load(URL_EFFECTS);
            var html = doc.DocumentNode.InnerHtml;

            var rows = doc.DocumentNode.SelectSingleNode("//table[@class='wikitable sortable']").SelectNodes("tr").Cast<HtmlNode>();
            var headings = (from HtmlNode cell in rows.First().SelectNodes("th|td")
                            select cell.InnerText).ToArray();

            var reNum = new Regex(@"^\d+");

            var efs = new List<Effect>();
            foreach (var row in rows.Skip(1))
            {
                var colIndex = 0;
                var ef = new Effect();
                foreach (HtmlNode cell in row.SelectNodes("th|td"))
                {
                    var text = cell.InnerText;
                    switch (headings[colIndex])
                    {
                        case "Effect (ID)":
                            var _class = cell.Attributes["class"].Value;
                            ef.IsBeneficial = _class.Equals("EffectPos") ? 1 : 0;
                            var parts = text.Split(new string[] { "\n(", ")" }, StringSplitOptions.None);
                            ef.Name = parts[0];
                            ef.ID = parts[1];
                            break;
                        case "Description":
                            ef.Description = text.Replace("&lt;", "<")
                                                 .Replace("&gt;", ">");
                            break;
                        case "Base_Cost": ef.Cost = Single.Parse(text); break;
                        case "Base_Mag":
                            ef.Mag = Int32.Parse(reNum.Match(text).Groups[0].Value);
                            ef.Description = ef.Description.Replace("<mag>", ef.Mag.ToString());
                            break;
                        case "Base_Dur":
                            ef.Dur = Int32.Parse(reNum.Match(text).Groups[0].Value);
                            ef.Description = ef.Description.Replace("<dur>", ef.Dur.ToString());
                            break;
                        case "": ef.Value = Int32.Parse(reNum.Match(text).Groups[0].Value); break;

                        case "Ingredients":
                        default:
                            break;
                    }
                    colIndex++;
                }
                efs.Add(ef);
            }
            return efs.ToArray();
        }

        public Ingredient[] BuildIngredients(string url)
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var html = doc.DocumentNode.InnerHtml;

            var rows = doc.DocumentNode.SelectSingleNode("//table[@class='wikitable']").SelectNodes("tr").Cast<HtmlNode>();
            var headings = (from HtmlNode cell in rows.First().SelectNodes("th|td")
                            select cell.InnerText).ToArray();

            var ings = new List<Ingredient>();

            var newIng = true;
            Ingredient ing = null;
            foreach (var row in rows.Skip(1))
            {
                var colIndex = 0;
                int defaultDlcCode;
                switch (url)
                {
                    default:
                    case URL_INGREDIENTS_MAIN: defaultDlcCode = 0; break;
                    case URL_INGREDIENTS_DRAGONBORN: defaultDlcCode = 3; break;
                }
                if (newIng) ing = new Ingredient { DLC = defaultDlcCode };
                foreach (HtmlNode cell in row.SelectNodes("th|td"))
                {
                    var text = cell.InnerText;
                    if (newIng)
                    {
                        switch (colIndex)
                        {
                            case 0:
                                break;
                            case 1:
                                var parts = text.Split('\n');
                                ing.Name = parts[0];
                                if (ing.Name.Contains("DG"))
                                {
                                    ing.Name = ing.Name.Replace("DG", "");
                                    ing.DLC = 1;
                                }
                                else if (ing.Name.Contains("HF"))
                                {
                                    ing.Name = ing.Name.Replace("HF", "");
                                    ing.DLC = 2;
                                }
                                ing.ID = parts[1].ToUpper();
                                break;
                            case 2: ing.Description = text; break;
                        }
                    }
                    else
                    {
                        switch (colIndex)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                                if (text.Contains(";")) text = text.Split(';')[1];
                                var prop = new Property
                                {
                                    IngID = ing.ID,
                                    EfID = Effects.Values.First(ef => text.Split(new string[] { " (" }, StringSplitOptions.None)[0].Equals(ef.Name)).ID
                                };
                                Properties.Add(prop);
                                break;
                            case 4: ing.Value = Int32.Parse(text); break;
                            case 5: ing.Weight = Single.Parse(text); break;
                            case 6: // Merchant Avail.
                            case 7: // Garden
                            default:
                                break;
                        }
                    }
                    colIndex++;
                }
                newIng = !newIng;
                if (!ings.Any(ingr => ingr.ID.Equals(ing.ID))) ings.Add(ing);
            }
            return ings.ToArray();
        }

        #endregion

        #region Draw Interface

        private DataGridView[] dgvEffects;
        private ComboBox[] drpIng;
        private const int TXT_EFFECTS_WIDTH = 73;

        // Filter Controls
        private CheckBox chkPosEffect;
        private CheckBox chkNegEffect;
        private CheckBox[] chkDlc;
        private Dictionary<string, CheckBox> chkEffect;

        // Character Trait Controls
        //private NumericUpDown spinAlchemyLevel; // 15-100
        //private CheckBox chkPhysicianPerk; // Restore Health, Magicka, or Stamina + 25%
        //private CheckBox chkBenefactorPerk; // +25% Beneficial
        //private CheckBox chkPoisonerPerk; // +25% Harmful
        //private CheckBox chkPurityPerk; // Only Harmful or only Beneficial

        public void DrawGUI()
        {            
            #region Ingredients
            drpIng = new ComboBox[3];
            var lblIng = new Label[3];
            var lblIngId = new Label[3];
            dgvEffects = new DataGridView[3];
            var noIngs = new List<Ingredient> { new Ingredient() { Name = "<NONE>" } };
            var allIngs = noIngs.ToList();
            allIngs.AddRange(Ingredients.Values.OrderBy(ing => ing.Name));

            for (int i = 0; i < 3; i++)
            {
                lblIng[i] = new Label();
                lblIng[i].Text = string.Format("Ingredient #{0}:", i);
                lblIng[i].AutoSize = true;
                lblIng[i].Location = i == 0 ? new Point(5, 5) : new Point(lblIng[i - 1].Location.X, drpIng[i - 1].Location.Y + 15 + drpIng[i - 1].Height);

                lblIngId[i] = new Label();
                lblIngId[i].Text = string.Empty;
                lblIngId[i].AutoSize = true;
                lblIngId[i].Location = new Point(i == 0 ? lblIng[i].Right - 16 : lblIngId[i - 1].Left, lblIng[i].Top);

                drpIng[i] = new ComboBox();
                drpIng[i].Size = new Size(150, drpIng[i].Height);
                drpIng[i].Location = new Point(lblIng[i].Location.X, lblIng[i].Location.Y + lblIng[i].Height);
                drpIng[i].DataSource = allIngs.ToList();
                if (i != 0) drpIng[i].Enabled = false;
                #region drpIng[i].SelectedIndexChanged
                switch (i)
                {
                    case 0:
                        drpIng[i].SelectedIndexChanged += (o, e) =>
                        {
                            var ing = drpIng.Select(drp => drp.SelectedValue as Ingredient).ToArray();
                            lblIngId[0].Text = ing[0].ID == null ? string.Empty : ing[0].ID;
                            var row1 = dgvEffects[0].Rows.Cast<DataGridViewRow>().First();
                            drpIng[1].Enabled = ing[0].ID != null;
                            for (int j = 0; j < 4; j++)
                            {
                                ((DataGridViewTextBoxCell)row1.Cells[j]).Value = drpIng[1].Enabled ? ing[0].Effects[j].Name : string.Empty;
                                dgvEffects[0].Columns[j].DefaultCellStyle.BackColor = Color.White;
                            }

                            var ing2 = drpIng[1].SelectedValue as Ingredient;
                            var newSet = FilterIngredients(ing[0].CompatibleIngredients.Where(ingr => ingr.ID != null).Select(ingr => Ingredients[ingr.ID]));
                            drpIng[1].DataSource = noIngs.Concat(newSet).ToList();
                            if (ing2.ID != null && newSet.Any(ingr => ingr.ID != null && ingr.ID.Equals(ing[1].ID))) drpIng[1].SelectedItem = ing[1].Name;
                        };
                        break;
                    case 1:
                        drpIng[i].SelectedIndexChanged += (o, e) =>
                        {
                            var ing = drpIng.Select(drp => drp.SelectedValue as Ingredient).ToArray();
                            lblIngId[1].Text = ing[1].ID == null ? string.Empty : ing[1].ID;
                            var row1 = dgvEffects[1].Rows.Cast<DataGridViewRow>().First();
                            for (int j = 0; j < 4; j++) dgvEffects[0].Columns[j].DefaultCellStyle.BackColor = Color.White;
                            for (int j = 0; j < 4; j++)
                            {
                                var cell = row1.Cells[j] as DataGridViewTextBoxCell;
                                if (ing[1].ID != null)
                                {
                                    var ef = ing[1].Effects[j];
                                    cell.Value = ef.Name;
                                }
                                else cell.Value = string.Empty;
                            }
                            SetEffectColors();

                            var results = noIngs.ToList();
                            if (ing[0].ID != null)
                            {
                                var compat1 = ing[0].CompatibleIngredients;
                                results.AddRange(compat1);
                            }
                            drpIng[2].Enabled = ing[1].ID != null;
                            if (drpIng[1].Enabled)
                            {
                                var compat2 = ing[1].CompatibleIngredients;
                                results.AddRange(compat2);
                            }
                            else lblPotionValue.Text = "N/A";
                            results = results.DistinctBy(ingr => ingr.ID).Where(ingr => ingr.ID == null || !(ingr.ID.Equals(ing[0].ID) || ingr.ID.Equals(ing[1].ID)))
                                                .OrderBy(ingr => !ingr.Name.Equals("<NONE>")).ThenBy(ingr => ingr.Name).ToList();
                            results = FilterIngredients(results.Where(ingr => ingr.ID != null).Select(ingr => Ingredients[ingr.ID]));
                            drpIng[2].DataSource = noIngs.Concat(results).ToList();
                            if (ing[2].ID != null && results.Any(ingr => ingr.ID != null && ingr.ID.Equals(ing[2].ID))) drpIng[2].SelectedItem = ing[2].Name;
                        };
                        break;
                    case 2:
                        drpIng[i].SelectedIndexChanged += (o, e) =>
                        {
                            var ing = drpIng.Select(drp => drp.SelectedValue as Ingredient).ToArray();
                            lblIngId[2].Text = ing[2].ID == null ? string.Empty : ing[2].ID;
                            var row1 = dgvEffects[2].Rows.Cast<DataGridViewRow>().First();
                            for (int j = 0; j < 4; j++)
                            {
                                var cell = row1.Cells[j] as DataGridViewTextBoxCell;
                                if (ing[2].ID != null)
                                {
                                    var ef = ing[2].Effects[j];
                                    cell.Value = ef.Name;
                                }
                                else cell.Value = string.Empty;
                            }
                            SetEffectColors();
                            UpdateResult(o, e);
                        };
                        break;
                }
                #endregion
                pnlIngredients.Controls.Add(drpIng[i]);
                drpIng[i].BringToFront();

                dgvEffects[i] = new DataGridView();
                dgvEffects[i].ColumnCount = 4;
                dgvEffects[i].Name = "dgvIng" + i.ToString() + "Effects";
                dgvEffects[i].Location = new Point(drpIng[i].Right + 10, drpIng[i].Top);
                dgvEffects[i].Size = new Size(100 + 3 * TXT_EFFECTS_WIDTH, 22);
                dgvEffects[i].AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvEffects[i].Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                dgvEffects[i].EditMode = DataGridViewEditMode.EditProgrammatically;
                dgvEffects[i].AllowUserToAddRows = false;
                dgvEffects[i].AllowUserToDeleteRows = false;
                dgvEffects[i].AllowUserToResizeColumns = false;
                dgvEffects[i].AllowUserToResizeRows = false;
                dgvEffects[i].RowHeadersVisible = false;
                dgvEffects[i].ColumnHeadersVisible = false;
                switch (i)
                {
                    case 0: dgvEffects[i].SelectionChanged += (o, e) => dgvEffects[0].ClearSelection(); break;
                    case 1: dgvEffects[i].SelectionChanged += (o, e) => dgvEffects[1].ClearSelection(); break;
                    case 2: dgvEffects[i].SelectionChanged += (o, e) => dgvEffects[2].ClearSelection(); break;
                }
                dgvEffects[i].Rows.Add(new DataGridViewRow());

                var row = dgvEffects[i].Rows.Cast<DataGridViewRow>().First();
                for (int j = 0; j < 4; j++)
                {
                    var cell = row.Cells[0] as DataGridViewTextBoxCell;
                    cell.Value = string.Empty;
                    cell.ReadOnly = true;
                }
            }
            pnlIngredients.Controls.AddRange(lblIng);
            pnlIngredients.Controls.AddRange(lblIngId);
            pnlIngredients.Controls.AddRange(dgvEffects);

            btnUpdateDb.Click += (o, e) => LoadDB(true);
            btnUpdateDb.Visible = Program.IsAdministrator;
            #endregion

            #region Filters
            btnApplyFilter.Click += ApplyFilter;
            pnlFilterChk.HorizontalScroll.Enabled = false;

            chkDlc = new CheckBox[4];
            for (int i = 0; i < 4; i++)
            {
                chkDlc[i] = new CheckBox();
                chkDlc[i].Text = i == 0 ? "ALL DLC" : ((DLC)i).ToString();
                chkDlc[i].AutoSize = true;
                chkDlc[i].Location = new Point(5, i == 0 ? 5 : chkDlc[i - 1].Bottom - 8);
                chkDlc[i].Checked = true;
                if (i > 0) chkDlc[i].CheckedChanged += chkDlc_CheckChanged;
                else
                {
                    chkDlc[i].ThreeState = true;
                    chkDlc[i].Click += (o, e) =>
                    {
                        switch (chkDlc[0].CheckState)
                        {
                            case CheckState.Checked:
                                foreach (var chk in chkDlc.Skip(1)) chk.CheckState = CheckState.Checked;
                                break;
                            case CheckState.Unchecked:
                            case CheckState.Indeterminate:
                                foreach (var chk in chkDlc.Skip(1)) chk.CheckState = CheckState.Unchecked;
                                break;
                        }
                    };
                }
            }
            pnlFilterChk.Controls.AddRange(chkDlc);

            var _efs = Effects.Values.OrderBy(_ing => _ing.Name).ToArray();
            chkEffect = new Dictionary<string, CheckBox>();
            var chkAllEffects = new CheckBox();
            chkAllEffects.Text = "ALL EFFECTS";
            chkAllEffects.AutoSize = true;
            chkAllEffects.Location = new Point(chkDlc[0].Left, chkDlc[3].Bottom + 20);
            chkAllEffects.Checked = true;
            chkAllEffects.ThreeState = true;
            chkAllEffects.Click += (o, e) =>
            {
                switch (chkAllEffects.CheckState)
                {
                    case CheckState.Checked:
                        foreach (var chk in chkEffect.Values) chk.CheckState = CheckState.Checked;
                        chkPosEffect.CheckState = CheckState.Checked;
                        chkNegEffect.CheckState = CheckState.Checked;
                        break;
                    case CheckState.Unchecked:
                    case CheckState.Indeterminate:
                        foreach (var chk in chkEffect.Values) chk.CheckState = CheckState.Unchecked;
                        chkPosEffect.CheckState = CheckState.Unchecked;
                        chkNegEffect.CheckState = CheckState.Unchecked;
                        break;
                }
            };
            chkEffect[string.Empty] = chkAllEffects;
            pnlFilterChk.Controls.Add(chkAllEffects);

            chkPosEffect = new CheckBox();
            chkPosEffect.Text = "Beneficial";
            chkPosEffect.AutoSize = true;
            chkPosEffect.Location = new Point(chkAllEffects.Left, chkAllEffects.Bottom - 2);
            chkPosEffect.ThreeState = true;
            chkPosEffect.CheckState = CheckState.Checked;
            chkPosEffect.Click += (o, e) =>
            {
                switch (chkPosEffect.CheckState)
                {
                    case CheckState.Checked:
                        foreach (var efId in chkEffect.Keys.Skip(1))
                        {
                            if (Effects[efId].IsBeneficial == 1)
                            {
                                chkEffect[efId].CheckState = CheckState.Checked;
                            }
                        }
                        break;
                    case CheckState.Indeterminate:
                        chkPosEffect.CheckState = CheckState.Unchecked;
                        goto case CheckState.Unchecked;
                    case CheckState.Unchecked:
                        foreach (var efId in chkEffect.Keys.Skip(1))
                        {
                            if (Effects[efId].IsBeneficial == 1)
                            {
                                chkEffect[efId].CheckState = CheckState.Unchecked;
                            }
                        }
                        break;
                }
            };
            pnlFilterChk.Controls.Add(chkPosEffect);
            chkPosEffect.BringToFront();

            chkNegEffect = new CheckBox();
            chkNegEffect.Text = "Harmful";
            chkNegEffect.AutoSize = true;
            chkNegEffect.Location = new Point(chkPosEffect.Left, chkPosEffect.Bottom - 2);

            chkNegEffect.ThreeState = true;
            chkNegEffect.CheckState = CheckState.Checked;
            chkNegEffect.Click += (o, e) =>
            {
                switch (chkNegEffect.CheckState)
                {
                    case CheckState.Checked:
                        foreach (var efId in chkEffect.Keys.Skip(1))
                        {
                            if (Effects[efId].IsBeneficial == 0)
                            {
                                chkEffect[efId].CheckState = CheckState.Checked;
                            }
                        }
                        break;
                    case CheckState.Indeterminate:
                        chkNegEffect.CheckState = CheckState.Unchecked;
                        goto case CheckState.Unchecked;
                    case CheckState.Unchecked:
                        foreach (var efId in chkEffect.Keys.Skip(1))
                        {
                            if (Effects[efId].IsBeneficial == 0)
                            {
                                chkEffect[efId].CheckState = CheckState.Unchecked;
                            }
                        }
                        break;
                }
            };
            pnlFilterChk.Controls.Add(chkNegEffect);
            chkNegEffect.BringToFront();

            var lblSep = new Label();
            lblSep.Text = string.Empty;
            lblSep.BorderStyle = BorderStyle.Fixed3D;
            lblSep.AutoSize = false;
            lblSep.Height = 2;
            lblSep.Width = pnlFilterChk.Width - 28;
            lblSep.Location = new Point(5, chkNegEffect.Bottom + 1);
            pnlFilterChk.Controls.Add(lblSep);

            for (int i = 0; i < _efs.Length; i++)
            {
                var effect = _efs[i];
                var chk = new CheckBox();
                chk.Text = effect.Name;
                chk.Location = new Point(chkAllEffects.Left, i == 0 ? chkNegEffect.Bottom + 5 : chkEffect[_efs[i - 1].ID].Bottom - 2);
                chk.AutoSize = true;
                chk.Checked = true;
                chk.CheckedChanged += chkEffect_CheckChanged;
                chkEffect[effect.ID] = chk;
                pnlFilterChk.Controls.Add(chk);
                chk.BringToFront();
            }
            #endregion

            #region Character Traits
            spinAlchemyLevel.ValueChanged += UpdateResult;
            spinAlchemistPerk.ValueChanged += UpdateResult;

            var ttTemplate = new ToolTip();
            ttTemplate.AutoPopDelay = 5000;
            ttTemplate.InitialDelay = 1000;
            ttTemplate.ReshowDelay = 500;
            ttTemplate.ShowAlways = true;

            chkPhysicianPerk.CheckedChanged += UpdateResult;
            ttTemplate.SetToolTip(this.chkPhysicianPerk, "+25% Bonus to Restore Health, Magicka, or Stamina");
            chkBenefactorPerk.CheckedChanged += UpdateResult;
            ttTemplate.SetToolTip(this.chkBenefactorPerk, "+25% Bonus to beneficial Effects");
            chkPoisonerPerk.CheckedChanged += UpdateResult;
            ttTemplate.SetToolTip(this.chkPoisonerPerk, "+25% Bonus to harmful Effects");
            chkPurityPerk.CheckedChanged += UpdateResult;
            ttTemplate.SetToolTip(this.chkPurityPerk, "Potion contains only beneficial effects or only harmful effects");
            #endregion
        }

        public void chkDlc_CheckChanged(object sender, EventArgs e)
        {
            var chk = sender as CheckBox;
            var chkAll = chkDlc[0];
            switch (chkAll.CheckState)
            {
                case CheckState.Checked:
                    if (!chk.Checked) chkAll.CheckState = CheckState.Indeterminate;
                    break;
                case CheckState.Indeterminate:
                    break;
                case CheckState.Unchecked:
                    if (chk.Checked) chkAll.CheckState = CheckState.Indeterminate;
                    break;
            }
        }

        public void chkEffect_CheckChanged(object sender, EventArgs e)
        {
            var chk = sender as CheckBox;
            var chkAll = chkEffect[""];
            switch (chkAll.CheckState)
            {
                case CheckState.Checked:
                    if (!chk.Checked) chkAll.CheckState = CheckState.Indeterminate;
                    break;
                case CheckState.Indeterminate:
                    break;
                case CheckState.Unchecked:
                    if (chk.Checked) chkAll.CheckState = CheckState.Indeterminate;
                    break;
            }
            if (Effects.Values.First(ef => ef.Name.Equals(chk.Text)).IsBeneficial == 1)
            {
                switch (chkPosEffect.CheckState)
                {
                    case CheckState.Checked:
                        if (!chk.Checked) chkPosEffect.CheckState = CheckState.Indeterminate;
                        break;
                    case CheckState.Indeterminate:
                        break;
                    case CheckState.Unchecked:
                        if (chk.Checked) chkPosEffect.CheckState = CheckState.Indeterminate;
                        break;
                }
            }
            else
            {
                switch (chkNegEffect.CheckState)
                {
                    case CheckState.Checked:
                        if (!chk.Checked) chkNegEffect.CheckState = CheckState.Indeterminate;
                        break;
                    case CheckState.Indeterminate:
                        break;
                    case CheckState.Unchecked:
                        if (chk.Checked) chkNegEffect.CheckState = CheckState.Indeterminate;
                        break;
                }
            }
        }

        public void ApplyFilter(object sender, EventArgs e)
        {
            if (btnApplyFilter.Text.Contains("*")) btnApplyFilter.Text += "*";
            var noIngs = new List<Ingredient> { new Ingredient { Name = "<NONE>" } };
            var allIngs = noIngs.ToList();
            allIngs.AddRange(FilterIngredients(Ingredients.Values));
            drpIng[0].DataSource = allIngs;
        }

        public List<Ingredient> FilterIngredients(IEnumerable<Ingredient> ings)
        {
            return (from ing in ings
                    where ing.DLC == 0 || chkDlc[0].CheckState == CheckState.Checked || chkDlc[ing.DLC].Checked
                    where (chkPosEffect.Checked && ing.Effects.Any(ef => ef.IsBeneficial == 1)) || (chkNegEffect.Checked && ing.Effects.Any(ef => ef.IsBeneficial == 0))
                    where chkEffect[""].CheckState == CheckState.Checked || ing.Effects.Any(ef => chkEffect[ef.ID].Checked)
                    orderby ing.Name
                    select ing).ToList();
        }

        public void SetEffectColors()
        {
            var ing = drpIng.Select(drp => drp.SelectedValue as Ingredient).ToArray();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++) dgvEffects[i].Columns[j].DefaultCellStyle.BackColor = Color.White;
            }
            if (ing[1].ID != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    var ef = ing[1].Effects[i];
                    if (ing[0].ID != null)
                    {
                        var efIdList = ing[0].Effects.Select(_ef => _ef.ID).ToList();
                        var i0 = efIdList.IndexOf(ef.ID);
                        if (i0 != -1)
                        {
                            dgvEffects[1].Columns[i].DefaultCellStyle.BackColor = Color.LightBlue;
                            dgvEffects[0].Columns[i0].DefaultCellStyle.BackColor = Color.LightBlue;
                        }
                        else dgvEffects[1].Columns[i].DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
            if (ing[2].ID != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    var ef = ing[2].Effects[i];
                    if (ing[0].ID != null)
                    {
                        var i0 = ing[0].Effects.Select(_ef => _ef.ID).ToList().IndexOf(ef.ID);
                        if (i0 != -1)
                        {
                            dgvEffects[2].Columns[i].DefaultCellStyle.BackColor = Color.LightYellow;
                            var ing0ColStyle = dgvEffects[0].Columns[i0].DefaultCellStyle;
                            ing0ColStyle.BackColor = ing0ColStyle.BackColor == Color.LightBlue ? Color.LightGreen : Color.LightYellow;
                        }
                    }
                    if (ing[1].ID != null)
                    {
                        var i0 = ing[1].Effects.Select(_ef => _ef.ID).ToList().IndexOf(ef.ID);
                        if (i0 != -1)
                        {
                            var ing2ColStyle = dgvEffects[2].Columns[i].DefaultCellStyle;
                            ing2ColStyle.BackColor = ing2ColStyle.BackColor == Color.LightYellow ? Color.LightGreen : Color.Pink;
                            var ing1ColStyle = dgvEffects[1].Columns[i0].DefaultCellStyle;
                            ing1ColStyle.BackColor = ing1ColStyle.BackColor == Color.LightBlue ? Color.LightGreen : Color.Pink;
                        }
                    }
                }
            }
        }

        #endregion

        #region Generate Potion

        public void UpdateResult(object sender, EventArgs e)
        {
            dgvPotion.Rows.Clear();
            var ing = drpIng.Select(drp => drp.SelectedValue as Ingredient).ToArray();
            var value = 0;
            if (ing[0].ID == null || ing[1].ID == null)
            {
                lblPotionValue.Text = "N/A";
                lblPotionClass.Text = "";
                return;
            }
            var effects = (from ef1 in ing[0].Effects
                           join ef2 in ing[1].Effects
                           on ef1.ID equals ef2.ID
                           select ef1).DistinctBy(ef => ef.ID).ToList();
            if (ing[2].ID != null)
            {
                effects = effects.Concat(from ef1 in ing[0].Effects
                                         join ef3 in ing[2].Effects on ef1.ID equals ef3.ID
                                         select ef1)
                                 .Concat(from ef2 in ing[1].Effects
                                         join ef3 in ing[2].Effects on ef2.ID equals ef3.ID
                                         select ef2)
                                 .DistinctBy(ef => ef.ID).ToList();
            }
            var mag = new double[effects.Count];
            var dur = new double[effects.Count];
            var values = new List<int>(); ;
            for (int i = 0; i < effects.Count; i++)
            {
                var effect = effects[i];
                mag[i] = GetEffectMagnitude(effect);
                dur[i] = GetEffectDuration(effect);
                values.Add(GetEffectValue(effect, mag[i], dur[i]));
            }
            var strongestIndex = values.IndexOf(values.Max());
            var strongestEffect = effects[strongestIndex];
            var resultEffects = effects.Select((ef, i) => new Tuple<Effect, int>(ef, i));
            lblPotionClass.Text = strongestEffect.IsBeneficial == 1 ? "Potion" : "Poison";
            if (chkPurityPerk.Checked)
                resultEffects = resultEffects.Where(tup => tup.Item1.IsBeneficial == strongestEffect.IsBeneficial);

            value = resultEffects.Sum(tup => values[tup.Item2]);

            foreach (var tup in resultEffects)
                dgvPotion.Rows.Add(tup.Item1.Name, string.Format("{0:.00}", mag[tup.Item2]), string.Format("{0:.00}", dur[tup.Item2]));

            lblPotionValue.Text = value.ToString();
        }

        public double GetEffectMagnitude(Effect ef)
        {
            switch (ef.Name)
            {
                case "Invisibility":
                case "Paralysis":
                case "Slow":
                case "Waterbreathing":
                case "Cure Disease":
                    return 0.0;
                default:
                    return ef.Mag *
                            4 *
                            (Math.Pow(1.5, Convert.ToDouble(spinAlchemyLevel.Value) / 100)) *
                            (1 + Convert.ToDouble(spinAlchemistPerk.Value) / 100) *
                            (1 + (chkPhysicianPerk.Checked && ef.Name.Contains("Restore") ? 0.25 : 0)) *
                            (1 + (chkPoisonerPerk.Checked && ef.IsBeneficial == 0 ? 0.25 : 0) + (chkBenefactorPerk.Checked && ef.IsBeneficial == 1 ? 0.25 : 0));
            }
        }

        public double GetEffectDuration(Effect ef)
        {
            switch (ef.Name)
            {
                case "Invisibility":
                case "Paralysis":
                case "Slow":
                case "Waterbreathing":
                    return ef.Dur *
                            4 *
                            (Math.Pow(1.5, Convert.ToDouble(spinAlchemyLevel.Value) / 100)) *
                            (1 + Convert.ToDouble(spinAlchemistPerk.Value) / 100) *
                            (1 + (chkPhysicianPerk.Checked && ef.Name.Contains("Restore") ? 0.25 : 0)) *
                            (1 + (chkPoisonerPerk.Checked && ef.IsBeneficial == 0 ? 0.25 : 0) + (chkBenefactorPerk.Checked && ef.IsBeneficial == 1 ? 0.25 : 0));
                default:
                    return ef.Dur;
            }
        }

        public int GetEffectValue(Effect ef, double mag, double dur)
        {
            var coef = 0.0794328;
            if (mag == 0.0 && dur == 0.0) mag = ef.Mag;
            return (int)Math.Floor(ef.Cost * (mag != 0.0 ? Math.Pow(mag, 1.1) : 1) * coef * (dur != 0.0 ? Math.Pow(dur, 1.1) : 1));
        }

        #endregion

    }
}
