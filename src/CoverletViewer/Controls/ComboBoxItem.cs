namespace CoverletViewer.Controls
{
    public class ComboBoxItem
    {
        public double Value { get; private set; }
        public string Text { get; private set; }

        public ComboBoxItem(double value, string text)
        {
            Value = value;
            Text = text;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
