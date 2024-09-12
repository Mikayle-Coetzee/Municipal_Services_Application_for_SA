using PROG7312_ST10023767.Classes;
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

namespace PROG7312_ST10023767.Views
{
    /// <summary>
    /// Interaction logic for MainReportUserControl.xaml
    /// </summary>
    public partial class MainReportUserControl : UserControl
    {
        /// <summary>
        /// Manages the issues reported by the user
        /// </summary>
        private IssueManager issueManager;

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Default Constructor 
        /// </summary>
        /// <param name="issueManager"></param>
        public MainReportUserControl(IssueManager issueManager)
        {
            InitializeComponent();
            this.issueManager = issueManager;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Handles the button click event for reporting an issue.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            string category = "";

            // Check which button was clicked and set the category accordingly
            switch (sender)
            {
                case Button btn when btn == btnReportUtilities:
                    category = "Utilities";
                    break;
                case Button btn when btn == btnReportSanitation:
                    category = "Sanitation";
                    break;
                case Button btn when btn == btnReportPotholes:
                    category = "Potholes";
                    break;
                case Button btn when btn == btnReportTraffic:
                    category = "Traffic";
                    break;
                case Button btn when btn == btnReportRoadSigns:
                    category = "Road signs";
                    break;
                case Button btn when btn == btnReportOtherIssue:
                    category = "Other issue";
                    break;
                case Button btn when btn == btnReportTrafficLights:
                    category = "Traffic lights";
                    break;
                case Button btn when btn == btnReportCarCrash:
                    category = "Car crash";
                    break;
            }

            
            // Navigate to the ReportIssueUserControl and set the selected category
            ReportIssueUserControl reportIssueUserControl = new ReportIssueUserControl(issueManager);
            reportIssueUserControl.SetCategory(category);
            ReportFrame.Navigate(reportIssueUserControl);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        ///  Handles the back button click event and hides the current UserControl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBackToMain_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;

        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Handles the click event for the "Back" button, hiding the current UserControl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
