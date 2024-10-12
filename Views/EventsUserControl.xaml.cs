using PROG7312_ST10023767.Classes;
using System;
using System.Collections.Generic;
using System.IO; // Add this to work with file paths
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
using Microsoft.Win32;
using System.Windows.Controls.Primitives;

namespace PROG7312_ST10023767.Views
{
    /// <summary>
    /// Interaction logic for EventsUserControl.xaml
    /// </summary>
    public partial class EventsUserControl : UserControl
    {
        // Dictionary to store events sorted by location
        private Dictionary<string, List<EventClass>> locationEvents = new Dictionary<string, List<EventClass>>();
        private List<string> mediaAttachments = new List<string>();

        public EventsUserControl()
        {
            InitializeComponent();
            LoadLocations();
            UpdateEventsList();
        }

        private void LoadLocations()
        {
            foreach (var location in locationEvents.Keys)
            {
                Button locationButton = new Button
                {
                    Content = location,
                    Width = 100,
                    Margin = new Thickness(5)
                };
                locationButton.Click += LocationButton_Click;
                LocationWrapPanel.Children.Add(locationButton);
            }
        }

        private void LocationButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string selectedLocation = button.Content.ToString();

            cmbFilter.SelectedIndex = 0;

            LocationTextBlock.Text = $"Events & Announcement in Selected {selectedLocation}";

            EventsList.Items.Clear();

