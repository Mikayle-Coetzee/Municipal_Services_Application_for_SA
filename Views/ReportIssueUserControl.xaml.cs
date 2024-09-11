using Microsoft.Win32;
using PROG7312_ST10023767.Classes;
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
using static PROG7312_ST10023767.Classes.IssueClass;

namespace PROG7312_ST10023767.Views
{
    /// <summary>
    /// Interaction logic for ReportIssueUserControl.xaml
    /// </summary>
    public partial class ReportIssueUserControl : UserControl
    {
        private IssueManager issueManager;
        private ChatService chatService;
        private List<string> attachedMediaPaths = new List<string>();
        private MediaService mediaManager;

        private int currentIndex = 0;

        int progressValue = 1;
        bool isLocationValid = false, isDescriptionValid = false, isMediaUploaded = false;

        public ReportIssueUserControl(IssueManager issueManager)
        {
            InitializeComponent();
            this.issueManager = issueManager;            
            this.chatService = new ChatService(ChatHistoryPanel, issueManager, attachedMediaPaths);
            this.mediaManager = new MediaService(attachedMediaPaths);
            
            btnSubmit.IsEnabled = false;
            ShowWelcomeMessage();
        }
        private async void ShowWelcomeMessage()
        {
            await Task.Delay(1500);
            chatService.AddChatBubble(
                "Hello! To submit a report, fill in the details and click submit. Alternatively, enter:\n" +
                "1. Help (for support)\n" +
                "2. View (to view submitted reports)\n" +
                "3. New (to start a new conversation)\n" +
                "4. Search (to search for a specific report)",
                isMedia: false,
                isUser: false,
                isInput: false);
        }


        public string GetRichTextBoxText()
        {
            TextRange textRange = new TextRange(txtDescription.Document.ContentStart, txtDescription.Document.ContentEnd);
            return textRange.Text.Trim();  // Trim unnecessary whitespace
        }

        private async void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields()) return;

            var issue = new IssueClass(txtLocation.Text, (cmbCategory.SelectedItem as ComboBoxItem)?.Content.ToString(), GetRichTextBoxText());
            foreach (var mediaPath in attachedMediaPaths)
            {
                issue.AddAttachment(mediaPath);
            }

            issueManager.AddIssue(issue);
            string reportMessage = $"Location: {txtLocation.Text}\nCategory: {(cmbCategory.SelectedItem as ComboBoxItem)?.Content.ToString()}\nDescription: {GetRichTextBoxText()}\nTime: {DateTime.Now}";

            chatService.AddChatBubble(reportMessage, isMedia: attachedMediaPaths.Any(), isUser: true, isInput: false);

            ClearFields();
            MessageBox.Show("Thank you for submitting a report.");
            await AutoResponse(issue.Category);
        }
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
        private void UpdateProgressBar(int value)
        {
            progressBar.Value = value;
            progressValue = value;
        }
        private void ClearTextInputs()
        {
            txtLocation.Clear();
            txtDescription.Document.Blocks.Clear();
            cmbCategory.SelectedIndex = 0;
            lblMotivation.Content = "You're off to a great start! A category has been selected.";
        }

        private void ResetMediaUI()
        {
            attachedMediaPaths.Clear();
            SetVisibility(Visibility.Collapsed, reUploadIcon, lblReUploadMedia, btnClearFile, mediaContainer, btnNext, btnPrevious);
        }

        private void ResetValidationFlags()
        {
            isLocationValid = false;
            isDescriptionValid = false;
            isMediaUploaded = false;
        }

        private void SetVisibility(Visibility visibility, params UIElement[] elements)
        {
            foreach (var element in elements)
            {
                element.Visibility = visibility;
            }
        }


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

        private async Task AutoResponse(string category)
        {
            await Task.Delay(1000);

            var responseBubble = chatService.AddChatBubble($"Your report on {category} has been received. Processing it now. Estimated time to assistance: 5 minutes.", isMedia: false, isUser: false, isInput: false);
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

            chatService.AddChatBubble($"Your report on {category} has been successfully submitted. We'll contact you soon.", isMedia: false, isUser: false, isInput: false);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Navigating to Main Menu...");
            this.Visibility = Visibility.Hidden;

        }

        private void btnBackToReports_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            string command = txtMessage.Text.ToLower();
            switch (command)
            {
                case "1":
                case "help":
                    chatService.AddChatBubble("You can submit a report by filling in the details and clicking submit.", isMedia: false, isUser: false, isInput: false);
                    break;
                case "2":
                case "view":
                    chatService.AddChatBubble("Here are your submitted reports:", isMedia: false, isUser: false, isInput: false);
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
                    chatService.AddChatBubble("Command not recognized. Please enter a valid option.", isMedia: false, isUser: false, isInput: false);
                    break;
            }

            txtMessage.Clear();
        }

        private void ShowSearchPopup()
        {
            chatService.AddChatBubble("Please enter the location and select a category to search.", isMedia: false, isUser: false, isInput: true);
        }

        private void txtLocation_TextChanged(object sender, TextChangedEventArgs e)
        {
            HandleTextChange(txtLocation.Text, ref isLocationValid, 30, "Great! You've entered the location. Keep going!", "Oops! The location is missing. Please enter it.");
        }

        private void cmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }

        private void txtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            HandleTextChange(GetRichTextBoxText(), ref isDescriptionValid, 30, "Awesome! You've described the issue. Almost there!", "Please describe the issue.");
        }

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

        private void HandleTextChange(string text, ref bool flag, int progress, string successMessage, string failMessage)
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
        private void CheckSubmitButtonVisibility()
        {
            btnSubmit.IsEnabled = (progressValue == 61 && !isMediaUploaded) || (progressValue == 91);
        }


        private void btnAttachFile_Click(object sender, RoutedEventArgs e)
        {
            mediaManager.AttachMedia();

            if (!isMediaUploaded)
            {
                UpdateProgressBar(progressValue + 30);
                isMediaUploaded = true;
                lblMotivation.Content = "Fantastic! Your report is ready. Click submit to complete!";
            }

            mediaManager.DisplayUploadedMedia(mediaContainer, new ContentControl(), btnNext, btnPrevious, lblReUploadMedia, reUploadIcon, btnClearFile);
        }
    }
}
