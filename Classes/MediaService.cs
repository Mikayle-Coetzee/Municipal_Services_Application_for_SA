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
        /// <summary>
        /// Holds a list of media attached paths 
        /// </summary>
        private List<string> attachedMediaPaths;

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Index representing the current media being displayed
        /// </summary>
        private int currentIndex;

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Constructor to initialize MediaService with the attached media paths
        /// </summary>
        /// <param name="attachedMediaPaths"></param>
        public MediaService(List<string> attachedMediaPaths)
        {
            this.attachedMediaPaths = attachedMediaPaths;
            this.currentIndex = 0;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Opens a file dialog for the user to select media files
        /// </summary>
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

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Displays the attached media in the provided container with navigation options
        /// </summary>
        /// <param name="mediaContainer"></param>
        /// <param name="mediaDisplay"></param>
        /// <param name="btnNext"></param>
        /// <param name="btnPrevious"></param>
        /// <param name="lblReUploadMedia"></param>
        /// <param name="reUploadIcon"></param>
        /// <param name="btnReuploadFile"></param>
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

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Loads and displays the media at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="mediaDisplay"></param>
        public void LoadMedia(int index, ContentControl mediaDisplay)
        {
            if (attachedMediaPaths == null || !attachedMediaPaths.Any() || index < 0 || index >= attachedMediaPaths.Count)
                return;

            string mediaPath = attachedMediaPaths[index];
            string extension = Path.GetExtension(mediaPath).ToLower();

            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
            {
                mediaDisplay.Content = CreateImageElement(mediaPath,mediaDisplay);
            }
            else if (extension == ".mp4")
            {
                mediaDisplay.Content = CreateVideoElement(mediaPath, mediaDisplay);
            }
            else if (extension == ".pdf" || extension == ".docx")
            {
                mediaDisplay.Content = CreateDocumentLink(mediaPath);
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

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Navigates through media items based on direction 
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="mediaDisplay"></param>
        public void NavigateMedia(int direction, ContentControl mediaDisplay)
        {
            currentIndex += direction;
            currentIndex = Math.Max(0, Math.Min(currentIndex, attachedMediaPaths.Count - 1));
            LoadMedia(currentIndex, mediaDisplay);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Creates an image element
        /// </summary>
        /// <param name="mediaPath"></param>
        /// <returns></returns>
        private Image CreateImageElement(string mediaPath, ContentControl mediaDisplay)
        {
            BitmapImage bitmap = new BitmapImage(new Uri(mediaPath, UriKind.Absolute));
            Image img = new Image
            {
                Source = bitmap,
                Width = mediaDisplay.Width,
                Height = mediaDisplay.Height,
                Stretch = Stretch.Fill
            };

            return img;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Creates a video element
        /// </summary>
        /// <param name="mediaPath"></param>
        /// <returns></returns>
        private MediaElement CreateVideoElement(string mediaPath, ContentControl mediaDisplay)
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

            return documentLink;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MediaService(){ }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Loads the image into an image control
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="imageControl"></param>
        public void LoadImage(string imagePath, Image imageControl)
        {
            if (imagePath == null) return;


            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            imageControl.Source = bitmap;

        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Converts byteArray to BitmapImage
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        private BitmapImage ByteArrayToImage(byte[] byteArray)
        {
            using (var stream = new MemoryStream(byteArray))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                return image;
            }
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Gets The media type
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string GetMediaType(string filename)
        {
            string extension = System.IO.Path.GetExtension(filename).ToLower();
            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                return "Image";
            if (extension == ".mp4" || extension == ".avi" || extension == ".mov")
                return "Video";
            if (extension == ".doc" || extension == ".docx" || extension == ".pdf")
                return "Document";

            return "Unknown";
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
