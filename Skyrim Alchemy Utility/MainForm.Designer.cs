namespace Skyrim_Alchemy_Utility
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlSplit = new System.Windows.Forms.SplitContainer();
            this.pnlFilterChk = new System.Windows.Forms.Panel();
            this.btnApplyFilter = new System.Windows.Forms.Button();
            this.pnlPotion = new System.Windows.Forms.Panel();
            this.lblPotionValue = new System.Windows.Forms.Label();
            this.dgvPotion = new System.Windows.Forms.DataGridView();
            this.colEffect = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMagnitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblValue = new System.Windows.Forms.Label();
            this.lblPotionClass = new System.Windows.Forms.Label();
            this.lblPotion = new System.Windows.Forms.Label();
            this.pnlTraits = new System.Windows.Forms.Panel();
            this.chkPurityPerk = new System.Windows.Forms.CheckBox();
            this.chkPoisonerPerk = new System.Windows.Forms.CheckBox();
            this.chkBenefactorPerk = new System.Windows.Forms.CheckBox();
            this.chkPhysicianPerk = new System.Windows.Forms.CheckBox();
            this.lblAlchemist = new System.Windows.Forms.Label();
            this.spinAlchemyLevel = new System.Windows.Forms.NumericUpDown();
            this.spinAlchemistPerk = new LabeledNumericUpDown();
            this.lblAlchemyLevel = new System.Windows.Forms.Label();
            this.lblTraits = new System.Windows.Forms.Label();
            this.pnlIngredients = new System.Windows.Forms.Panel();
            this.btnUpdateDb = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pnlSplit)).BeginInit();
            this.pnlSplit.Panel1.SuspendLayout();
            this.pnlSplit.Panel2.SuspendLayout();
            this.pnlSplit.SuspendLayout();
            this.pnlPotion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPotion)).BeginInit();
            this.pnlTraits.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinAlchemistPerk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinAlchemyLevel)).BeginInit();
            this.pnlIngredients.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSplit
            // 
            this.pnlSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSplit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.pnlSplit.IsSplitterFixed = true;
            this.pnlSplit.Location = new System.Drawing.Point(0, 0);
            this.pnlSplit.Name = "pnlSplit";
            // 
            // pnlSplit.Panel1
            // 
            this.pnlSplit.Panel1.Controls.Add(this.pnlFilterChk);
            this.pnlSplit.Panel1.Controls.Add(this.btnApplyFilter);
            // 
            // pnlSplit.Panel2
            // 
            this.pnlSplit.Panel2.Controls.Add(this.pnlPotion);
            this.pnlSplit.Panel2.Controls.Add(this.pnlTraits);
            this.pnlSplit.Panel2.Controls.Add(this.pnlIngredients);
            this.pnlSplit.Size = new System.Drawing.Size(692, 389);
            this.pnlSplit.SplitterDistance = 188;
            this.pnlSplit.TabIndex = 0;
            // 
            // pnlFilterChk
            // 
            this.pnlFilterChk.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFilterChk.AutoScroll = true;
            this.pnlFilterChk.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFilterChk.Location = new System.Drawing.Point(-1, 38);
            this.pnlFilterChk.Name = "pnlFilterChk";
            this.pnlFilterChk.Size = new System.Drawing.Size(190, 350);
            this.pnlFilterChk.TabIndex = 1;
            // 
            // btnApplyFilter
            // 
            this.btnApplyFilter.AutoSize = true;
            this.btnApplyFilter.Location = new System.Drawing.Point(57, 8);
            this.btnApplyFilter.Name = "btnApplyFilter";
            this.btnApplyFilter.Size = new System.Drawing.Size(75, 23);
            this.btnApplyFilter.TabIndex = 0;
            this.btnApplyFilter.Text = "Apply Filter";
            this.btnApplyFilter.UseVisualStyleBackColor = true;
            // 
            // pnlPotion
            // 
            this.pnlPotion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPotion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPotion.Controls.Add(this.lblPotionValue);
            this.pnlPotion.Controls.Add(this.dgvPotion);
            this.pnlPotion.Controls.Add(this.lblValue);
            this.pnlPotion.Controls.Add(this.lblPotionClass);
            this.pnlPotion.Controls.Add(this.lblPotion);
            this.pnlPotion.Location = new System.Drawing.Point(210, 218);
            this.pnlPotion.Name = "pnlPotion";
            this.pnlPotion.Size = new System.Drawing.Size(285, 166);
            this.pnlPotion.TabIndex = 2;
            // 
            // lblPotionValue
            // 
            this.lblPotionValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPotionValue.Location = new System.Drawing.Point(176, 25);
            this.lblPotionValue.Name = "lblPotionValue";
            this.lblPotionValue.Size = new System.Drawing.Size(100, 16);
            this.lblPotionValue.TabIndex = 12;
            this.lblPotionValue.Text = "N/A";
            this.lblPotionValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dgvPotion
            // 
            this.dgvPotion.AllowUserToAddRows = false;
            this.dgvPotion.AllowUserToDeleteRows = false;
            this.dgvPotion.AllowUserToResizeColumns = false;
            this.dgvPotion.AllowUserToResizeRows = false;
            this.dgvPotion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPotion.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPotion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPotion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colEffect,
            this.colMagnitude,
            this.colDuration});
            this.dgvPotion.Location = new System.Drawing.Point(8, 41);
            this.dgvPotion.Name = "dgvPotion";
            this.dgvPotion.RowHeadersVisible = false;
            this.dgvPotion.Size = new System.Drawing.Size(268, 120);
            this.dgvPotion.TabIndex = 11;
            // 
            // colEffect
            // 
            this.colEffect.HeaderText = "Effect";
            this.colEffect.Name = "colEffect";
            this.colEffect.ReadOnly = true;
            // 
            // colMagnitude
            // 
            this.colMagnitude.HeaderText = "Magnitude";
            this.colMagnitude.Name = "colMagnitude";
            this.colMagnitude.ReadOnly = true;
            // 
            // colDuration
            // 
            this.colDuration.HeaderText = "Duration";
            this.colDuration.Name = "colDuration";
            this.colDuration.ReadOnly = true;
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Location = new System.Drawing.Point(5, 25);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(37, 13);
            this.lblValue.TabIndex = 10;
            this.lblValue.Text = "Value:";
            // 
            // lblPotionClass
            // 
            this.lblPotionClass.AutoSize = true;
            this.lblPotionClass.Location = new System.Drawing.Point(47, 5);
            this.lblPotionClass.Name = "lblPotionClass";
            this.lblPotionClass.Size = new System.Drawing.Size(0, 13);
            this.lblPotionClass.TabIndex = 9;
            // 
            // lblPotion
            // 
            this.lblPotion.AutoSize = true;
            this.lblPotion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPotion.Location = new System.Drawing.Point(5, 5);
            this.lblPotion.Name = "lblPotion";
            this.lblPotion.Size = new System.Drawing.Size(40, 13);
            this.lblPotion.TabIndex = 9;
            this.lblPotion.Text = "Result:";
            // 
            // pnlTraits
            // 
            this.pnlTraits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlTraits.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTraits.Controls.Add(this.chkPurityPerk);
            this.pnlTraits.Controls.Add(this.chkPoisonerPerk);
            this.pnlTraits.Controls.Add(this.chkBenefactorPerk);
            this.pnlTraits.Controls.Add(this.chkPhysicianPerk);
            this.pnlTraits.Controls.Add(this.spinAlchemistPerk);
            this.pnlTraits.Controls.Add(this.lblAlchemist);
            this.pnlTraits.Controls.Add(this.spinAlchemyLevel);
            this.pnlTraits.Controls.Add(this.lblAlchemyLevel);
            this.pnlTraits.Controls.Add(this.lblTraits);
            this.pnlTraits.Location = new System.Drawing.Point(5, 218);
            this.pnlTraits.Name = "pnlTraits";
            this.pnlTraits.Size = new System.Drawing.Size(200, 166);
            this.pnlTraits.TabIndex = 0;
            // 
            // chkPurityPerk
            // 
            this.chkPurityPerk.AutoSize = true;
            this.chkPurityPerk.Location = new System.Drawing.Point(5, 135);
            this.chkPurityPerk.Name = "chkPurityPerk";
            this.chkPurityPerk.Size = new System.Drawing.Size(52, 17);
            this.chkPurityPerk.TabIndex = 8;
            this.chkPurityPerk.Text = "Purity";
            this.chkPurityPerk.UseVisualStyleBackColor = true;
            // 
            // chkPoisonerPerk
            // 
            this.chkPoisonerPerk.AutoSize = true;
            this.chkPoisonerPerk.Location = new System.Drawing.Point(5, 113);
            this.chkPoisonerPerk.Name = "chkPoisonerPerk";
            this.chkPoisonerPerk.Size = new System.Drawing.Size(67, 17);
            this.chkPoisonerPerk.TabIndex = 7;
            this.chkPoisonerPerk.Text = "Poisoner";
            this.chkPoisonerPerk.UseVisualStyleBackColor = true;
            // 
            // chkBenefactorPerk
            // 
            this.chkBenefactorPerk.AutoSize = true;
            this.chkBenefactorPerk.Location = new System.Drawing.Point(5, 91);
            this.chkBenefactorPerk.Name = "chkBenefactorPerk";
            this.chkBenefactorPerk.Size = new System.Drawing.Size(78, 17);
            this.chkBenefactorPerk.TabIndex = 6;
            this.chkBenefactorPerk.Text = "Benefactor";
            this.chkBenefactorPerk.UseVisualStyleBackColor = true;
            // 
            // chkPhysicianPerk
            // 
            this.chkPhysicianPerk.AutoSize = true;
            this.chkPhysicianPerk.Location = new System.Drawing.Point(5, 69);
            this.chkPhysicianPerk.Name = "chkPhysicianPerk";
            this.chkPhysicianPerk.Size = new System.Drawing.Size(71, 17);
            this.chkPhysicianPerk.TabIndex = 5;
            this.chkPhysicianPerk.Text = "Physician";
            this.chkPhysicianPerk.UseVisualStyleBackColor = true;
            // 
            // spinAlchemistPerk
            // 
            this.spinAlchemistPerk.Increment = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.spinAlchemistPerk.Location = new System.Drawing.Point(147, 44);
            this.spinAlchemistPerk.Name = "spinAlchemistPerk";
            this.spinAlchemistPerk.Size = new System.Drawing.Size(46, 20);
            this.spinAlchemistPerk.TabIndex = 4;
            this.spinAlchemistPerk.Label = "%";
            // 
            // lblAlchemist
            // 
            this.lblAlchemist.AutoSize = true;
            this.lblAlchemist.Location = new System.Drawing.Point(5, 46);
            this.lblAlchemist.Name = "lblAlchemist";
            this.lblAlchemist.Size = new System.Drawing.Size(113, 13);
            this.lblAlchemist.TabIndex = 3;
            this.lblAlchemist.Text = "Alchemist Perk Bonus:";
            // 
            // spinAlchemyLevel
            // 
            this.spinAlchemyLevel.Location = new System.Drawing.Point(147, 18);
            this.spinAlchemyLevel.Minimum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.spinAlchemyLevel.Name = "spinAlchemyLevel";
            this.spinAlchemyLevel.Size = new System.Drawing.Size(46, 20);
            this.spinAlchemyLevel.TabIndex = 2;
            this.spinAlchemyLevel.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // lblAlchemyLevel
            // 
            this.lblAlchemyLevel.AutoSize = true;
            this.lblAlchemyLevel.Location = new System.Drawing.Point(5, 20);
            this.lblAlchemyLevel.Name = "lblAlchemyLevel";
            this.lblAlchemyLevel.Size = new System.Drawing.Size(79, 13);
            this.lblAlchemyLevel.TabIndex = 1;
            this.lblAlchemyLevel.Text = "Alchemy Level:";
            // 
            // lblTraits
            // 
            this.lblTraits.AutoSize = true;
            this.lblTraits.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTraits.Location = new System.Drawing.Point(5, 5);
            this.lblTraits.Name = "lblTraits";
            this.lblTraits.Size = new System.Drawing.Size(85, 13);
            this.lblTraits.TabIndex = 0;
            this.lblTraits.Text = "Character Traits:";
            // 
            // pnlIngredients
            // 
            this.pnlIngredients.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlIngredients.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlIngredients.Controls.Add(this.btnUpdateDb);
            this.pnlIngredients.Location = new System.Drawing.Point(5, 5);
            this.pnlIngredients.Name = "pnlIngredients";
            this.pnlIngredients.Size = new System.Drawing.Size(490, 208);
            this.pnlIngredients.TabIndex = 2;
            // 
            // btnUpdateDb
            // 
            this.btnUpdateDb.Location = new System.Drawing.Point(385, 180);
            this.btnUpdateDb.Name = "btnUpdateDb";
            this.btnUpdateDb.Size = new System.Drawing.Size(100, 23);
            this.btnUpdateDb.TabIndex = 0;
            this.btnUpdateDb.Text = "Update Database";
            this.btnUpdateDb.UseVisualStyleBackColor = true;
            this.btnUpdateDb.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 389);
            this.Controls.Add(this.pnlSplit);
            this.MinimumSize = new System.Drawing.Size(708, 428);
            this.Name = "MainForm";
            this.Text = "Skyrim Alchemy Utility";
            this.pnlSplit.Panel1.ResumeLayout(false);
            this.pnlSplit.Panel1.PerformLayout();
            this.pnlSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlSplit)).EndInit();
            this.pnlSplit.ResumeLayout(false);
            this.pnlPotion.ResumeLayout(false);
            this.pnlPotion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPotion)).EndInit();
            this.pnlTraits.ResumeLayout(false);
            this.pnlTraits.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinAlchemistPerk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinAlchemyLevel)).EndInit();
            this.pnlIngredients.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer pnlSplit;
        private System.Windows.Forms.Button btnApplyFilter;
        private System.Windows.Forms.Panel pnlFilterChk;
        private System.Windows.Forms.Panel pnlIngredients;
        private System.Windows.Forms.Panel pnlTraits;
        private System.Windows.Forms.Panel pnlPotion;
        private System.Windows.Forms.Button btnUpdateDb;
        private System.Windows.Forms.Label lblTraits;
        private System.Windows.Forms.Label lblAlchemyLevel;
        private System.Windows.Forms.NumericUpDown spinAlchemyLevel;
        //private System.Windows.Forms.NumericUpDown spinAlchemistPerk;
        private LabeledNumericUpDown spinAlchemistPerk;
        private System.Windows.Forms.Label lblAlchemist;
        private System.Windows.Forms.CheckBox chkPhysicianPerk;
        private System.Windows.Forms.CheckBox chkBenefactorPerk;
        private System.Windows.Forms.CheckBox chkPoisonerPerk;
        private System.Windows.Forms.CheckBox chkPurityPerk;
        private System.Windows.Forms.Label lblPotion;
        private System.Windows.Forms.Label lblPotionClass;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.DataGridView dgvPotion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEffect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMagnitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDuration;
        private System.Windows.Forms.Label lblPotionValue;
    }
}

