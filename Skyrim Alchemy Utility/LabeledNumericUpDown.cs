using System.Windows.Forms;

namespace Skyrim_Alchemy_Utility
{
    public class LabeledNumericUpDown : NumericUpDown
    {
        public string Label;
        public LabeledNumericUpDown() : base() { }
        public LabeledNumericUpDown(string label)
            : base()
        {
            Label = label;
        }
        protected override void UpdateEditText()
        {
            this.Text = this.Value.ToString() + Label;
        }
    }
}
