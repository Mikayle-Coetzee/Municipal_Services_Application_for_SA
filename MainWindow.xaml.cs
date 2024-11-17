using PROG7312_ST10023767.Controllers;
using PROG7312_ST10023767.Models.Managers;
using PROG7312_ST10023767.Views;
using System.Windows;
using System.Windows.Controls;

namespace PROG7312_ST10023767
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IssueManager issueManager;
        private PostManager postManager;
        private IssueTracker issueTracker;
        private AddDummyDataClass AddDummyDataClass ;
         public MainWindow()
        {
            InitializeComponent();
            issueManager = new IssueManager();
            postManager = new PostManager();
            issueTracker = new IssueTracker();

            var addDummyData = new AddDummyDataClass();

            // Add dummy issues to the IssueManager
            addDummyData.AddDummyIssues(issueManager,issueTracker);

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
            // Navigate to the EventsUserControl view
            EventsUserControl events = new EventsUserControl(postManager, issueManager, issueTracker);

            // Set the visibility of the events section to visible
            events.Visibility = Visibility.Visible;
            events.BtnReportIssue.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            // Navigate to the events using the MainFrame
            MainFrame.Navigate(content: events);

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
            EventsUserControl events = new EventsUserControl(postManager, issueManager , issueTracker);

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
            // Navigate to the EventsUserControl view
            EventsUserControl events = new EventsUserControl(postManager, issueManager, issueTracker);

            // Set the visibility of the events section to visible
            events.Visibility = Visibility.Visible;
            events.BtnStats.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            // Navigate to the events using the MainFrame
            MainFrame.Navigate(content: events);
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
