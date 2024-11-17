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

        private PostManager postManager;
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
        public ReportIssueUserControl(IssueManager issueManager, IssueTracker issueTracker , PostManager postManager)
        {
            InitializeComponent();
            this.issueManager = issueManager;
            this.issueTracker = issueTracker;
            this.postManager = postManager;
            this.chatService = new ChatService(ChatHistoryPanel, issueManager, attachedMediaPaths);
            this.mediaManager = new MediaService(attachedMediaPaths);

            SubmitEventButton.IsEnabled = false;
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
            return textRange.Text.Trim();  
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
                GetRichTextBoxText(), "0");   

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
            lblMotivation.Content = "You're off to a great start! Category selected.";
            ResetValidationFlags();
            UpdateProgressBar(1);
            attachedMediaPaths.Clear();
            MediaList.Items.Clear();
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
                "Great! You've entered the location!", "Oops! The location is missing.");
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
                "Awesome! You've described the issue!", "Please describe the issue.");
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
        private void RemoveMedia_Click(object sender, RoutedEventArgs e)
        {
            if (MediaList.SelectedItem != null)
            {
                string selectedMedia = MediaList.SelectedItem.ToString().Trim(); // Selected file name

                // Debugging: Check the selected media
                Console.WriteLine($"Selected media: {selectedMedia}");

                // Check if any item in the attachedMediaPaths list matches the file name
                bool itemFound = false;
                foreach (var mediaPath in attachedMediaPaths)
                {
                    // Extract the file name from the full file path
                    string fileName = System.IO.Path.GetFileName(mediaPath);

                    // Debugging: Check the file name from attached path
                    Console.WriteLine($"Comparing {selectedMedia} with {fileName}");

                    if (fileName.Equals(selectedMedia, StringComparison.OrdinalIgnoreCase))
                    {
                        // Found the media, remove it
                        attachedMediaPaths.Remove(mediaPath);
                        MediaList.Items.Remove(selectedMedia);

                        // Update progress or other UI updates
                        if (isMediaUploaded)
                        {
                            UpdateProgressBar(progressValue - 30); // Adjust the progress
                            isMediaUploaded = false;
                            lblMotivation.Content = "Media removed! You can still upload.";
                        }

                        itemFound = true;
                        MessageBox.Show("Media item removed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
                }

                if (!itemFound)
                {
                    MessageBox.Show("Media item not found in the list.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a media item to remove.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private void txtMessage_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Enables or disables the submit button based on progress values
        /// </summary>
        private void CheckSubmitButtonVisibility()
        {
            SubmitEventButton.IsEnabled = (progressValue == 61 && !isMediaUploaded) || (progressValue == 91);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Handles the file attachment logic and updates the media manager to display attached media
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAttachFile_Click(object sender, RoutedEventArgs e)
        {
            if (!isMediaUploaded)
            {
                UpdateProgressBar(progressValue + 30);
                isMediaUploaded = true;
                lblMotivation.Content = "Fantastic! Your report is ready!";
            }

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Media Files|*.jpg;*.jpeg;*.png;*.gif;*.mp4;*.avi;*.mov;*.doc;*.docx;*.pdf",
                Title = "Select Media Files"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filePath in openFileDialog.FileNames)
                {
                    attachedMediaPaths.Add(filePath);                   
                    MediaList.Items.Add(System.IO.Path.GetFileName(filePath));

                }
            }
        }


        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text == "Enter Message Here...")
            {
                textBox.Text = "";  
                textBox.Foreground = new SolidColorBrush(Colors.Black);  
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Enter Message Here...";  
                textBox.Foreground = new SolidColorBrush(Colors.Gray);  
            }
        }

    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//

