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
        private IssueManager issueManager;

        public MainReportUserControl(IssueManager issueManager)
        {
            InitializeComponent();
            this.issueManager = issueManager;
        }
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


            // Navigate to the ReportIssueUserControl view
            ReportIssueUserControl reportIssueUserControl = new ReportIssueUserControl(issueManager);

            // Set the selected category in the ReportIssueUserControl's ComboBox
            reportIssueUserControl.SetCategory(category);

            // Navigate to the ReportIssueUserControl in the ReportFrame
            ReportFrame.Navigate(reportIssueUserControl);
        }


        private void btnBackToMain_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }
    }
}
