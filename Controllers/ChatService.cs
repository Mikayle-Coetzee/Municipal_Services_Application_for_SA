using PROG7312_ST10023767.Models.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static PROG7312_ST10023767.Models.Managers.IssueManager;

namespace PROG7312_ST10023767.Controllers
{
    public class ChatService
    {
        /// <summary>
        /// Panel to hold the chat history
        /// </summary>
        private readonly StackPanel chatHistoryPanel;

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Manager for handling issue reports
        /// </summary>
        private readonly IssueManager issueManager;

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// List of media paths attached to the current message
        /// </summary>
        private readonly List<string> attachedMediaPaths;

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        ///  Constructor to initialize the ChatService
        /// </summary>
        /// <param name="chatHistoryPanel"></param>
        /// <param name="issueManager"></param>
        /// <param name="attachedMediaPaths"></param>
        public ChatService(StackPanel chatHistoryPanel, IssueManager issueManager, List<string> attachedMediaPaths)
        {
            this.chatHistoryPanel = chatHistoryPanel;
            this.issueManager = issueManager;
            this.attachedMediaPaths = attachedMediaPaths;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// This method creates and adds a chat bubble to the chat history panel
        /// </summary>
        /// <param name="message"></param>
        /// <param name="isMedia"></param>
        /// <param name="isUser"></param>
        /// <param name="isInput"></param>
        /// <returns></returns>
        public Border AddChatBubble(string message, bool isMedia, bool isUser, bool isInput)
        {
            var chatBubble = new Border
            {
                Background = (Brush)Application.Current.FindResource(isUser ? "greenSolidColorBrush" : "offWhiteSolidColorBrush"),
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(10),
                Margin = new Thickness(5),
                MaxWidth = 350,
                HorizontalAlignment = isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left
            };

            var bubbleContent = new StackPanel();
            if (isMedia && attachedMediaPaths?.Count > 0)
                attachedMediaPaths.ForEach(mediaPath => DisplayAttachedMedia(bubbleContent, mediaPath));

            if (isUser)
                AddUserMessage(message, bubbleContent);
            else if (isInput)
                AddInputFields(message, bubbleContent);
            else
                AddSystemMessage(message, bubbleContent);

            chatBubble.Child = bubbleContent;
            chatHistoryPanel.Children.Add(chatBubble);

            return chatBubble;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Adds user message in the chat bubble
        /// </summary>
        /// <param name="message"></param>
        /// <param name="bubbleContent"></param>
        private void AddUserMessage(string message, StackPanel bubbleContent)
        {
            var messageParts = message.Split('\n');
            var fields = new[] { "Location", "Category", "Description", "Time" };

            for (int i = 0; i < messageParts.Length && i < fields.Length; i++)
            {
                bubbleContent.Children.Add(new TextBlock
                {
                    Text = messageParts[i],
                    FontWeight = i < 2 ? FontWeights.Bold : FontWeights.Normal,
                    Foreground = (Brush)Application.Current.FindResource("darkestSolidColorBrush"),
                    Margin = new Thickness(0, 5, 0, 5),
                    TextWrapping = TextWrapping.Wrap
                });
            }
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Adds system message in the chat bubble
        /// </summary>
        /// <param name="message"></param>
        /// <param name="bubbleContent"></param>
        private void AddSystemMessage(string message, StackPanel bubbleContent)
        {
            bubbleContent.Children.Add(new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 5, 0, 5)
            });
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Adds input fields and search functionality
        /// </summary>
        /// <param name="message"></param>
        /// <param name="bubbleContent"></param>
        private void AddInputFields(string message, StackPanel bubbleContent)
        {
            bubbleContent.Children.Add(new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 5, 0, 5)
            });

            var tbxLocation = new TextBox { Margin = new Thickness(0, 5, 0, 5) };
            bubbleContent.Children.Add(tbxLocation);

            var cmbCategory = new ComboBox { Margin = new Thickness(0, 5, 0, 5) };
            Enum.GetValues(typeof(ReportCategory)).Cast<ReportCategory>().ToList().ForEach(c => cmbCategory.Items.Add(c));
            bubbleContent.Children.Add(cmbCategory);

            var btnSearch = new Button
            {
                Content = "Search",
                Margin = new Thickness(0, 5, 0, 5)
            };
            btnSearch.Click += (s, e) =>
            {
                var location = tbxLocation.Text;
                var category = cmbCategory.SelectedValue?.ToString();
                if (!string.IsNullOrEmpty(location) && !string.IsNullOrEmpty(category))
                    DisplaySearchedReport(location, category);
                else
                    MessageBox.Show("Please enter a location and select a category.");
            };
            bubbleContent.Children.Add(btnSearch);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Creates an image element
        /// </summary>
        /// <param name="mediaPath"></param>
        /// <returns></returns>
        private Image CreateImageElement(string mediaPath)
        {
            return new Image
            {
                Width = 150,
                Height = 150,
                Source = new BitmapImage(new Uri(mediaPath)),
                Stretch = Stretch.UniformToFill,
                Margin = new Thickness(0, 0, 0, 5)
            };
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Creates a video element
        /// </summary>
        /// <param name="mediaPath"></param>
        /// <returns></returns>
        private MediaElement CreateVideoElement(string mediaPath)
        {
            var mediaVideo = new MediaElement
            {
                Width = 200,
                Height = 150,
                Source = new Uri(mediaPath),
                LoadedBehavior = MediaState.Manual,
                UnloadedBehavior = MediaState.Close,
                Margin = new Thickness(0, 0, 0, 5)
            };
            mediaVideo.Play();
            return mediaVideo;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Creates a document link element
        /// </summary>
        /// <param name="mediaPath"></param>
        /// <returns></returns>
        private TextBlock CreateDocumentLink(string mediaPath)
        {
            var documentLink = new TextBlock
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

            return documentLink;
        }


        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Displays a searched report if found
        /// </summary>
        /// <param name="reportLocation"></param>
        /// <param name="reportCategory"></param>

        private void DisplaySearchedReport(string reportLocation, string reportCategory)
        {
            bool found = false;
            var issues = this.issueManager.GetIssues();

            foreach (var issue in issues)
            {
                if (issue.Category == reportCategory && issue.Location == reportLocation)
                {
                    string reportMessage = $"Location: {issue.Location}\nCategory: {issue.Category}\nDescription: {issue.Description}\nTime: {issue.Timestamp}";

                    // Add each report as a chat bubble with media if present
                    Border chatBubble = AddChatBubble(reportMessage, isUser: false, isMedia: issue.Attachments.Any(), isInput: false);

                    // If the issue has attachments, display them
                    if (issue.Attachments.Any())
                    {
                        StackPanel mediaContainer = new StackPanel
                        {
                            Orientation = Orientation.Vertical
                        };

                        foreach (var mediaPath in issue.Attachments)
                        {
                            DisplayAttachedMedia(mediaContainer, mediaPath);
                        }

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

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Displays attached media
        /// </summary>
        /// <param name="bubbleContent"></param>
        /// <param name="mediaPath"></param>
        private void DisplayAttachedMedia(StackPanel bubbleContent, string mediaPath)
        {
            string extension = System.IO.Path.GetExtension(mediaPath).ToLower();

            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
            {
                bubbleContent.Children.Add(CreateImageElement(mediaPath));
            }
            else if (extension == ".mp4")
            {
                bubbleContent.Children.Add(CreateVideoElement(mediaPath));
            }
            else if (extension == ".pdf" || extension == ".docx")
            {
                bubbleContent.Children.Add(CreateDocumentLink(mediaPath));
            }
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Displays all reports from the issue manager
        /// </summary>
        public void DisplayAllReports()
        {
            // Fetch all reports from IssueManager
            var issues = issueManager.GetIssues();

            foreach (var issue in issues)
            {
                string reportMessage = $"Location: {issue.Location}\nCategory: {issue.Category}\nDescription: {issue.Description}\nTime: {issue.Timestamp}";

                // Add each report as a chat bubble with media if present
                Border chatBubble = AddChatBubble(reportMessage, isMedia: issue.Attachments.Any(), isUser: false, isInput: false);

                // If the issue has attachments, display them
                if (issue.Attachments.Any())
                {
                    StackPanel mediaContainer = new StackPanel
                    {
                        Orientation = Orientation.Vertical
                    };

                    foreach (var mediaPath in issue.Attachments)
                    {
                        DisplayAttachedMedia(mediaContainer, mediaPath);
                    }

                    (chatBubble.Child as StackPanel)?.Children.Add(mediaContainer);
                }
            }
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
