using PROG7312_ST10023767.Classes;
using PROG7312_ST10023767.Views;
using System.Windows;

namespace PROG7312_ST10023767
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IssueManager issueManager;
        private PostManager postManager;

        public MainWindow()
        {
            InitializeComponent();
            issueManager = new IssueManager();
            postManager = new PostManager();
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

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Navigates to the EventsUserControl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLocalEvents_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the EventsUserControl view
            EventsUserControl events = new EventsUserControl(postManager, issueManager);

            // Set the visibility of the events section to visible
            events.Visibility = Visibility.Visible;

            // Navigate to the events using the MainFrame
            MainFrame.Navigate(content: events);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// To be implemented in part 3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnServiceRequestStatus_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("The 'Service Request Status' Feature Is Coming");
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
