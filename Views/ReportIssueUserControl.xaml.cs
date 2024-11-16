using Microsoft.Win32;
using PROG7312_ST10023767.Controllers;
using PROG7312_ST10023767.Models;
using PROG7312_ST10023767.Models.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using static PROG7312_ST10023767.Models.IssueClass;

namespace PROG7312_ST10023767.Views
{
    /// <summary>
    /// Interaction logic for ReportIssueUserControl.xaml
    /// </summary>
    public partial class ReportIssueUserControl : UserControl
    {
        /// <summary>
        /// Manages the creation and storage of issues
        /// </summary>
        private IssueManager issueManager;

        private IssueTracker issueTracker;
        /// <summary>
        /// Manages the chat-based user interface for interacting with the issue report
        /// </summary>
        private ChatService chatService;

        /// <summary>
        /// Holds the paths of the media files attached to the issue
        /// </summary>
        private List<string> attachedMediaPaths = new List<string>();

        /// <summary>
        /// Manages the media operations such as attachment and display
        /// </summary>
        private MediaService mediaManager;

        /// <summary>
        /// Tracks the index of the current media item for navigation
        /// </summary>
        private int currentIndex = 0;

        /// <summary>
        /// Tracks the progress value for the progress bar
        /// </summary>
        int progressValue = 1;

