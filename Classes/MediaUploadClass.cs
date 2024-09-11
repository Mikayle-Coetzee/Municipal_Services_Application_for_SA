using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.IO;
using Microsoft.Win32;

namespace PROG7312_ST10023767.Classes
{
    public class MediaUploadClass
    {
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
        }

        public static void HandleFileUpload(List<string> attachedMediaPaths)
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
    }
}
