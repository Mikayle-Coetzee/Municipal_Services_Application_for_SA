using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PROG7312_ST10023767.Views
{
    /// <summary>
    /// Interaction logic for CategoryFilterWindow.xaml
    /// </summary>
    public partial class CategoryFilterWindow : Window
    {
        /// <summary>
        /// Gets the category selected by the user.
        /// </summary>
        public string SelectedCategory { get; private set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Initializes a new instance of the CategoryFilterWindow class.
        /// </summary>
        public CategoryFilterWindow()
        {
            InitializeComponent();

        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Handles the click event for the "OK" button, validating the selected category and closing the dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (cmbCategory.SelectedItem != null)
            {
                
                SelectedCategory = (cmbCategory.SelectedItem as ComboBoxItem)?.Content.ToString();
                DialogResult = true; 
            }
            else
            {
                MessageBox.Show("Please select a category.");
            }
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Populates the category dropdown with unique categories provided.
        /// </summary>
        /// <param name="uniqueCategories"></param>
        public void PopulateCategories(HashSet<string> uniqueCategories)
        {
            cmbCategory.Items.Clear();

            foreach (var category in uniqueCategories)
            {
                cmbCategory.Items.Add(new ComboBoxItem
                {
                    Content = category
                });
                btnOK.IsEnabled = true;

            }

            if (uniqueCategories.Count == 0)
            {
                cmbCategory.Items.Add(new ComboBoxItem
                {
                    Content = "No Categories Yet"
                });
                btnOK.IsEnabled = false;
            }
            cmbCategory.SelectedIndex = 0;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Handles the click event for the "Cancel" button, closing the dialog without applying changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;  
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//

