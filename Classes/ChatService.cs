using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static PROG7312_ST10023767.Classes.IssueManager;

namespace PROG7312_ST10023767.Classes
{
    public class ChatService
    {
        private StackPanel chatHistoryPanel;
        private IssueManager issueManager;
        private List<string> attachedMediaPaths;
        public ChatService(StackPanel chatHistoryPanel, IssueManager issueManager, List<string> attachedMediaPaths)
        {
            this.chatHistoryPanel = chatHistoryPanel;
            this.issueManager = issueManager;
            this.attachedMediaPaths = attachedMediaPaths;

        }

        public Border AddChatBubble(string message, bool isMedia, bool isUser, bool isInput)
        {
            Border chatBubble = new Border
            {
                Background = isUser ? (Brush)Application.Current.FindResource("greenSolidColorBrush") : (Brush)Application.Current.FindResource("offWhiteSolidColorBrush"),
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(10),
                Margin = new Thickness(5),
                MaxWidth = 350,
                HorizontalAlignment = isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left
            };

            StackPanel bubbleContent = new StackPanel();

            // Handle media attachments
            if (isMedia && attachedMediaPaths != null && attachedMediaPaths.Count > 0)
            {
                foreach (var mediaPath in attachedMediaPaths)
                {
                    DisplayAttachedMedia(bubbleContent, mediaPath);
                }
            }

            // If the user has submitted input
            if (isUser)
            {
                // Split the message to display different fields (e.g., Location, Category, Description)
                string[] messageParts = message.Split('\n');

                if (messageParts.Length > 0)
                {
                    TextBlock locationText = new TextBlock
                    {
                        Text = messageParts[0], // Location
                        FontWeight = FontWeights.Bold,
                        Foreground = (Brush)Application.Current.FindResource("darkestSolidColorBrush")
                    };
                    bubbleContent.Children.Add(locationText);
                }

                if (messageParts.Length > 1)
                {
                    TextBlock issueType = new TextBlock
                    {
                        Text = messageParts[1], // Category
                        FontWeight = FontWeights.Bold,
                        Foreground = (Brush)Application.Current.FindResource("darkestSolidColorBrush")
                    };
                    bubbleContent.Children.Add(issueType);
                }

                if (messageParts.Length > 2)
                {
                    TextBlock descriptionText = new TextBlock
                    {
                        Text = messageParts[2], // Description
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 5, 0, 5)
                    };
                    bubbleContent.Children.Add(descriptionText);
                }

                if (messageParts.Length > 3)
                {
                    // Timestamp at the bottom
                    TextBlock timestamp = new TextBlock
                    {
                        Text = messageParts[3], // Time
                        FontSize = 10,
                        Foreground = (Brush)Application.Current.FindResource("darkSolidColorBrush"),
                        HorizontalAlignment = HorizontalAlignment.Right
                    };
                    bubbleContent.Children.Add(timestamp);
                }
            }
            else if (!isUser && isInput)
            {
                // Display input fields (TextBox and ComboBox) and search functionality
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

                // Populate ComboBox with enum values
                foreach (var category in Enum.GetValues(typeof(ReportCategory)))
                {
                    cmbCategory.Items.Add(category);
                }

                // Add ComboBox to the UI
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
                    string category = cmbCategory.SelectedValue?.ToString();
                    if (!string.IsNullOrEmpty(location) && !string.IsNullOrEmpty(category))
                    {
                        DisplaySearchedReport(location, category); // This method should be implemented elsewhere in your application
                    }
                    else
                    {
                        MessageBox.Show("Please enter a location and select a category.");
                    }
                };
                bubbleContent.Children.Add(btnSearch);
            }
            else
            {
                // Display simple message for system-generated bubbles
                TextBlock descriptionText = new TextBlock
                {
                    Text = message, // Simple system message
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 5, 0, 5)
                };
                bubbleContent.Children.Add(descriptionText);
            }

            // Add the content to the chat bubble
            chatBubble.Child = bubbleContent;

            // Add the chat bubble to the ChatHistoryPanel
            chatHistoryPanel.Children.Add(chatBubble);

            return chatBubble;
        }
        private void DisplaySearchedReport(string reportLocation, string reportCategory)
        {
            bool found = false;
            var issues = this.issueManager.GetIssues();

            foreach (var issue in issues)
            {
                if (issue.Category == reportCategory && issue.Location == reportLocation)
                {
                    // Construct a report message from the IssueClass properties
                    string reportMessage = $"Location: {issue.Location}\nCategory: {issue.Category}\nDescription: {issue.Description}\nTime: {issue.Timestamp}";

                    // Add each report as a chat bubble with media if present
                    Border chatBubble = AddChatBubble(reportMessage, isUser: false, isMedia: issue.Attachments.Any(),isInput:false);

                    // If the issue has attachments, display them
                    if (issue.Attachments.Any())
                    {
                        StackPanel mediaContainer = new StackPanel
                        {
                            Orientation = Orientation.Vertical
                        };

                        // Display each attachment using MediaService
                        foreach (var mediaPath in issue.Attachments)
                        {
                            MediaService.DisplayMedia(mediaContainer, mediaPath);
                        }

                        // Append the media container to the chat bubble
                        (chatBubble.Child as StackPanel)?.Children.Add(mediaContainer);
                    }

                    found = true;
                    break;
                }
            }

            if (!found)
            {
                AddChatBubble($"Sorry, I cannot find a report for {reportLocation} in the {reportCategory} category. Please re-enter and try again.", isUser: false, isMedia: false, isInput: false);
            }
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

        public void DisplayAllReports()
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


    }
}
