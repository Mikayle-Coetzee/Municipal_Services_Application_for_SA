using PROG7312_ST10023767.Classes;
using System;
using System.Collections.Generic;
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
using Microsoft.Win32;
using System.Windows.Controls.Primitives;
using System.Collections;

namespace PROG7312_ST10023767.Views
{
    /// <summary>
    /// Interaction logic for EventsUserControl.xaml
    /// </summary>
    public partial class EventsUserControl : UserControl
    {
        // Dictionary to store events sorted by location
        private SortedDictionary<string, List<EventClass>> locationEvents = new SortedDictionary<string, List<EventClass>>();
        private Stack<MediaFileClass> mediaAttachments = new Stack<MediaFileClass>();
        private Dictionary<string, List<string>> userSearchHistory = new Dictionary<string, List<string>>();

        private HashSet<string> uniqueCategories = new HashSet<string>();

        public EventsUserControl()
        {
            InitializeComponent();
            LoadLocations();
            UpdateEventsList();

            SetDateToNow(EventDate); 
            SetDateToNow(EndDate);
        }

        private void LoadLocations()
        {
            foreach (var location in locationEvents.Keys)
            {
                Button locationButton = new Button
                {
                    Content = location,
                    Margin = new Thickness(5),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Top
                };
                locationButton.Click += LocationButton_Click;
                LocationWrapPanel.Children.Add(locationButton);
            }
        }


            public void SetDateToNow(DatePicker datePicker)
            {
                if (datePicker != null)
                {
                    datePicker.SelectedDate = DateTime.Now; 
                }
            }


        private void LocationButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string selectedLocation = button.Content.ToString();

            cmbFilter.SelectedIndex = 0;

            LocationTextBlock.Text = $"Events & Announcement in {selectedLocation}";

            EventsList.Items.Clear();

            if (locationEvents.ContainsKey(selectedLocation))
            {
                if (locationEvents[selectedLocation].Count == 0)
                {
                    NoPostsMessageBox("No Posts Posted Yet", "Be a champ in your community by clicking on the 'Create Post' and posting first!");
                    return;
                }
                UpdateEventsList(locationEvents[selectedLocation]);
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
            byte[] mediaContent = mediaItem.Tag as byte[];

            if (mediaContent != null)
            {
                string tempFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), mediaItem.Text);

