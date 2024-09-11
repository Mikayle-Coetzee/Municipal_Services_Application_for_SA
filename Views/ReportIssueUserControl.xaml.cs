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
        private IssueManagerClass issueManager;

        private List<string> attachedMediaPaths = new List<string>();

        private int currentIndex = 0;

        int progressValue = 25;
        bool isLocationValid = false, isDescriptionValid = false, isMediaUploaded = false;

        public ReportIssueUserControl(IssueManagerClass issueManager)
        {
            InitializeComponent();
            this.issueManager = issueManager;            
            btnSubmit.IsEnabled = false;

            ShowWelcomeMessage();
        }
        private async void ShowWelcomeMessage()
        {
            await Task.Delay(1500);
            AddChatBubble("Hello, you can submit a report by filling in the details on the right and clicking submit, or you can enter:\n" +
                          "1. Help (for support)\n" +
                          "2. View (for viewing all your reports submitted)\n" +
                          "3. New (for creating a new conversation)\n" +
                          "4. Search (for searching a specific report)",
                          isMedia: false,
                          isUser: false, isInput: false);
        }

        public string GetRichTextBoxText()
        {
            System.Windows.Documents.TextRange textRange = new System.Windows.Documents.TextRange(txtDescription.Document.ContentStart, txtDescription.Document.ContentEnd);
            return textRange.Text;
        }

        private async void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string location = txtLocation.Text;
            string category = (cmbCategory.SelectedItem as ComboBoxItem)?.Content.ToString();
            string description = GetRichTextBoxText();

            if (string.IsNullOrEmpty(location) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Create an IssueClass instance
            IssueClass issue = new IssueClass(location, category, description);
            foreach (var mediaPath in attachedMediaPaths)
            {
                issue.AddAttachment(mediaPath);
            }

            // Add the issue to IssueManager
            issueManager.AddIssue(issue);

            string reportMessage = $"Location: {location}\nCategory: {category}\nDescription: {description}\nTime: {DateTime.Now}";

            AddChatBubble(reportMessage, isMedia: attachedMediaPaths.Any(), isUser: true, isInput: false);

            ClearFields();

            MessageBox.Show("Thank you for submitting a report.");

            // Auto-Response Confirmation and Progress Simulation
            await AutoResponse(category);
        }

        private void ClearFields()
        {
            txtLocation.Clear();
            txtDescription.Document = new System.Windows.Documents.FlowDocument();
            cmbCategory.SelectedIndex = 0;
            attachedMediaPaths.Clear(); 
            reUploadIcon.Visibility = Visibility.Collapsed;
            lblReUploadMedia.Visibility = Visibility.Collapsed;
            btnReuploadFile.Visibility = Visibility.Collapsed;
            mediaContainer.Visibility = Visibility.Collapsed;
            btnNext.Visibility = Visibility.Collapsed;
            btnPrevious.Visibility = Visibility.Collapsed;
            isLocationValid = false;
            isDescriptionValid = false;
            isMediaUploaded = false;

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

            // Add automatic response bubble to the left side
            Border responseBubble = AddChatBubble($"Your report on {category} has been received. \nWe're processing it.\nEstimated time to assistance: 5 minutes.", isMedia: false, isUser: false, isInput: false);

            // ProgressBar inside the response bubble with padding
            ProgressBar progressBar = new ProgressBar
            {
                Width = 250,     
                Height = 20,
                Maximum = 100,
                Value = 0,
                Foreground = (Brush)FindResource("redSolidColorBrush"),
                Background = (Brush)FindResource("offWhiteSolidColorBrush")
            };

            // Add ProgressBar to the response bubble
            (responseBubble.Child as StackPanel).Children.Add(progressBar);

            // Simulate the progress bar update for 5 minutes (300 seconds)
            for (int i = 0; i <= 100; i++)
            {
                progressBar.Value = i;
                if (i == 100)
                {
                    progressBar.Foreground = (Brush)FindResource("greenSolidColorBrush");
                }
                await Task.Delay(3000); 
            }

            // After 5 minutes, update the chat bubble with a final message
            AddChatBubble($"Your report on {category} has been submitted successfully. \nYou will be contacted soon.", isMedia: false, isUser: false, isInput: false);
        }

        private Border AddChatBubble(string message, bool isMedia, bool isUser, bool isInput)
        {
            Border chatBubble = new Border
            {
                Background = isUser ? (Brush)FindResource("greenSolidColorBrush") : (Brush)FindResource("offWhiteSolidColorBrush"),
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(10), 
                Margin = new Thickness(5),
                MaxWidth = 350,              
                HorizontalAlignment = isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left  
            };

            StackPanel bubbleContent = new StackPanel();

            if (isMedia && attachedMediaPaths.Any())
            {
                foreach (var mediaPath in attachedMediaPaths)
                {
                    DisplayAttachedMedia(bubbleContent, mediaPath);
                }
            }

            if (isUser)
            {
                TextBlock issueType = new TextBlock
                {
                    Text = message.Split('\n')[1], // Category
                    FontWeight = FontWeights.Bold,
                    Foreground = (Brush)FindResource("darkestSolidColorBrush")
                };
                bubbleContent.Children.Add(issueType);

                TextBlock locationText = new TextBlock
                {
                    Text = message.Split('\n')[0], // Location
                    FontWeight = FontWeights.Bold,
                    Foreground = (Brush)FindResource("darkestSolidColorBrush")
                };
                bubbleContent.Children.Add(locationText);

                TextBlock descriptionText = new TextBlock
                {
                    Text = message.Split('\n')[2], // Description
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 5, 0, 5)
                };
                bubbleContent.Children.Add(descriptionText);
                
                // Timestamp at the bottom
                TextBlock timestamp = new TextBlock
                {
                    Text = message.Split('\n')[3], // Time
                    FontSize = 10,
                    Foreground = (Brush)FindResource("darkSolidColorBrush"),
                    HorizontalAlignment = HorizontalAlignment.Right
                };
                bubbleContent.Children.Add(timestamp);
            }
            else if (isUser == false && isInput == true)
            {
                // TextBlock message
                TextBlock messageText = new TextBlock
                {
                    Text = message,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 5, 0, 5)
                };
                bubbleContent.Children.Add(messageText);

                // TextBox for location input
                TextBox tbxLocation = new TextBox
                {
                    Margin = new Thickness(0, 5, 0, 5)
                };
                bubbleContent.Children.Add(tbxLocation);

                // ComboBox for category selection
                ComboBox cmbCategory = new ComboBox
                {
                    Margin = new Thickness(0, 5, 0, 5)
                };
                cmbCategory.Items.Add("Traffic");
                cmbCategory.Items.Add("Water Issue");
                cmbCategory.Items.Add("Electricity Issue");
                bubbleContent.Children.Add(cmbCategory);

                // Search button
                Button btnSearch = new Button
                {
                    Content = "Search",
                    Margin = new Thickness(0, 5, 0, 5)
                };
                btnSearch.Click += (s, e) =>
                {
                    string location = tbxLocation.Text;
                    string category = cmbCategory.SelectedValue.ToString();
                    if (!string.IsNullOrEmpty(location) && !string.IsNullOrEmpty(category))
                    {
                        DisplaySearchedReport(location, category);
                    }
                    else
                    {
                        MessageBox.Show("Please enter a location and select a category.");
                    }
                };
                bubbleContent.Children.Add(btnSearch);
            }
            else if (isUser == false)
            {
                TextBlock descriptionText = new TextBlock
                {
                    Text = message, // Description
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 5, 0, 5)
                };
                bubbleContent.Children.Add(descriptionText);

            }

            // Add the content to the chat bubble
            chatBubble.Child = bubbleContent;

            // Add the chat bubble to the ChatHistoryPanel
            ChatHistoryPanel.Children.Add(chatBubble);

            return chatBubble;
        }

        private void DisplayAttachedMedia(StackPanel bubbleContent, string mediaPath)
        {
            string extension = System.IO.Path.GetExtension(mediaPath).ToLower();

            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
            {
                Image mediaImage = new Image
                {
                    Width = 150,
                    Height = 150,
                    Source = new BitmapImage(new Uri(mediaPath)),
                    Stretch = Stretch.UniformToFill,
                    Margin = new Thickness(0, 0, 0, 5)
                };
                bubbleContent.Children.Add(mediaImage);
            }
            else if (extension == ".mp4")
            {
                MediaElement mediaVideo = new MediaElement
                {
                    Width = 200,
                    Height = 150,
                    Source = new Uri(mediaPath),
                    LoadedBehavior = MediaState.Manual,
                    UnloadedBehavior = MediaState.Close,
                    Margin = new Thickness(0, 0, 0, 5)
                };
                mediaVideo.Play();
                bubbleContent.Children.Add(mediaVideo);
            }
            else if (extension == ".pdf" || extension == ".docx")
            {
                TextBlock documentLink = new TextBlock
                {
                    Text = "Open Document: " + System.IO.Path.GetFileName(mediaPath),
                    FontStyle = FontStyles.Italic,
                    Foreground = Brushes.Blue,
                    Margin = new Thickness(0, 0, 0, 5),
                    Cursor = Cursors.Hand,
                    TextDecorations = TextDecorations.Underline
                };

                documentLink.MouseLeftButtonUp += (s, e) =>
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = mediaPath,
                        UseShellExecute = true
                    });
                };

                bubbleContent.Children.Add(documentLink);
            }
        }
       
        private void DisplayUploadedMedia()
        {
            // Check if the mediaPaths list is null or empty
            if (attachedMediaPaths == null || !attachedMediaPaths.Any())
            {
                lblReUploadMedia.Visibility = Visibility.Collapsed;
                return;
            }

            // Set up media display container
            mediaContainer.Visibility = Visibility.Visible;
            mediaContainer.Children.Clear();

            // Create a ContentControl to display the current media
            ContentControl mediaDisplay = new ContentControl
            {
                Width = 200,  // Fixed width
                Height = 150, // Fixed height
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            // Populate mediaDisplay with the current media item
            LoadMedia(currentIndex, mediaDisplay);

            // Add mediaDisplay to the container
            mediaContainer.Children.Add(mediaDisplay);

            btnNext.Visibility = Visibility.Visible;
            btnPrevious.Visibility = Visibility.Visible;

            btnPrevious.Click += (s, e) => NavigateMedia(-1, mediaDisplay);
            btnNext.Click += (s, e) => NavigateMedia(1, mediaDisplay);



            // Show re-upload options
            lblReUploadMedia.Visibility = Visibility.Visible;
            reUploadIcon.Visibility = Visibility.Visible;
            btnReuploadFile.Visibility = Visibility.Visible;
        }
        private void LoadMedia(int index, ContentControl mediaDisplay)
        {
            if (attachedMediaPaths == null || !attachedMediaPaths.Any() || index < 0 || index >= attachedMediaPaths.Count)
                return;

            string mediaPath = attachedMediaPaths[index];
            string extension = System.IO.Path.GetExtension(mediaPath).ToLower();

            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
            {
                // Display the image with uniform size
                BitmapImage bitmap = new BitmapImage(new Uri(mediaPath, UriKind.Absolute));
                Image img = new Image
                {
                    Source = bitmap,
                    Width = mediaDisplay.Width,
                    Height = mediaDisplay.Height,
                    Stretch = Stretch.Fill // Ensure the image fills the space and keeps proportions
                };
                mediaDisplay.Content = img;
            }
            else if (extension == ".mp4")
            {
                MediaElement mediaVideo = new MediaElement
                {
                    Width = mediaDisplay.Width,
                    Height = mediaDisplay.Height,
                    Source = new Uri(mediaPath, UriKind.Absolute),
                    LoadedBehavior = MediaState.Manual,
                    UnloadedBehavior = MediaState.Close,
                    Stretch = Stretch.Fill
                };

                mediaDisplay.Content = mediaVideo;
                mediaVideo.Play();
            }
            else if (extension == ".pdf" || extension == ".docx")
            {
                TextBlock documentLink = new TextBlock
                {
                    Text = "Open Document: " + System.IO.Path.GetFileName(mediaPath),
                    FontStyle = FontStyles.Italic,
                    Foreground = Brushes.Blue,
                    Cursor = Cursors.Hand,
                    TextDecorations = TextDecorations.Underline,
                    TextWrapping = TextWrapping.Wrap
                };

                documentLink.MouseLeftButtonUp += (s, e) =>
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = mediaPath,
                        UseShellExecute = true
                    });
                };

                mediaDisplay.Content = documentLink;
            }
            else
            {
                TextBlock unsupportedMedia = new TextBlock
                {
                    Text = "Unsupported media type: " + System.IO.Path.GetFileName(mediaPath),
                    Foreground = Brushes.Red
                };

                mediaDisplay.Content = unsupportedMedia;
            }
        }

        private void NavigateMedia(int direction, ContentControl mediaDisplay)
        {
            // Update currentIndex based on direction
            currentIndex += direction;

            // Ensure index is within bounds
            if (currentIndex < 0)
                currentIndex = 0;
            else if (currentIndex >= attachedMediaPaths.Count)
                currentIndex = attachedMediaPaths.Count - 1;

            // Reload media with updated index
            LoadMedia(currentIndex, mediaDisplay);
        }

     
        private void btnReuploadFile_Click(object sender, RoutedEventArgs e)
        {
            attachedMediaPaths.Clear();
            btnAttachFile_Click(sender, e);
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
            string command = txtMessage.Text;
            switch (command.ToLower())
            {
                case "1":
                case "help":
                    AddChatBubble("You can submit a report by filling in the details on the right and clicking submit, I hope this helps.", isMedia: false, isUser: false, isInput: false);
                    break;
                case "2":
                case "view":
                    AddChatBubble("Here are all your submitted reports:", isMedia: false, isUser: false, isInput: false);
                    DisplayAllReports();
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
                    AddChatBubble("Sorry, I didn't understand that command. Please enter a valid option.", isMedia: false, isUser: false, isInput: false);
                    break;
            }

            txtMessage.Clear();
        }

        private void ShowSearchPopup()
        {
            AddChatBubble("Please enter the location and select a category to search.", isMedia: false, isUser: false, isInput: true);
        }

        private void txtLocation_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                if (!isLocationValid) 
                {
                    progressValue += 25;
                    progressBar.Value = progressValue;
                    isLocationValid = true;
                }
            }
            else
            {
                if (isLocationValid) 
                {
                    progressValue -= 25;
                    progressBar.Value = progressValue;
                    isLocationValid = false;
                }
            }

            CheckSubmitButtonVisibility();
        }

        private void cmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }

        private void txtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(GetRichTextBoxText()))
            {
                if (!isDescriptionValid)
                {
                    progressValue += 25;
                    progressBar.Value = progressValue;
                    isDescriptionValid = true;
                }
            }
            else
            {
                if (isDescriptionValid)
                {
                    progressValue -= 25;
                    progressBar.Value = progressValue;
                    isDescriptionValid = false;
                }
            }

            CheckSubmitButtonVisibility();
        }

        private void DisplayAllReports()
        {
            // Fetch all reports from IssueManager
            var issues = issueManager.GetIssues();

            foreach (var issue in issues)
            {
                // Construct a report message from the IssueClass properties
                string reportMessage = $"Location: {issue.Location}\nCategory: {issue.Category}\nDescription: {issue.Description}\nTime: {issue.Timestamp}";

                // Add each report as a chat bubble with media if present
                Border chatBubble = AddChatBubble(reportMessage, isMedia: issue.Attachments.Any(), isUser: false, isInput: false);

                // If the issue has attachments, display them
                if (issue.Attachments.Any())
                {
                    // Create a StackPanel to hold the attachments
                    StackPanel mediaContainer = new StackPanel
                    {
                        Orientation = Orientation.Vertical
                    };

                    // Display each attachment
                    foreach (var mediaPath in issue.Attachments)
                    {
                        DisplayAttachedMedia(mediaContainer, mediaPath);
                    }

                    // Append the media container to the chat bubble
                    (chatBubble.Child as StackPanel)?.Children.Add(mediaContainer);
                }
            }
        }

        private void DisplaySearchedReport(string reportLocation, string reportCategory)
        {
            bool found = false;
            var issues = issueManager.GetIssues();

            foreach (var issue in issues)
            {
                if (issue.Category == reportCategory && issue.Location == reportLocation)
                {
                    // Construct a report message from the IssueClass properties
                    string reportMessage = $"Location: {issue.Location}\nCategory: {issue.Category}\nDescription: {issue.Description}\nTime: {issue.Timestamp}";

                    // Add each report as a chat bubble with media if present
                    Border chatBubble = AddChatBubble(reportMessage, isMedia: issue.Attachments.Any(), isUser: false, isInput: false);

                    // If the issue has attachments, display them
                    if (issue.Attachments.Any())
                    {
                        // Create a StackPanel to hold the attachments
                        StackPanel mediaContainer = new StackPanel
                        {
                            Orientation = Orientation.Vertical
                        };

                        // Display each attachment
                        foreach (var mediaPath in issue.Attachments)
                        {
                            DisplayAttachedMedia(mediaContainer, mediaPath);
                        }

                        // Append the media container to the chat bubble
                        (chatBubble.Child as StackPanel)?.Children.Add(mediaContainer);
                    }
                    found = true;
                    break;
                }
                else
                {
                    found = false;
                }

            }

            if (!found)
            {
                Border chatBubble = AddChatBubble($"Sorry, I can not find a report reported at {reportLocation} with {reportCategory} category. Please re-enter and try again.", isMedia: false, isUser: false, isInput: false);
            }
        }
        private void CheckSubmitButtonVisibility()
        {
            if ((progressValue == 75 && !isMediaUploaded)|| (progressValue == 100))
            {
                btnSubmit.IsEnabled = true;
            }
            else
            {
                btnSubmit.IsEnabled = false;
            }
        }
        private void btnAttachFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png|All Files|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filePath in openFileDialog.FileNames)
                {
                    attachedMediaPaths.Add(filePath);
                }

                if (!isMediaUploaded)
                {
                    progressValue += 25;
                    progressBar.Value = progressValue;
                    isMediaUploaded = true;
                }

                CheckSubmitButtonVisibility();
                DisplayUploadedMedia();
            }
        }
    }
}
