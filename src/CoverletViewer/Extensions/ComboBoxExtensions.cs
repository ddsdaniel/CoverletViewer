using CoverletViewer.Controls;
using System.Windows.Forms;

namespace CoverletViewer.Extensions
{
    public static class ComboBoxExtensions
    {
        public static double GetSelectedValue(this ComboBox source)
        {
            var item = (ComboBoxItem)source.SelectedItem;
            return item.Value;
        }

        public static void SelectByValue(this ComboBox source, double targetValue)
        {
            for (int i = 0; i < source.Items.Count; i++)
            {
                var item = (ComboBoxItem)source.Items[i];
                if (item.Value == targetValue)
                {
                    source.SelectedIndex = i;
                    return;
                }
            }
        }
    }
}