             if (locationEvents.ContainsKey(selectedLocation))
            {
                foreach (var ev in locationEvents[selectedLocation])
                {                    
                    Image typeImage = new Image { Width = 50, Height = 50, Stretch = Stretch.Fill };
                    Image categoryImage = new Image { Width = 50, Height = 50, Stretch = Stretch.Fill };

                    LoadImage(GetImagePath(ev.Type), typeImage);
                    LoadImage(GetImagePathCategory(ev.Category), categoryImage);

                    EventsList.Items.Add(typeImage);
                    EventsList.Items.Add(categoryImage);

                    TextBlock eventBlock = new TextBlock
                    {
                        Text = $"Title: {ev.Title}\nDate: {ev.Date}\nTime: {ev.Time}\nLocation: {ev.Location}\nDescription: {ev.Description}",
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 0, 0, 10)
                    };

                     EventsList.Items.Add(eventBlock);

                     if (ev.MediaFiles != null && ev.MediaFiles.Count > 0)
                    {
                         TextBlock mediaBlock = new TextBlock
                        {
                            Text = "Media Files:",
                            FontWeight = FontWeights.Bold,
                            Margin = new Thickness(0, 10, 0, 5)
                        };


                         EventsList.Items.Add(mediaBlock);

                         foreach (var media in ev.MediaFiles)
                        {
                             string fileName = System.IO.Path.GetFileName(media);
                            TextBlock mediaItem = new TextBlock
                            {
                                Text = fileName,
                                Margin = new Thickness(20, 0, 0, 5), 
                                Tag = media 
                            };

                            mediaItem.MouseDown += MediaItem_MouseDown;

                            EventsList.Items.Add(mediaItem);
                        }
                    }
                }
            }
        }


        private void LoadImage(string imagePath, Image imageControl)
        {
            if (imagePath == null) return; 


            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute); 
            bitmap.CacheOption = BitmapCacheOption.OnLoad; 
            bitmap.EndInit();

            imageControl.Source = bitmap;
            
        }



        private string GetImagePath(string type)
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

        private string GetImagePathCategory(string category)
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
        private void ViewAllEvents_Click(object sender, RoutedEventArgs e)
        {
            UpdateEventsList();
        }

      
        private void MediaItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock mediaItem = sender as TextBlock;
            string mediaPath = mediaItem.Tag.ToString(); 

            if (File.Exists(mediaPath))
            {
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(mediaPath) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("File does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddMedia_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true, 
                Filter = "Media Files|*.jpg;*.jpeg;*.png;*.gif;*.mp4;*.avi;*.mov;*.doc;*.docx;*.pdf",
                Title = "Select Media Files"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    mediaAttachments.Add(filename);
                    MediaList.Items.Add(System.IO.Path.GetFileName(filename)); 
                }
            }
        }

        private void RemoveMedia_Click(object sender, RoutedEventArgs e)
        {
            if (MediaList.SelectedItem != null)
            {
                string selectedMedia = MediaList.SelectedItem.ToString();
                mediaAttachments.RemoveAll(m => System.IO.Path.GetFileName(m) == selectedMedia);  
                MediaList.Items.Remove(selectedMedia);
            }
        }

        private void SubmitEventButton_Click(object sender, RoutedEventArgs e)
        {
            // Gather event data
            string title = EventTitle.Text;
            string date = EventDate.SelectedDate?.ToString("d MMM yyyy") ?? "";
            string time = EventTime.Text;
            string location = EventLocation.Text;
            string description = EventDescription.Text;
            string type = (cmbType.SelectedItem as ComboBoxItem)?.Content.ToString();
            string category = (cmbCategory.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(date) || string.IsNullOrWhiteSpace(time) || string.IsNullOrWhiteSpace(location))
            {
                MessageBox.Show("Please fill in all the fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!locationEvents.ContainsKey(location))
            {
                locationEvents[location] = new List<EventClass>();
                Button locationButton = new Button
                {
                    Content = location,
                    Width = 100,
                    Margin = new Thickness(5)
                };
                locationButton.Click += LocationButton_Click;
                LocationWrapPanel.Children.Add(locationButton);
            }

            EventClass newEvent = new EventClass(
                title, 
                date, 
                time, 
                location, 
                description, 
                new List<string>(mediaAttachments),
                type,
                category
                );
            locationEvents[location].Add(newEvent);

            // Reset form
            EventTitle.Text = "";
            EventDate.SelectedDate = null;
            EventTime.Text = "";
            EventLocation.Text = "";
            EventDescription.Text = "";
            MediaList.Items.Clear();
            mediaAttachments.Clear();
            cmbCategory.SelectedIndex = 0;
            cmbType.SelectedIndex = 0;


            MessageBox.Show("Event submitted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFilter = (cmbFilter.SelectedItem as ComboBoxItem)?.Content.ToString();
            string selectedLocation = LocationTextBlock.Text.Contains("Selected") ? LocationTextBlock.Text.Replace("Events & Announcement in Selected ", "") : null;

            List<EventClass> eventsToDisplay = selectedLocation != null && locationEvents.ContainsKey(selectedLocation)
                ? locationEvents[selectedLocation]
                : locationEvents.Values.SelectMany(evList => evList).ToList(); // Get all events if no specific location is selected.

            if (selectedFilter == "All")
            {
                eventsToDisplay = eventsToDisplay.ToList();
            }
            else
            if(selectedFilter == "Events")
            {
                eventsToDisplay = eventsToDisplay.Where(item => item.Type == "Event").ToList();
            }
            else if (selectedFilter == "Announcements")
            {
                eventsToDisplay = eventsToDisplay.Where(item => item.Type == "Announcement").ToList();
            }
            else if (selectedFilter == "Category")
            {
                CategoryFilterWindow categoryWindow = new CategoryFilterWindow();

                if (categoryWindow.ShowDialog() == true)
                {
                    string selectedCategory = categoryWindow.SelectedCategory;

                    eventsToDisplay = eventsToDisplay.Where(item => item.Category == selectedCategory).ToList();
                }
            }

            UpdateEventsList(eventsToDisplay);
        }


        private void UpdateEventsList(List<EventClass> filteredEvents = null)
        {
            if (EventsList != null)
            {
                EventsList.Items.Clear();
                LocationTextBlock.Text = filteredEvents == null ? "All Events & Announcements" : LocationTextBlock.Text;

                var eventsToDisplay = filteredEvents ?? locationEvents.Values.SelectMany(evList => evList).ToList();

                foreach (var ev in eventsToDisplay)
                {
                    Image typeImage = new Image { Width = 50, Height = 50, Stretch = Stretch.Fill };
                    Image categoryImage = new Image { Width = 50, Height = 50, Stretch = Stretch.Fill };

                    LoadImage(GetImagePath(ev.Type), typeImage);
                    LoadImage(GetImagePathCategory(ev.Category), categoryImage);

                    EventsList.Items.Add(typeImage);
                    EventsList.Items.Add(categoryImage);

                    TextBlock eventBlock = new TextBlock
                    {
                        Text = $"Title: {ev.Title}\nDate: {ev.Date}\nTime: {ev.Time}\nLocation: {ev.Location}\nDescription: {ev.Description}",
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 0, 0, 10)
                    };

                    EventsList.Items.Add(eventBlock);

                    if (ev.MediaFiles != null && ev.MediaFiles.Count > 0)
                    {
                        TextBlock mediaBlock = new TextBlock
                        {
                            Text = "Media Files:",
                            FontWeight = FontWeights.Bold,
                            Margin = new Thickness(0, 10, 0, 5)
                        };

                        EventsList.Items.Add(mediaBlock);

                        foreach (var media in ev.MediaFiles)
                        {
                            TextBlock mediaItem = new TextBlock
                            {
                                Text = System.IO.Path.GetFileName(media),
                                Margin = new Thickness(20, 0, 0, 5),
                                Tag = media
                            };

                            mediaItem.MouseDown += MediaItem_MouseDown;
                            EventsList.Items.Add(mediaItem);
                        }
                    }
                }
            }
        }

    }
}
