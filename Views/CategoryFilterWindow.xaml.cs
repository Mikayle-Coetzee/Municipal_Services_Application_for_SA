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
        public string SelectedCategory { get; private set; }

        public CategoryFilterWindow()
        {
            InitializeComponent();

        }

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


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;  
        }
    }
}
