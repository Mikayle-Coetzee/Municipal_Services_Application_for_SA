using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.IO;

namespace PROG7312_ST10023767.Classes
{
    public class DisplayHelper
    {
        /// <summary>
        /// New instance of the media helper class
        /// </summary>
        private MediaService mediaHelper = new MediaService();

        #region Message Display Methods

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Creates an event border with a panel for displaying messages.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Border CreateEventBorderWithPanel(string title, string message)
        {
            Border eventBorder = new Border
            {
                CornerRadius = new CornerRadius(15),
                Margin = new Thickness(5),
                Background = (Brush)Application.Current.FindResource("blueSolidColorBrush")
            };

            StackPanel eventPanel = CreateEventPanel(title, message);

            // Add the StackPanel to the Border
            eventBorder.Child = eventPanel;

            return eventBorder;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Method to create the event panel
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public StackPanel CreateEventPanel(string title, string message)
        {
            StackPanel eventPanel = new StackPanel();

            Grid horizontalGrid = CreateHorizontalGridForMessageBox(title);
            eventPanel.Children.Add(horizontalGrid);

            TextBlock eventMessage = CreateMessageBlock(message);
            eventPanel.Children.Add(eventMessage);

            return eventPanel;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Method to create the horizontal grid (title with icons)
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Grid CreateHorizontalGridForMessageBox(string title)
        {
            Grid horizontalGrid = new Grid();
            horizontalGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            horizontalGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            horizontalGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

            Image eventTypeImage = new Image { Width = 40, Height = 40, Stretch = Stretch.Fill, Margin = new Thickness(5) };
            mediaHelper.LoadImage(GetImagePathCategory("Other"), eventTypeImage);
            Grid.SetColumn(eventTypeImage, 0);
            horizontalGrid.Children.Add(eventTypeImage);

            TextBlock eventInfo = new TextBlock
            {
                Text = title,
                Margin = new Thickness(0, 15, 0, 5),
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                MinWidth = 800,
                Width = 800
            };
            Grid.SetColumn(eventInfo, 1);
            horizontalGrid.Children.Add(eventInfo);

            Image categoryImage = new Image { Width = 40, Height = 40, Stretch = Stretch.Fill, Margin = new Thickness(5) };
            mediaHelper.LoadImage(GetImagePathCategory("Other"), categoryImage);
            Grid.SetColumn(categoryImage, 2);
            horizontalGrid.Children.Add(categoryImage);

            return horizontalGrid;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Method to create the message block
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public TextBlock CreateMessageBlock(string message)
        {
            return new TextBlock
            {
                Text = message,
                Margin = new Thickness(5, 5, 0, 5),
                FontWeight = FontWeights.Bold,
                TextWrapping = TextWrapping.Wrap
            };
        }

        #endregion

        #region Image Handling Methods
        /// <summary>
        /// Gets the file path of an image based on its type.
        /// </summary>
        public string GetImagePath(string type)
        {
            string relativeImagePath;

            if (type == "Announcement")
            {
                relativeImagePath = "Images/Part2/announcement.png";
            }
            else
            {
                relativeImagePath = "Images/Part2/event.png";
            }

            string fullImagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeImagePath);

            if (File.Exists(fullImagePath))
            {
                return fullImagePath;
            }
            else
            {
                MessageBox.Show($"Image not found: {fullImagePath}");
                return null;
            }
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Gets the file path of an image based on its category.
        /// </summary>
        public string GetImagePathCategory(string category)
        {
            string relativeImagePath;

            switch (category)
            {
                case "Music":
                    relativeImagePath = "Images/Part2/music.png";
                    break;
                case "Sports":
                    relativeImagePath = "Images/Part2/sports.png";
                    break;
                case "Art":
                    relativeImagePath = "Images/Part2/art.png";
                    break;
                case "Theater":
                    relativeImagePath = "Images/Part2/theater.png";
                    break;
                case "Networking":
                    relativeImagePath = "Images/Part2/networking.png";
                    break;
                default:
                    relativeImagePath = "Images/Part2/other.png";
                    break;
            }

            string fullImagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeImagePath);

            if (File.Exists(fullImagePath))
            {
                return fullImagePath;
            }
            else
            {
                MessageBox.Show($"Image not found: {fullImagePath}");
                return null;
            }
        }
        #endregion

        #region Message Handling Methods

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Determines the status of the event based on current date and time
        /// </summary>
        /// <param name="ev"></param>
        /// <param name="currentDateTime"></param>
        /// <param name="statusBorder"></param>
        /// <returns></returns>
        public string DetermineEventStatus(EventClass ev, DateTime currentDateTime, out Border statusBorder)
        {
            DateTime startDateTime, endDateTime;
            string status;

            statusBorder = new Border
            {
                CornerRadius = new CornerRadius(15),
                Height = 5,
                Margin = new Thickness(7, 0, 7, 0)
            };

            if (DateTime.TryParse(ev.StartDate, out startDateTime) && DateTime.TryParse(ev.EndDate, out endDateTime))
            {
                endDateTime = endDateTime.Date.AddDays(1).AddTicks(-1);

                if (currentDateTime >= startDateTime && currentDateTime <= endDateTime)
                {
                    status = "busy";
                    statusBorder.Background = (Brush)Application.Current.FindResource("busySolidColorBrush");
                }
                else if (currentDateTime > endDateTime)
                {
                    status = "past";
                    statusBorder.Background = (Brush)Application.Current.FindResource("pastSolidColorBrush");
                }
                else
                {
                    status = "upcoming";
                    statusBorder.Background = (Brush)Application.Current.FindResource("upcomingSolidColorBrush");
                }
            }
            else
            {
                status = "unknown";
                statusBorder.Background = (Brush)Application.Current.FindResource("defaultSolidColorBrush");
                Console.WriteLine($"Failed to parse dates for event: {ev.Title}, StartDate: {ev.StartDate}, EndDate: {ev.EndDate}");
            }

            return status;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Creates a Border for the event
        /// </summary>
        /// <param name="ev"></param>
        /// <returns></returns>
        public Border CreateEventBorder(EventClass ev)
        {
            return new Border
            {
                CornerRadius = new CornerRadius(15),
                Margin = new Thickness(5),
                Background = (Brush)Application.Current.FindResource(ev.Type == "Event" ? "greenSolidColorBrush" : "offWhiteSolidColorBrush")
            };
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Creates a StackPanel for the event with the status border and event details
        /// </summary>
        /// <param name="statusBorder"></param>
        /// <param name="ev"></param>
        /// <returns></returns>
        public StackPanel CreateEventPanel(Border statusBorder, EventClass ev)
        {
            StackPanel eventPanel = new StackPanel();
            eventPanel.Children.Add(statusBorder);

            Grid horizontalGrid = CreateHorizontalGrid(ev);
            eventPanel.Children.Add(horizontalGrid);

            return eventPanel;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Creates a horizontal grid for the event display
        /// </summary>
        /// <param name="ev"></param>
        /// <returns></returns>
        public Grid CreateHorizontalGrid(EventClass ev)
        {
            Grid horizontalGrid = new Grid();
            horizontalGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            horizontalGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            horizontalGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

            Image eventTypeImage = new Image { Width = 40, Height = 40, Stretch = Stretch.Fill, Margin = new Thickness(5) };
            mediaHelper.LoadImage(GetImagePath(ev.Type), eventTypeImage);
            Grid.SetColumn(eventTypeImage, 0);
            horizontalGrid.Children.Add(eventTypeImage);

            TextBlock eventInfo = new TextBlock
            {
                Text = $"{ev.StartTime}: {ev.StartDate} - {ev.EndTime}: {ev.EndDate}",
                Margin = new Thickness(0, 15, 0, 5),
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                MinWidth = 800,
                Width = 800
            };
            Grid.SetColumn(eventInfo, 1);
            horizontalGrid.Children.Add(eventInfo);

            Image categoryImage = new Image { Width = 40, Height = 40, Stretch = Stretch.Fill, Margin = new Thickness(5) };
            mediaHelper.LoadImage(GetImagePathCategory(ev.Category), categoryImage);
            Grid.SetColumn(categoryImage, 2);
            horizontalGrid.Children.Add(categoryImage);

            return horizontalGrid;
        }

        #endregion
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
