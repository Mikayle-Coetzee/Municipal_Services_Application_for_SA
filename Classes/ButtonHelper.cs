using System.Windows;
using System.Windows.Controls;

namespace PROG7312_ST10023767.Classes
{
    public static class ButtonHelper
    {
        public static readonly DependencyProperty TagHoverProperty =
           DependencyProperty.RegisterAttached("TagHover", typeof(string), typeof(ButtonHelper), new PropertyMetadata(null));

        public static string GetTagHover(Button button)
        {
            return (string)button.GetValue(TagHoverProperty);
        }

        public static void SetTagHover(Button button, string value)
        {
            button.SetValue(TagHoverProperty, value);
        }

        public static readonly DependencyProperty TagClickProperty =
            DependencyProperty.RegisterAttached("TagClick", typeof(string), typeof(ButtonHelper), new PropertyMetadata(null));

        public static string GetTagClick(Button button)
        {
            return (string)button.GetValue(TagClickProperty);
        }

        public static void SetTagClick(Button button, string value)
        {
            button.SetValue(TagClickProperty, value);
        }
    }
}
