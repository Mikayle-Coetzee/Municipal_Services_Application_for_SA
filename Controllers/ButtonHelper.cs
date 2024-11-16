using System.Windows;
using System.Windows.Controls;

namespace PROG7312_ST10023767.Controllers
{
    /// <summary>
    /// Provides attached properties to extend button functionality
    /// </summary>
    public static class ButtonHelper
    {
        /// <summary>
        /// A DependencyProperty that allows a string to be associated with a button for hover events
        /// </summary>
        public static readonly DependencyProperty TagHoverProperty =
           DependencyProperty.RegisterAttached("TagHover", typeof(string), typeof(ButtonHelper), new PropertyMetadata(null));

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Gets the value of the TagHoverProperty attached to a button
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static string GetTagHover(Button button)
        {
            return (string)button.GetValue(TagHoverProperty);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Sets the value of the TagHoverProperty for a button
        /// </summary>
        /// <param name="button"></param>
        /// <param name="value"></param>
        public static void SetTagHover(Button button, string value)
        {
            button.SetValue(TagHoverProperty, value);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// A DependencyProperty that allows a string to be associated with a button for click events
        /// </summary>
        public static readonly DependencyProperty TagClickProperty =
            DependencyProperty.RegisterAttached("TagClick", typeof(string), typeof(ButtonHelper), new PropertyMetadata(null));

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Gets the value of the TagClickProperty attached to a button
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static string GetTagClick(Button button)
        {
            return (string)button.GetValue(TagClickProperty);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Sets the value of the TagClickProperty for a button
        /// </summary>
        /// <param name="button"></param>
        /// <param name="value"></param>
        public static void SetTagClick(Button button, string value)
        {
            button.SetValue(TagClickProperty, value);
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
