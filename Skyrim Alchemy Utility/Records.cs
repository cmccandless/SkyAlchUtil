using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Skyrim_Alchemy_Utility
{
    public class DB
    {
        public Effect[] Effects;
        public Ingredient[] Ingredients;
        public Property[] Properties;
    }
    public class Ingredient
    {
        [XmlAttribute]
        public string ID;
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public int Value;
        [XmlAttribute]
        public float Weight;
        [XmlAttribute]
        public string Description;
        [XmlAttribute]
        public int DLC;

        [XmlIgnore]
        private List<Effect> _effects;

        [XmlIgnore]
        public List<Effect> Effects
        {
            get
            {
                if (_effects == null)
                    _effects = (from prop in MainForm.Properties
                                let ingId = prop.IngID
                                let efId = prop.EfID
                                where ingId == ID
                                let ef = MainForm.Effects[efId]
                                orderby ef.Name
                                select ef).DistinctBy(ef=>ef.ID).ToList();

                return _effects;
            }
            private set
            {
                _effects = value;
            }
        }

        [XmlIgnore]
        private List<Ingredient> _compatibleIngredients;

        [XmlIgnore]
        public List<Ingredient> CompatibleIngredients
        {
            get
            {
                if (_compatibleIngredients == null)
                {
                    var results = new List<Ingredient> { new Ingredient { Name = "<NONE>" } };
                    if (this.ID != null)
                        results.AddRange((from prop in MainForm.Properties
                                          let ingId = prop.IngID
                                          let efId = prop.EfID
                                          join effect in this.Effects
                                          on efId equals effect.ID
                                          where this.ID == null || !ingId.Equals(this.ID)
                                          let ing = MainForm.Ingredients[ingId]
                                          orderby ing.Name
                                          select ing).DistinctBy(ing => ing.ID));
                    _compatibleIngredients = results;
                }
                return _compatibleIngredients;
            }
            private set
            {
                _compatibleIngredients = value;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public class Effect
    {
        [XmlAttribute]
        public string ID;
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public string Description;
        [XmlAttribute]
        public float Cost;
        [XmlAttribute]
        public int Mag;
        [XmlAttribute]
        public int Dur;
        [XmlAttribute]
        public int Value;
        [XmlAttribute]
        public int IsBeneficial;
    }

    public class Property
    {
        [XmlAttribute]
        public string EfID;
        [XmlAttribute]
        public string IngID;
        public string PK
        {
            get { return (IngID == null ? "" : IngID) + (EfID == null ? "" : EfID); }
        }
    }
}
