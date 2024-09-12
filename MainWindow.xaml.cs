using PROG7312_ST10023767.Classes;
using PROG7312_ST10023767.Views;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PROG7312_ST10023767
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IssueManager issueManager;

        public MainWindow()
        {
            InitializeComponent();
            issueManager = new IssueManager();

        }

        /// <summary>
        /// This was intended to navigate to the MainReportUserControl, allowing users to select a category by clicking a button.
        /// But, according to the POE requirements, users must be redirected to the report submission page instead...so this 
        /// directs the user to the report issue submission page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReportIssues_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the ReportIssueUserControl view
            ReportIssueUserControl reportIssue = new ReportIssueUserControl(issueManager);

            // Set the visibility of the reportIssue section to visible
            reportIssue.Visibility = Visibility.Visible;

            // Navigate to the reportIssue using the MainFrame
            MainFrame.Navigate(content: reportIssue);
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
