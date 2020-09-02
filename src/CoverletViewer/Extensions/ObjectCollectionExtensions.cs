using CoverletViewer.Controls;
using static System.Windows.Forms.ComboBox;

namespace CoverletViewer.Extensions
{
    public static class ObjectCollectionExtensions
    {
        public static void AddItem(this ObjectCollection source, double value, string text)
        {
            var item = new ComboBoxItem(value, text);
            source.Add(item);
        }
    }
}