        /// <summary>
        /// Flags for checking if location, description, and media have been validated
        /// </summary>
        bool isLocationValid = false, isDescriptionValid = false, isMediaUploaded = false;

        
        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Initializes a new instance of ReportIssueUserControl
        /// </summary>
        /// <param name="issueManager"></param>
        public ReportIssueUserControl(IssueManager issueManager, IssueTracker issueTracker)
        {
            InitializeComponent();
            this.issueManager = issueManager;
            this.issueTracker = issueTracker;
            this.chatService = new ChatService(ChatHistoryPanel, issueManager, attachedMediaPaths);
            this.mediaManager = new MediaService(attachedMediaPaths);

            btnSubmit.IsEnabled = false;
            ShowWelcomeMessage();
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Displays a welcome message when the user opens the report submission form
        /// </summary>
        private async void ShowWelcomeMessage()
        {
            await Task.Delay(1500);
            chatService.AddChatBubble(
                "Hey, there! Welcome to SA Reports. To submit a report, fill in the details and click submit. Alternatively, enter:\n" +
                "1. Help (for support)\n" +
                "2. View (to view submitted reports)\n" +
                "3. New (to start a new conversation)\n" +
                "4. Search (to search for a specific report)",
                isMedia: false,
                isUser: false,
                isInput: false);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Retrieves and returns the trimmed text from the RichTextBox used for the issue description
        /// </summary>
        /// <returns></returns>
        public string GetRichTextBoxText()
        {
            TextRange textRange = new TextRange(txtDescription.Document.ContentStart, txtDescription.Document.ContentEnd);
            return textRange.Text.Trim();  // Trim unnecessary whitespace
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Handles the logic for submitting the issue when the submit button is clicked
        /// Validates input fields, attaches media, and sends the report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields()) return;

            var issue = new IssueClass(txtLocation.Text, (cmbCategory.SelectedItem as ComboBoxItem)?.Content.ToString(), 
                GetRichTextBoxText(), 1);   

            foreach (var mediaPath in attachedMediaPaths)
            {
                issue.AddAttachment(mediaPath);
            }

            issueManager.AddIssue(issue);
            issueTracker.AddIssue(issue);

            string reportMessage = $"Location: {txtLocation.Text}\nCategory: " +
                $"{(cmbCategory.SelectedItem as ComboBoxItem)?.Content.ToString()}\nDescription: " +
                $"{GetRichTextBoxText()}\nTime: {DateTime.Now}";

            chatService.AddChatBubble(reportMessage, isMedia: attachedMediaPaths.Any(), isUser: true, isInput: false);

            ClearFields();
            MessageBox.Show("Thank you for submitting a report.");
            await AutoResponse(issue.Category);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Validates if all required fields are filled
        /// </summary>
        /// <returns></returns>
        private bool ValidateFields()
        {
            string location = txtLocation.Text;
            string category = (cmbCategory.SelectedItem as ComboBoxItem)?.Content.ToString();
            string description = GetRichTextBoxText();

            if (string.IsNullOrEmpty(location) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Please fill in all fields.");
                return false;
            }

            return true;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Clears all input fields after the report has been submitted
        /// </summary>
        private void ClearFields()
        {
            txtLocation.Clear();
            txtDescription.Document.Blocks.Clear();
            cmbCategory.SelectedIndex = 0;
            lblMotivation.Content = "You're off to a great start! A category has been selected.";
            ResetMediaUI();
            ResetValidationFlags();
            UpdateProgressBar(1);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Updates the progress bar value
        /// </summary>
        /// <param name="value"></param>
        private void UpdateProgressBar(int value)
        {
            progressBar.Value = value;
            progressValue = value;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Resets the media UI to its initial state
        /// </summary>
        private void ResetMediaUI()
        {
            attachedMediaPaths.Clear();
            SetVisibility(Visibility.Collapsed, reUploadIcon, lblReUploadMedia, btnClearFile, mediaContainer, 
                btnNext, btnPrevious);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Resets the validation flags for location, description, and media
        /// </summary>
        private void ResetValidationFlags()
        {
            isLocationValid = false;
            isDescriptionValid = false;
            isMediaUploaded = false;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Sets the visibility of multiple UI elements
        /// </summary>
        /// <param name="visibility"></param>
        /// <param name="elements"></param>
        private void SetVisibility(Visibility visibility, params UIElement[] elements)
        {
            foreach (var element in elements)
            {
                element.Visibility = visibility;
            }
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Sets the selected category in the ComboBox
        /// </summary>
        /// <param name="category"></param>
        public void SetCategory(string category)
        {
            foreach (ComboBoxItem item in cmbCategory.Items)
            {
                if (item.Content.ToString() == category)
                {
                    cmbCategory.SelectedItem = item;
                    break;
                }
            }
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Displays an automatic response after submitting a report
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        private async Task AutoResponse(string category)
        {
            await Task.Delay(1000);

            var responseBubble = chatService.AddChatBubble($"Your report on {category} has been received. " +
                $"Processing it now. Estimated time to assistance: 5 minutes.", isMedia: false, isUser: false, isInput: false);
            var progressBarInBubble = new ProgressBar
            {
                Width = 250,
                Height = 20,
                Maximum = 100,
                Value = 0,
                Foreground = (Brush)FindResource("redSolidColorBrush"),
                Background = (Brush)FindResource("offWhiteSolidColorBrush")
            };
            (responseBubble.Child as StackPanel).Children.Add(progressBarInBubble);

            for (int i = 0; i <= 100; i++)
            {
                progressBarInBubble.Value = i;
                if (i == 100)
                {
                    progressBarInBubble.Foreground = (Brush)FindResource("greenSolidColorBrush");
                }
                await Task.Delay(3000);
            }

            chatService.AddChatBubble($"Your report on {category} has been successfully submitted. We'll contact you soon.", 
                isMedia: false, isUser: false, isInput: false);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Handles the back button click event and hides the current UserControl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Directs the user to the main report page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBackToReports_Click(object sender, RoutedEventArgs e)
        {
            /*var mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                var mainReportControl = new MainReportUserControl(issueManager);

                mainWindow.MainFrame.Content = mainReportControl;
            }*/
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Sends a command based on user input to either help, view, start new conversation, or search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            string command = txtMessage.Text.ToLower();
            switch (command)
            {
                case "1":
                case "help":
                    chatService.AddChatBubble("You can submit a report by filling in the details and clicking submit.", 
                        isMedia: false, isUser: false, isInput: false);
                    break;
                case "2":
                case "view":
                    chatService.AddChatBubble("Here are your submitted reports:",
                        isMedia: false, isUser: false, isInput: false);
                    chatService.DisplayAllReports();
                    break;
                case "3":
                case "new":
                    ChatHistoryPanel.Children.Clear();
                    ShowWelcomeMessage();
                    break;
                case "4":
                case "search":
                    ShowSearchPopup();
                    break;
                default:
                    chatService.AddChatBubble("Command not recognized. Please enter a valid option.", 
                        isMedia: false, isUser: false, isInput: false);
                    break;
            }

            txtMessage.Clear();
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Displays a search input prompt to allow the user to search for a report
        /// </summary>
        private void ShowSearchPopup()
        {
            chatService.AddChatBubble("Please enter the location and select a category to search.", 
                isMedia: false, isUser: false, isInput: true);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Handles text changes in the location field and updates the progress bar accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLocation_TextChanged(object sender, TextChangedEventArgs e)
        {
            HandleTextChange(txtLocation.Text, ref isLocationValid, 30, 
                "Great! You've entered the location. Keep going!", "Oops! The location is missing. Please enter it.");
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Handles text changes in the description field and updates the progress bar accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            HandleTextChange(GetRichTextBoxText(), ref isDescriptionValid, 30,
                "Awesome! You've described the issue. Almost there!", "Please describe the issue.");
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Handles the logic for clearing attached files and updating the UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearFile_Click(object sender, RoutedEventArgs e)
        {
            attachedMediaPaths.Clear();
            if (isMediaUploaded)
            {
                UpdateProgressBar(progressValue - 30);
                isMediaUploaded = false;
                lblMotivation.Content = "Media removed! You can still upload something if needed.";
            }
            ResetMediaUI();
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Handles the logic for text change, updates progress bar, and modifies validation flags
        /// </summary>
        /// <param name="text"></param>
        /// <param name="flag"></param>
        /// <param name="progress"></param>
        /// <param name="successMessage"></param>
        /// <param name="failMessage"></param>
        private void HandleTextChange(string text, ref bool flag, int progress, string successMessage,
            string failMessage)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (!flag)
                {
                    UpdateProgressBar(progressValue + progress);
                    flag = true;
                    lblMotivation.Content = successMessage;
                }
            }
            else
            {
                if (flag)
                {
                    UpdateProgressBar(progressValue - progress);
                    flag = false;
                    lblMotivation.Content = failMessage;
                }
            }

            CheckSubmitButtonVisibility();
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Enables or disables the submit button based on progress values
        /// </summary>
        private void CheckSubmitButtonVisibility()
        {
            btnSubmit.IsEnabled = (progressValue == 61 && !isMediaUploaded) || (progressValue == 91);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Handles the file attachment logic and updates the media manager to display attached media
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAttachFile_Click(object sender, RoutedEventArgs e)
        {
            mediaManager.AttachMedia();

            if (!isMediaUploaded)
            {
                UpdateProgressBar(progressValue + 30);
                isMediaUploaded = true;
                lblMotivation.Content = "Fantastic! Your report is ready. Click submit to complete!";
            }

            mediaManager.DisplayUploadedMedia(mediaContainer, new ContentControl(), btnNext, btnPrevious,
                lblReUploadMedia, reUploadIcon, btnClearFile);
        }

    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//