                try
                {
                    File.WriteAllBytes(tempFilePath, mediaContent);

                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(tempFilePath) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening media: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No media content available.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    byte[] fileContent = File.ReadAllBytes(filename);

                     string mediaType = GetMediaType(filename);

                     MediaFileClass mediaFile = new MediaFileClass(System.IO.Path.GetFileName(filename), fileContent, mediaType);
                    mediaAttachments.Push(mediaFile);
                    MediaList.Items.Add(System.IO.Path.GetFileName(filename));
                }
            }
        }

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


         private string GetMediaType(string filename)
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



        private void RemoveMedia_Click(object sender, RoutedEventArgs e)
        {
            if (MediaList.SelectedItem != null)
            {
                string selectedMedia = MediaList.SelectedItem.ToString();

                // Use a temporary stack to hold items while popping
                Stack<MediaFileClass> tempStack = new Stack<MediaFileClass>();

                bool itemFound = false;

                while (mediaAttachments.Count > 0)
                {
                    var media = mediaAttachments.Pop();
                    if (media.FileName != selectedMedia)
                    {
                        tempStack.Push(media); // If not the selected item, push it to the temp stack
                    }
                    else
                    {
                        itemFound = true; // The item was found and removed
                        MediaList.Items.Remove(selectedMedia);
                        break;
                    }
                }

                // Restore the items back to the original stack
                while (tempStack.Count > 0)
                {
                    mediaAttachments.Push(tempStack.Pop());
                }

                if (!itemFound)
                {
                    MessageBox.Show("Media item not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SubmitEventButton_Click(object sender, RoutedEventArgs e)
        {
             string title = EventTitle.Text;
            string date = EventDate.SelectedDate?.ToString("d MMM yyyy") ?? "";
            string location = EventLocation.Text;
            string description = EventDescription.Text;
            string type = (cmbType.SelectedItem as ComboBoxItem)?.Content.ToString();
            string category = (cmbCategory.SelectedItem as ComboBoxItem)?.Content.ToString();
            string endDate = EndDate.SelectedDate?.ToString("d MMM yyyy") ?? "";
            string venue = EventVenue.Text;


            string selectedHour = (cmbHour.SelectedItem as ComboBoxItem)?.Content.ToString();
            string selectedMinute = (cmbMinute.SelectedItem as ComboBoxItem)?.Content.ToString();
            string selectedAmPm = (cmbAmPm.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(selectedHour) || string.IsNullOrEmpty(selectedMinute) || string.IsNullOrEmpty(selectedAmPm))
            {
                MessageBox.Show("Please select a valid time.");
                return;
            }

            string time = $"{selectedHour}:{selectedMinute} {selectedAmPm}";

            if (!string.IsNullOrEmpty(category))
            {
                uniqueCategories.Add(category);
            }

            string selectedHourEnd = (cmbHourEnd.SelectedItem as ComboBoxItem)?.Content.ToString();
            string selectedMinuteEnd = (cmbMinuteEnd.SelectedItem as ComboBoxItem)?.Content.ToString();
            string selectedAmPmEnd = (cmbAmPmEnd.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(selectedHourEnd) || string.IsNullOrEmpty(selectedMinuteEnd) || string.IsNullOrEmpty(selectedAmPmEnd))
            {
                MessageBox.Show("Please select a valid time.");
                return;
            }

            string timeEnd = $"{selectedHourEnd}:{selectedMinuteEnd} {selectedAmPmEnd}";

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(date) || string.IsNullOrWhiteSpace(time) || string.IsNullOrWhiteSpace(location))
            {
                MessageBox.Show("Please fill in all the fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!locationEvents.ContainsKey(location))
            {
                venueButtonsPanel.Visibility = Visibility.Visible;
                LocationWrapPanel.Visibility = Visibility.Visible;

                locationEvents[location] = new List<EventClass>();
                Button locationButton = new Button
                {
                    Content = location,
                    Margin = new Thickness(5),
                    Style = (Style)FindResource("RoundedButtonStyle"),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalContentAlignment = HorizontalAlignment.Stretch // Aligns the content to the left
                };

                // Attach the Click event handler
                locationButton.Click += LocationButton_Click;


                // Set the width of the button dynamically based on the available space in the WrapPanel
                if(LocationWrapPanel.ActualWidth != 0)
                {
                locationButton.Width = LocationWrapPanel.ActualWidth - 10; // Adjust the margin or padding as needed
                }

                LocationWrapPanel.Children.Add(locationButton);
            }

            EventClass newEvent = new EventClass(
                title,
                date,
                time,
                location,
                description,
                mediaAttachments.ToArray().ToList(),
                type,
                category,
                endDate,
                venue,
                timeEnd

                );
            locationEvents[location].Add(newEvent);

            ClearAllFields();

            MessageBox.Show("Event submitted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ClearAllFields()
        {
            EventTitle.Text = "";
            SetDateToNow(EventDate);
            SetDateToNow(EndDate);
            cmbHour.SelectedIndex = 0;
            cmbMinute.SelectedIndex = 0;
            cmbAmPm.SelectedIndex = 0;
            cmbHourEnd.SelectedIndex = 0;
            cmbMinuteEnd.SelectedIndex = 0;
            cmbAmPmEnd.SelectedIndex = 0;
            EventVenue.Text = "";
            EventLocation.Text = "";
            EventDescription.Text = "";
            MediaList.Items.Clear();
            mediaAttachments.Clear();
            cmbCategory.SelectedIndex = 0;
            cmbType.SelectedIndex = 0;
        }


        private void LocationWrapPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Loop through each button in the WrapPanel and resize them
            foreach (UIElement child in LocationWrapPanel.Children)
            {
                if (child is Button button)
                {
                    button.Width = LocationWrapPanel.ActualWidth - 10; // Adjust based on margin or padding
                }
            }
        }

        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFilter = (cmbFilter.SelectedItem as ComboBoxItem)?.Content.ToString();
            string selectedLocation = LocationTextBlock.Text.Contains("Selected") ? LocationTextBlock.Text.Replace("Events & Announcement in Selected ", "") : null;

            List<EventClass> eventsToDisplay = selectedLocation != null && locationEvents.ContainsKey(selectedLocation)
                ? locationEvents[selectedLocation]
                : locationEvents.Values.SelectMany(evList => evList).ToList(); // Get all events if no specific location is selected.


            DateTime currentDateTime = DateTime.Now;


            if (selectedFilter == "All")
            {
                eventsToDisplay = eventsToDisplay.ToList();
            }
            else
            if (selectedFilter == "Events")
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
                categoryWindow.PopulateCategories(uniqueCategories);

                if (categoryWindow.ShowDialog() == true)
                {
                    string selectedCategory = categoryWindow.SelectedCategory;

                    eventsToDisplay = eventsToDisplay.Where(item => item.Category == selectedCategory).ToList();
                }
            }
            else if (selectedFilter == "Upcoming")
            {
                eventsToDisplay = eventsToDisplay.Where(item => currentDateTime < DateTime.Parse(item.StartDate)).ToList();
            }
            else if (selectedFilter == "Past")
            {
                eventsToDisplay = eventsToDisplay.Where(item => currentDateTime > DateTime.Parse(item.EndDate) && currentDateTime > DateTime.Parse(item.StartDate) && currentDateTime >= DateTime.Parse(item.EndDate).Date.AddDays(1).AddTicks(-1)).ToList() ;
            }
            else if (selectedFilter == "Busy")
            {
                eventsToDisplay = eventsToDisplay.Where(item => currentDateTime >= DateTime.Parse(item.StartDate) && currentDateTime <= DateTime.Parse(item.EndDate).Date.AddDays(1).AddTicks(-1)).ToList();
            }

            if (eventsToDisplay.Count == 0)
            {
                NoPostsMessageBox("No Posts Matching That Filter Request", "Unfortunatley there is no posts that match your filtering requests, feel free to add more posts or apply a different filter.");
                return;
            }

            UpdateEventsList(eventsToDisplay);
        }


        private void UpdateEventsList(List<EventClass> filteredEvents = null)
        {
            if (EventsList != null)
            {
                EventsList.Items.Clear();
                LocationTextBlock.Text = filteredEvents == null ? "All Events & Announcements" : LocationTextBlock.Text;

                var eventsToDisplay = filteredEvents ?? locationEvents.Values.SelectMany(evList => evList).ToList().OrderBy(e => e.StartDate).ToList();

                if (eventsToDisplay.Count == 0)
                {
                    NoPostsMessageBox("No Posts Posted Yet", "Be a champ in your community by posting first!");
                    return;
                }

                foreach (var ev in eventsToDisplay)
                {
                    bool isEvent = ev.Type == "Event";
                    string status;
                    DateTime currentDateTime = DateTime.Now;

                    DateTime startDateTime;
                    DateTime endDateTime;

                    Border statusBorder = new Border
                    {
                        CornerRadius = new CornerRadius(15),
                        Height = 5,
                        Margin = new Thickness(7, 0, 7, 0)
                    };

                    if (DateTime.TryParse(ev.StartDate, out startDateTime) &&
                        DateTime.TryParse(ev.EndDate, out endDateTime))
                    {
                        endDateTime = endDateTime.Date.AddDays(1).AddTicks(-1);

                        if (currentDateTime >= startDateTime && currentDateTime <= endDateTime)
                        {
                            // Status is busy
                            status = "busy";
                            statusBorder.Background = (Brush)Application.Current.FindResource("busySolidColorBrush");
                        }
                        else if (currentDateTime > endDateTime) // Past end date
                        {
                            // Status is past
                            status = "past";
                            statusBorder.Background = (Brush)Application.Current.FindResource("pastSolidColorBrush");
                        }
                        else
                        {
                            // Status is upcoming
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

                    Console.WriteLine($"Status for event '{ev.Title}': {status}");



                    // Create a Border for rounded corners
                    Border eventBorder = new Border
                    {
                        CornerRadius = new CornerRadius(15), // Set corner radius
                        Margin = new Thickness(5),
                        Background = (Brush)Application.Current.FindResource(isEvent ? "greenSolidColorBrush" : "offWhiteSolidColorBrush")
                    };

                    // Create a StackPanel inside the Border
                    StackPanel eventPanel = new StackPanel();

                    eventPanel.Children.Add(statusBorder);

                    Grid horizontalGrid = new Grid();
                    horizontalGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                    horizontalGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    horizontalGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

                    Image eventTypeImage = new Image { Width = 40, Height = 40, Stretch = Stretch.Fill, Margin = new Thickness(5) };
                    LoadImage(GetImagePath(ev.Type), eventTypeImage);
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
                    LoadImage(GetImagePathCategory(ev.Category), categoryImage);
                    Grid.SetColumn(categoryImage, 2);
                    horizontalGrid.Children.Add(categoryImage);

                    eventPanel.Children.Add(horizontalGrid);

                    TextBlock eventTitle = new TextBlock
                    {
                        Text = "Title: " + ev.Title,
                        Margin = new Thickness(5, 5, 0, 5),
                        FontWeight = FontWeights.Bold,
                        TextWrapping = TextWrapping.Wrap
                    };
                    eventPanel.Children.Add(eventTitle);

                    TextBlock eventDescription = new TextBlock
                    {
                        Text = "Description: " + ev.Description,
                        Margin = new Thickness(5, 5, 0, 5),
                        TextWrapping = TextWrapping.Wrap
                    };
                    eventPanel.Children.Add(eventDescription);

                    TextBlock eventVenue = new TextBlock
                    {
                        Text = $"Venue: {ev.Venue}",
                        Margin = new Thickness(5, 5, 0, 5),
                        FontWeight = FontWeights.Bold,
                        TextWrapping = TextWrapping.Wrap
                    };
                    eventPanel.Children.Add(eventVenue);

                    TextBlock eventArea = new TextBlock
                    {
                        Text = $"City/Area: {ev.Location}",
                        Margin = new Thickness(5, 5, 0, 5),
                        FontWeight = FontWeights.Bold,
                        TextWrapping = TextWrapping.Wrap
                    };
                    eventPanel.Children.Add(eventArea);

                    if (ev.MediaFiles != null && ev.MediaFiles.Count > 0)
                    {
                        TextBlock mediaBlock = new TextBlock
                        {
                            Text = "Media Files:",
                            FontWeight = FontWeights.Bold,
                            Margin = new Thickness(0, 10, 0, 5),
                            TextWrapping = TextWrapping.Wrap
                        };

                        eventPanel.Children.Add(mediaBlock);

                        foreach (var media in ev.MediaFiles)
                        {
                            TextBlock mediaItem = new TextBlock
                            {
                                Text = media.FileName,
                                Margin = new Thickness(20, 0, 0, 5),
                                Tag = media.FileContent,
                                TextWrapping = TextWrapping.Wrap
                            };

                            mediaItem.MouseDown += MediaItem_MouseDown;
                            eventPanel.Children.Add(mediaItem);
                        }
                    }

                    // Add the StackPanel to the Border
                    eventBorder.Child = eventPanel;

                    // Finally, add the Border to the EventsList
                    EventsList.Items.Add(eventBorder);
                }
            }
            
        }


        private void NoPostsMessageBox(string Title, string Message)
        {
            if (EventsList != null)
            {
                EventsList.Items.Clear();
                Border eventBorder = new Border
                {
                    CornerRadius = new CornerRadius(15), // Set corner radius
                    Margin = new Thickness(5),
                    Background = (Brush)Application.Current.FindResource("blueSolidColorBrush")
                };

                // Create a StackPanel inside the Border
                StackPanel eventPanel = new StackPanel();

                Grid horizontalGrid = new Grid();
                horizontalGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                horizontalGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                horizontalGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

                Image eventTypeImage = new Image { Width = 40, Height = 40, Stretch = Stretch.Fill, Margin = new Thickness(5) };
                LoadImage(GetImagePathCategory("Other"), eventTypeImage);
                Grid.SetColumn(eventTypeImage, 0);
                horizontalGrid.Children.Add(eventTypeImage);

                TextBlock eventInfo = new TextBlock
                {
                    Text = Title,
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
                LoadImage(GetImagePathCategory("Other"), categoryImage);
                Grid.SetColumn(categoryImage, 2);
                horizontalGrid.Children.Add(categoryImage);

                eventPanel.Children.Add(horizontalGrid);

                TextBlock eventMessage = new TextBlock
                {
                    Text = Message,
                    Margin = new Thickness(5, 5, 0, 5),
                    FontWeight = FontWeights.Bold,
                    TextWrapping = TextWrapping.Wrap
                };
                eventPanel.Children.Add(eventMessage);


                // Add the StackPanel to the Border
                eventBorder.Child = eventPanel;

                // Finally, add the Border to the EventsList
                EventsList.Items.Add(eventBorder);
            }

        }
        private void viewByArea_Click(object sender, RoutedEventArgs e)
        {
            if (venueButtonsPanel.Visibility == Visibility.Visible)
            {
                venueButtonsPanel.Visibility = Visibility.Collapsed;
            }else
            {
                venueButtonsPanel.Visibility = Visibility.Visible;
            }
        }

        private void btnAddEvent_Click(object sender, RoutedEventArgs e)
        {
            if (createPostPanel.Visibility == Visibility.Visible)
            {
                createPostPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                createPostPanel.Visibility = Visibility.Visible;
            }
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        ///  Handles the back button click event and hides the current UserControl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchQuery = txbSearch.Text;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                // Track the user's search
                TrackUserSearch("currentUserId", searchQuery);  // Replace with actual user ID logic
            }


            string query = searchQuery.ToLower().Trim();

            if (string.IsNullOrWhiteSpace(query))
            {
                UpdateEventsList();
                return;
            }

            List<EventClass> filteredEvents = locationEvents.Values
                .SelectMany(evList => evList) 
                .Where(ev => ev.Title.ToLower().Contains(query) ||
                             ev.Location.ToLower().Contains(query) ||
                             ev.Category.ToLower().Contains(query) ||
                             ev.Venue.ToLower().Contains(query) ||
                             ev.EndDate.ToLower().Contains(query) ||
                             ev.StartDate.ToLower().Contains(query) ||
                             ev.StartTime.ToLower().Contains(query) ||
                             ev.EndTime.ToLower().Contains(query) ||
                             ev.Description.ToLower().Contains(query))
                .ToList();

            if (filteredEvents.Count == 0)
            {
                NoPostsMessageBox("No Posts Matching That Search Request", "Unfortunatley there is no posts matching that search request, feel free to try something else!");
                return;
            }
            // Display the filtered list of events
            UpdateEventsList(filteredEvents);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Post Canceled");
            ClearAllFields();
        }

        private void btnShowReccomended_Click(object sender, RoutedEventArgs e)
        {
            LocationTextBlock.Text = $"Recommendations";

            if (RecommendEventsBasedOnSearch("currentUserId").Count == 0)
            {
                NoPostsMessageBox("No Recommended Posts Yet", "Unfortunatley we cant run our algoritm to recommend you something if we have no data to work off of, please search something to build your search history");
                return;
            }
            UpdateEventsList(RecommendEventsBasedOnSearch("currentUserId"));
        }

        private void TrackUserSearch(string userId, string searchQuery)
        {
            if (!userSearchHistory.ContainsKey(userId))
            {
                userSearchHistory[userId] = new List<string>();
            }

            userSearchHistory[userId].Add(searchQuery.ToLower());
        }

        private List<EventClass> RecommendEventsBasedOnSearch(string userId)
        {
            if (!userSearchHistory.ContainsKey(userId) || userSearchHistory[userId].Count == 0)
            {
                return new List<EventClass>();  
            }

            List<string> pastSearches = userSearchHistory[userId];

            var frequentSearches = pastSearches.GroupBy(search => search)
                                               .OrderByDescending(group => group.Count())
                                               .Select(group => group.Key)
                                               .Take(3) 
                                               .ToList();

            List<EventClass> recommendedEvents = new List<EventClass>();

            foreach (var searchTerm in frequentSearches)
            {
                var matchingEvents = locationEvents.Values.SelectMany(evList => evList)
                                        .Where(ev => ev.Location.ToLower().Contains(searchTerm) ||
                                                     ev.Title.ToLower().Contains(searchTerm) ||
                                                     ev.Category.ToLower().Contains(searchTerm) ||
                                                     ev.Venue.ToLower().Contains(searchTerm) ||
                                                     ev.EndDate.ToLower().Contains(searchTerm) ||
                                                     ev.StartDate.ToLower().Contains(searchTerm) ||
                                                     ev.StartTime.ToLower().Contains(searchTerm) ||
                                                     ev.EndTime.ToLower().Contains(searchTerm) ||
                                                     ev.Description.ToLower().Contains(searchTerm))
                                                                .ToList();

                recommendedEvents.AddRange(matchingEvents);
            }

            return recommendedEvents;
        }

        private void txbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
