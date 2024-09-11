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

        private void btnReportIssues_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the MainReportUserControl view
            MainReportUserControl mainReport = new MainReportUserControl(issueManager);

            // Set the visibility of the mainReport section to visible
            mainReport.Visibility = Visibility.Visible;

            // Navigate to the mainReport using the MainFrame
            MainFrame.Navigate(content: mainReport);
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
