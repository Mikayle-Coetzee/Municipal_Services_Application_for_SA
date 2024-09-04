using Microsoft.Win32;
using PROG7312_ST10023767.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace PROG7312_ST10023767
{
    /// <summary>
    /// Interaction logic for ReportIssues.xaml
    /// </summary>
    public partial class ReportIssues : Window
    {
        private IssueManagerClass issueManager;

        public ReportIssues()
        {
            InitializeComponent();
            issueManager = new IssueManagerClass();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string location = txtLocation.Text;
            string category = (cmbCategory.SelectedItem as ComboBoxItem)?.Content.ToString();
            string description = rtbDescription.Document.ContentStart.GetTextInRun(LogicalDirection.Forward);

            IssueClass newIssue = new IssueClass(location, category, description);
            // Optionally add attachments here
            issueManager.AddIssue(newIssue);

            MessageBox.Show("Issue reported successfully!");
           
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();this.Close();
        }
    

        private void btnAttachFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                // Add the file path to the list of attachments
            }
        }
    }
}
