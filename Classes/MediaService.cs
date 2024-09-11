using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PROG7312_ST10023767.Classes
{
    public class MediaService
    {
        private List<string> attachedMediaPaths;
        private int currentIndex;

        public MediaService(List<string> attachedMediaPaths)
        {
            this.attachedMediaPaths = attachedMediaPaths;
            this.currentIndex = 0;
        }

        public void AttachMedia()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png|All Files|*.*",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filePath in openFileDialog.FileNames)
                {
                    attachedMediaPaths.Add(filePath);
                }
            }
        }

        public void DisplayUploadedMedia(StackPanel mediaContainer, ContentControl mediaDisplay, Button btnNext, Button btnPrevious, Label lblReUploadMedia, Image reUploadIcon, Button btnReuploadFile)
        {
            if (attachedMediaPaths == null || !attachedMediaPaths.Any())
            {
                lblReUploadMedia.Visibility = Visibility.Collapsed;
                return;
            }

            mediaContainer.Visibility = Visibility.Visible;
            mediaContainer.Children.Clear();

            LoadMedia(currentIndex, mediaDisplay);
            mediaContainer.Children.Add(mediaDisplay);

            btnNext.Visibility = Visibility.Visible;
            btnPrevious.Visibility = Visibility.Visible;

            btnPrevious.Click += (s, e) => NavigateMedia(-1, mediaDisplay);
            btnNext.Click += (s, e) => NavigateMedia(1, mediaDisplay);

            lblReUploadMedia.Visibility = Visibility.Visible;
            reUploadIcon.Visibility = Visibility.Visible;
            btnReuploadFile.Visibility = Visibility.Visible;
        }

        public void LoadMedia(int index, ContentControl mediaDisplay)
        {
            if (attachedMediaPaths == null || !attachedMediaPaths.Any() || index < 0 || index >= attachedMediaPaths.Count)
                return;

            string mediaPath = attachedMediaPaths[index];
            string extension = Path.GetExtension(mediaPath).ToLower();

            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
            {
                BitmapImage bitmap = new BitmapImage(new Uri(mediaPath, UriKind.Absolute));
                Image img = new Image
                {
                    Source = bitmap,
                    Width = mediaDisplay.Width,
                    Height = mediaDisplay.Height,
                    Stretch = Stretch.Fill
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
                    Text = "Open Document: " + Path.GetFileName(mediaPath),
                    FontStyle = FontStyles.Italic,
                    Foreground = Brushes.Blue,
                    Cursor = Cursors.Hand,
                    TextDecorations = TextDecorations.Underline,
                    TextWrapping = TextWrapping.Wrap
                };

                documentLink.MouseLeftButtonUp += (s, e) =>
                {
                    Process.Start(new ProcessStartInfo
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
                    Text = "Unsupported media type: " + Path.GetFileName(mediaPath),
                    Foreground = Brushes.Red
                };

                mediaDisplay.Content = unsupportedMedia;
            }
        }

        public void NavigateMedia(int direction, ContentControl mediaDisplay)
        {
            currentIndex += direction;
            currentIndex = Math.Max(0, Math.Min(currentIndex, attachedMediaPaths.Count - 1));
            LoadMedia(currentIndex, mediaDisplay);
        }


        public static void DisplayMedia(StackPanel bubbleContent, string mediaPath)
        {
            string extension = Path.GetExtension(mediaPath).ToLower();
            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
            {
                Image mediaImage = new Image
                {
                    Width = 150,
                    Height = 150,
                    Source = new BitmapImage(new Uri(mediaPath)),
                    Stretch = Stretch.Fill,
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
                    Margin = new Thickness(0, 0, 0, 5),
                    Stretch = Stretch.Fill,
                };
                mediaVideo.Play();
                bubbleContent.Children.Add(mediaVideo);
            }
        }
    }
}
