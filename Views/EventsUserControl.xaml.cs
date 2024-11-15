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
        private FilterAndRecommendHelper filterAndRecommendHelper = new FilterAndRecommendHelper();


        private DisplayHelper displayHelper = new DisplayHelper();

        /// <summary>
        /// New instance of the media helper class
        /// </summary>
        private MediaService mediaHelper = new MediaService();

        /// <summary>
        /// Manages the creation and storage of posts
        /// </summary>
        private PostManager PostManager;


        private IssueManager IssueManager;

        /// <summary>
        /// Dictionary to store events sorted by location
        /// </summary>
        private Stack<MediaFileClass> mediaAttachments = new Stack<MediaFileClass>();

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="postManager"></param>
        public EventsUserControl(PostManager postManager, IssueManager issueManager)
        {
            this.PostManager = postManager;
            this.IssueManager = issueManager;

            InitializeComponent();
            LoadLocations();
            UpdateEventsList();

            SetDateToNow(EventDate);
            SetDateToNow(EndDate);
        }

        #region Load and Display Methods

        /// <summary>
        /// Loads location buttons from PostManager into the UI
        /// </summary>
        private void LoadLocations()
        {
            foreach (var location in PostManager.locationEvents.Keys)
            {
                Button locationButton = new Button
                {
                    Content = location,
                    Margin = new Thickness(5),
                    Style = (Style)FindResource("RoundedButtonStyle"),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalContentAlignment = HorizontalAlignment.Stretch
                };
                locationButton.Click += LocationButton_Click;
                LocationWrapPanel.Children.Add(locationButton);
            }
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Updates the events list to show all events and announcmnets 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewAllEvents_Click(object sender, RoutedEventArgs e)
        {
            UpdateEventsList();
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Updates the event list based on the selected location.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocationButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string selectedLocation = button.Content.ToString();

            cmbFilter.SelectedIndex = 0;

            LocationTextBlock.Text = $"Events & Announcement in {selectedLocation}";

            EventsList.Items.Clear();

            if (PostManager.locationEvents.ContainsKey(selectedLocation))
            {
                if (PostManager.locationEvents[selectedLocation].Count == 0)
                {
                    NoPostsMessageBox("No Posts Posted Yet", "Be a champ in your community by clicking on the 'Create Post' and posting first!");
                    return;
                }
                UpdateEventsList(PostManager.locationEvents[selectedLocation]);
            }
        }

        /// <summary>
        /// Displays a message when there are no posts.
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Message"></param>
        private void NoPostsMessageBox(string Title, string Message)
        {
            if (EventsList != null)
            {
                ClearEventsList();
                Border eventBorder = displayHelper.CreateEventBorderWithPanel(Title, Message);
                EventsList.Items.Add(eventBorder);
            }
        }
        #endregion

        #region Date and Time Methods
        /// <summary>
        /// Sets the date picker to the current date.
        /// </summary>
        public void SetDateToNow(DatePicker datePicker)
        {
            if (datePicker != null)
            {
                datePicker.SelectedDate = DateTime.Now;
            }
        }
        #endregion

        #region Media Management Methods
        /// <summary>
        /// Opens a file dialog to select media files and adds them to the mediaAttachments stack.
        /// </summary>
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

                    string mediaType = mediaHelper.GetMediaType(filename);

                    MediaFileClass mediaFile = new MediaFileClass(System.IO.Path.GetFileName(filename),
                        fileContent, mediaType);
                    mediaAttachments.Push(mediaFile);
                    MediaList.Items.Add(System.IO.Path.GetFileName(filename));
                }
            }
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Removes the selected media item from the mediaAttachments stack.
        /// </summary>
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

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Opens the selected media item for viewing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        #endregion

        #region Event Submission Methods
        /// <summary>
        /// Validates input fields and submits a new event.
        /// </summary>
        private void SubmitEventButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }

            string title = EventTitle.Text;
            string date = EventDate.SelectedDate?.ToString("d MMM yyyy") ?? "";
            string location = EventLocation.Text;
            string description = EventDescription.Text;
            string type = (cmbType.SelectedItem as ComboBoxItem)?.Content.ToString();
            string category = (cmbCategory.SelectedItem as ComboBoxItem)?.Content.ToString();
            string endDate = EndDate.SelectedDate?.ToString("d MMM yyyy") ?? "";
            string venue = EventVenue.Text;

            string time = GetSelectedTime(cmbHour, cmbMinute, cmbAmPm);
            string timeEnd = GetSelectedTime(cmbHourEnd, cmbMinuteEnd, cmbAmPmEnd);

            AddCategoryToUniqueCategories(category);

            if (IsNewLocation(location))
            {
                AddNewLocation(location);
            }

            CreateAndSaveEvent(title, date, time, location, description, type, category, endDate, venue, timeEnd);

            ClearAllFields();

            MessageBox.Show("Event submitted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Validates the input fields to ensure all required information is present and correct.
        /// </summary>
        /// <returns></returns>
        private bool ValidateFields()
        {
            string title = EventTitle.Text;
            DateTime? startDate = EventDate.SelectedDate;
            DateTime? endDate = EndDate.SelectedDate;
            string location = EventLocation.Text;
            string time = GetSelectedTime(cmbHour, cmbMinute, cmbAmPm);
            string timeEnd = GetSelectedTime(cmbHourEnd, cmbMinuteEnd, cmbAmPmEnd);
            string description = EventDescription.Text;
            string venue = EventVenue.Text;

            // Check if all fields are filled
            if (string.IsNullOrWhiteSpace(title) || !startDate.HasValue || !endDate.HasValue || string.IsNullOrWhiteSpace(location) ||
                string.IsNullOrEmpty(time) || string.IsNullOrEmpty(timeEnd) || string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(venue))
            {
                MessageBox.Show("Please fill in all the fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            DateTime currentDate = DateTime.Now.Date;

            // Validate if the start date is not in the past
            if (startDate.Value.Date < currentDate && endDate.Value.Date < currentDate)
            {
                MessageBox.Show("Start date cannot be in the past.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Validate  if the end date is not in the past
            if (endDate.Value.Date < currentDate)
            {
                MessageBox.Show("End date cannot be in the past.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Validate so that start date cannot be after end date
            if (startDate.Value.Date > endDate.Value.Date)
            {
                MessageBox.Show("Start date cannot be later than end date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // If the dates are the same, validate so that start time cannot be later than end time
            if (startDate.Value.Date == endDate.Value.Date)
            {
                TimeSpan startTime = GetTimeSpan(cmbHour, cmbMinute, cmbAmPm);
                TimeSpan endTime = GetTimeSpan(cmbHourEnd, cmbMinuteEnd, cmbAmPmEnd);

                if (startTime > endTime)
                {
                    MessageBox.Show("Start time cannot be later than end time for the same day.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            return true;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        ///  Gets the selected time span from the provided ComboBoxes
        /// </summary>
        /// <param name="cmbHour"></param>
        /// <param name="cmbMinute"></param>
        /// <param name="cmbAmPm"></param>
        /// <returns></returns>
        private TimeSpan GetTimeSpan(ComboBox cmbHour, ComboBox cmbMinute, ComboBox cmbAmPm)
        {
            string selectedHour = (cmbHour.SelectedItem as ComboBoxItem)?.Content.ToString();
            string selectedMinute = (cmbMinute.SelectedItem as ComboBoxItem)?.Content.ToString();
            string selectedAmPm = (cmbAmPm.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(selectedHour) || string.IsNullOrEmpty(selectedMinute) || string.IsNullOrEmpty(selectedAmPm))
            {
                return TimeSpan.Zero;
            }

            int hour = int.Parse(selectedHour);
            if (selectedAmPm == "PM" && hour != 12)
            {
                hour += 12;
            }
            else if (selectedAmPm == "AM" && hour == 12)
            {
                hour = 0;
            }

            int minute = int.Parse(selectedMinute);
            return new TimeSpan(hour, minute, 0);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Gets the selected time from the provided ComboBoxes
        /// </summary>
        /// <param name="cmbHour"></param>
        /// <param name="cmbMinute"></param>
        /// <param name="cmbAmPm"></param>
        /// <returns></returns>
        private string GetSelectedTime(ComboBox cmbHour, ComboBox cmbMinute, ComboBox cmbAmPm)
        {
            string selectedHour = (cmbHour.SelectedItem as ComboBoxItem)?.Content.ToString();
            string selectedMinute = (cmbMinute.SelectedItem as ComboBoxItem)?.Content.ToString();
            string selectedAmPm = (cmbAmPm.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(selectedHour) || string.IsNullOrEmpty(selectedMinute) || string.IsNullOrEmpty(selectedAmPm))
            {
                MessageBox.Show("Please select a valid time.");
                return null;
            }

            return $"{selectedHour}:{selectedMinute} {selectedAmPm}";
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Clears all input fields after submission
        /// </summary>
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

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Creates and saves a new event using the provided information.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <param name="location"></param>
        /// <param name="description"></param>
        /// <param name="type"></param>
        /// <param name="category"></param>
        /// <param name="endDate"></param>
        /// <param name="venue"></param>
        /// <param name="timeEnd"></param>
        private void CreateAndSaveEvent(string title, string date, string time, string location, string description, string type, string category, string endDate, string venue, string timeEnd)
        {
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

            PostManager.locationEvents[location].Add(newEvent);
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// Adds the selected category to the unique categories list.
        /// </summary>
        /// <param name="category"></param>
        private void AddCategoryToUniqueCategories(string category)
        {
            if (!string.IsNullOrEmpty(category))
            {
                PostManager.uniqueCategories.Add(category);
            }
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Checks if the location is new
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private bool IsNewLocation(string location)
        {
            return !PostManager.locationEvents.ContainsKey(location);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Adds a new location to the PostManager
        /// </summary>
        /// <param name="location"></param>
        private void AddNewLocation(string location)
        {
            venueButtonsPanel.Visibility = Visibility.Visible;
            LocationWrapPanel.Visibility = Visibility.Visible;

            PostManager.locationEvents[location] = new List<EventClass>();

            Button locationButton = new Button
            {
                Content = location,
                Margin = new Thickness(5),
                Style = (Style)FindResource("RoundedButtonStyle"),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalContentAlignment = HorizontalAlignment.Stretch
            };

            locationButton.Click += LocationButton_Click;

            if (LocationWrapPanel.ActualWidth != 0)
            {
                locationButton.Width = LocationWrapPanel.ActualWidth - 10; 
            }

            LocationWrapPanel.Children.Add(locationButton);
        }

        #endregion

        #region UI Resizing

        /// <summary>
        /// Adjusts the width of buttons in the LocationWrapPanel when its size changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocationWrapPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (UIElement child in LocationWrapPanel.Children)
            {
                if (child is Button button)
                {
                    button.Width = LocationWrapPanel.ActualWidth - 10;
                }
            }
        }

        #endregion

        #region Event Filtering
        /// <summary>
        ///  Handles the selection change in the filter ComboBox to display filtered events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFilter = (cmbFilter.SelectedItem as ComboBoxItem)?.Content.ToString();
            string selectedLocation = GetSelectedLocation();

            List<EventClass> eventsToDisplay = GetEventsToDisplay(selectedLocation);

            DateTime currentDateTime = DateTime.Now;

            // Apply filter based on the filter selected  
            eventsToDisplay = ApplySelectedFilter(selectedFilter, eventsToDisplay, currentDateTime);

            // Show message if no posts match the filter
            if (eventsToDisplay.Count == 0)
            {
                NoPostsMessageBox("No Posts Matching That Filter Request",
                    "Unfortunately, there are no posts that match your filtering requests. Feel free to add more posts or apply a different filter.");
                LocationTextBlock.Text = "Filtered Posts";

                return;
            }

            // Update the post list with filtered posts
            UpdateEventsList(eventsToDisplay);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Gets the selected location from the UI
        /// </summary>
        /// <returns></returns>
        private string GetSelectedLocation()
        {
            return LocationTextBlock.Text.Contains("Selected")
                ? LocationTextBlock.Text.Replace("Events & Announcement in Selected ", "")
                : null;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Fetches events to display based on the selected location
        /// </summary>
        /// <param name="selectedLocation"></param>
        /// <returns></returns>
        private List<EventClass> GetEventsToDisplay(string selectedLocation)
        {
            if (selectedLocation != null && PostManager.locationEvents.ContainsKey(selectedLocation))
            {
                return PostManager.locationEvents[selectedLocation];
            }
            return PostManager.locationEvents.Values.SelectMany(evList => evList).ToList();
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Applies the selected filter to the list of events
        /// </summary>
        /// <param name="selectedFilter"></param>
        /// <param name="eventsToDisplay"></param>
        /// <param name="currentDateTime"></param>
        /// <returns></returns>
        private List<EventClass> ApplySelectedFilter(string selectedFilter, List<EventClass> eventsToDisplay, DateTime currentDateTime)
        {
            switch (selectedFilter)
            {
                case "All":
                    LocationTextBlock.Text = "All Events & Announcements";

                    return eventsToDisplay.ToList();

                case "Events":
                    LocationTextBlock.Text = "All Events";

                    return eventsToDisplay.Where(item => item.Type == "Event").ToList();

                case "Announcements":
                    LocationTextBlock.Text = "All Announcements";

                    return eventsToDisplay.Where(item => item.Type == "Announcement").ToList();

                case "Category":
                    LocationTextBlock.Text = "Filtered By Category";

                    return filterAndRecommendHelper.FilterByCategory(eventsToDisplay, PostManager.uniqueCategories);

                case "Upcoming":
                    LocationTextBlock.Text = "Upcoming Events & Announcements";

                    return eventsToDisplay.Where(item => currentDateTime < DateTime.Parse(item.StartDate)).ToList();

                case "Past":
                    LocationTextBlock.Text = "Past Events & Announcements";

                    return eventsToDisplay.Where(item => filterAndRecommendHelper.IsPastEvent(item, currentDateTime)).ToList();

                case "Busy":
                    LocationTextBlock.Text = "Events & Announcements Currently Busy" + selectedFilter;

                    return eventsToDisplay.Where(item => filterAndRecommendHelper.IsBusyEvent(item, currentDateTime)).ToList();

                default:
                    LocationTextBlock.Text = "Filtered Posts";

                    return eventsToDisplay;

            }

        }

        #endregion

        #region Post List Management
        /// <summary>
        /// Updates the post list with filtered or all events
        /// </summary>
        /// <param name="filteredEvents"></param>
        private void UpdateEventsList(List<EventClass> filteredEvents = null)
        {
            if (EventsList != null)
            {
                ClearEventsList();

                LocationTextBlock.Text = filteredEvents == null ? "All Events & Announcements" : LocationTextBlock.Text;

                var eventsToDisplay = GetEventsToDisplay(filteredEvents);

                if (eventsToDisplay.Count == 0)
                {
                    ShowNoPostsMessage();
                    return;
                }

                foreach (var ev in eventsToDisplay)
                {
                    DisplayEvent(ev);
                }
            }
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Clears all items in the events list.
        /// </summary>
        private void ClearEventsList()
        {
            EventsList.Items.Clear();
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Gets the list of posts to display, ordering by start date.
        /// </summary>
        /// <param name="filteredEvents"></param>
        /// <returns></returns>
        private List<EventClass> GetEventsToDisplay(List<EventClass> filteredEvents)
        {
            return filteredEvents ?? PostManager.locationEvents.Values.SelectMany(evList => evList).OrderBy(e => e.StartDate).ToList();
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        ///  Adds media files related to the event to the panel
        /// </summary>
        /// <param name="eventPanel"></param>
        /// <param name="ev"></param>
        private void AddMediaFilesToPanel(StackPanel eventPanel, EventClass ev, IssueClass ic)
        {
            if (ev != null)
            {
                foreach (var media in ev.MediaFiles)
                {

                    if (media.MediaType == "Image")
                    {
                        Image imageItem = new Image
                        {
                            Height = 200,
                            Margin = new Thickness(10),
                            Stretch = Stretch.Fill
                        };

                        if (media.FileContent != null && media.FileContent.Length > 0)
                        {
                            imageItem.Source = mediaHelper.ByteArrayToImage(media.FileContent);
                        }
                        else if (!string.IsNullOrEmpty(media.FileName))
                        {
                            mediaHelper.LoadImage(media.FileName, imageItem);
                        }

                        eventPanel.Children.Add(imageItem);
                    }
                    else if (media.MediaType == "Video")
                    {
                        MediaElement videoItem = new MediaElement
                        {
                            Height = 200,
                            Margin = new Thickness(10),
                            Stretch = Stretch.Fill,
                            LoadedBehavior = MediaState.Play,
                            UnloadedBehavior = MediaState.Stop,
                        };

                        if (media.FileContent != null && media.FileContent.Length > 0)
                        {
                            string tempFilePath = System.IO.Path.GetTempFileName() + ".mp4";
                            System.IO.File.WriteAllBytes(tempFilePath, media.FileContent);
                            videoItem.Source = new Uri(tempFilePath, UriKind.Absolute);
                        }
                        else if (!string.IsNullOrEmpty(media.FileName))
                        {
                            videoItem.Source = new Uri(media.FileName, UriKind.RelativeOrAbsolute);
                        }

                        eventPanel.Children.Add(videoItem);
                    }
                }

                TextBlock mediaBlock = new TextBlock
                {
                    Text = "All Media Files (Click on the file names to open):",
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(5, 10, 0, 5),
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
            else
            {
                foreach (var media in ic.Attachments)
                {
                    //code
                }
            }
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Displays a message when there are no posts available
        /// </summary>
        private void ShowNoPostsMessage()
        {
            NoPostsMessageBox("No Posts Posted Yet", "Be a champ in your community by posting first!");
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Displays the event details on the UI
        /// </summary>
        /// <param name="ev"></param>
        private void DisplayEvent(EventClass ev)
        {
            DateTime currentDateTime = DateTime.Now;
            string status = displayHelper.DetermineEventStatus(ev, null, currentDateTime, out Border statusBorder);

            Console.WriteLine($"Status for event '{ev.Title}': {status}");

            Border eventBorder = displayHelper.CreateEventBorder(ev, null);
            StackPanel eventPanel = displayHelper.CreateEventPanel(statusBorder, ev,null);

            // Display the post
            AddEventDetailsToPanel(eventPanel, ev, null);

            eventBorder.Child = eventPanel;

            EventsList.Items.Add(eventBorder);
        }
        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Adds detailed information about the event to the provided panel
        /// </summary>
        /// <param name="eventPanel"></param>
        /// <param name="ev"></param>
        private void AddEventDetailsToPanel(StackPanel eventPanel, EventClass ev, IssueClass ic)
        {
            if (ev != null)
            {

                eventPanel.Children.Add(new TextBlock
                {
                    Text = "Title: " + ev.Title,
                    Margin = new Thickness(5, 5, 0, 5),
                    FontWeight = FontWeights.Bold,
                    TextWrapping = TextWrapping.Wrap
                });

                eventPanel.Children.Add(new TextBlock
                {
                    Text = "Description: " + ev.Description,
                    Margin = new Thickness(5, 5, 0, 5),
                    TextWrapping = TextWrapping.Wrap
                });

                eventPanel.Children.Add(new TextBlock
                {
                    Text = $"Venue: {ev.Venue}",
                    Margin = new Thickness(5, 5, 0, 5),
                    FontWeight = FontWeights.Bold,
                    TextWrapping = TextWrapping.Wrap
                });

                eventPanel.Children.Add(new TextBlock
                {
                    Text = $"City/Area: {ev.Location}",
                    Margin = new Thickness(5, 5, 0, 5),
                    FontWeight = FontWeights.Bold,
                    TextWrapping = TextWrapping.Wrap
                });

                if (ev.MediaFiles != null && ev.MediaFiles.Count > 0)
                {
                    AddMediaFilesToPanel(eventPanel, ev, null);
                }
            }
            else
            {

                eventPanel.Children.Add(new TextBlock
                {
                    Text = "Category: " + ic.Category,
                    Margin = new Thickness(5, 5, 0, 5),
                    FontWeight = FontWeights.Bold,
                    TextWrapping = TextWrapping.Wrap
                });

                eventPanel.Children.Add(new TextBlock
                {
                    Text = "Description: " + ic.Description,
                    Margin = new Thickness(5, 5, 0, 5),
                    TextWrapping = TextWrapping.Wrap
                });

                eventPanel.Children.Add(new TextBlock
                {
                    Text = $"Location: {ic.Location}",
                    Margin = new Thickness(5, 5, 0, 5),
                    FontWeight = FontWeights.Bold,
                    TextWrapping = TextWrapping.Wrap
                });

                if (ic.Attachments != null && ic.Attachments.Count > 0)
                {
                    AddMediaFilesToPanel(eventPanel, null, ic);
                }
            }
        }


        #endregion

        #region Button Click Handlers

        /// <summary>
        /// Toggles the visibility of the venue buttons panel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewByArea_Click(object sender, RoutedEventArgs e)
        {
            venueButtonsPanel.Visibility = venueButtonsPanel.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Toggles the visibility of the event creation panel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddEvent_Click(object sender, RoutedEventArgs e)
        {
            createPostPanel.Visibility = createPostPanel.Visibility == Visibility.Visible
               ? Visibility.Collapsed
               : Visibility.Visible;
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

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Searches for events based on the user's query and updates the events list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            LocationTextBlock.Text = $"Search Results";

            string searchQuery = txbSearch.Text;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                filterAndRecommendHelper.TrackUserSearch("currentUserId", searchQuery, PostManager.userSearchHistory);
            }


            string query = searchQuery.ToLower().Trim();

            if (string.IsNullOrWhiteSpace(query))
            {
                UpdateEventsList();
                txbSearch.Text = "";
                return;
            }

            //search through everything related to a post to find matches
            List<EventClass> filteredEvents = PostManager.locationEvents.Values
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
                txbSearch.Text = "";
                return;
            }
            
            // Display the filtered list of posts
            UpdateEventsList(filteredEvents);
            txbSearch.Text = "";
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Cancels the current post and clears all input fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Post Canceled");
            ClearAllFields();
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        ///  Shows recommended events based on the user's search history
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowReccomended_Click(object sender, RoutedEventArgs e)
        {
            LocationTextBlock.Text = $"Recommendations";

            if (filterAndRecommendHelper.RecommendEventsBasedOnSearch("currentUserId", PostManager.userSearchHistory, PostManager.locationEvents).Count == 0)
            {
                NoPostsMessageBox("No Recommended Posts Yet", "Unfortunatley we cant run our algoritm to recommend you something if we have no data to work off of, please search something to build your search history");
                return;
            }
            UpdateEventsList(filterAndRecommendHelper.RecommendEventsBasedOnSearch("currentUserId", PostManager.userSearchHistory, PostManager.locationEvents));
        }

        #endregion

        #region Text Change Handlers

        /// <summary>
        /// Handles changes in the search text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        #endregion

        /// <summary>
        /// This method will make the buttons visible or collapsed when clicked on the 'Event' button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEvents_Click(object sender, RoutedEventArgs e)
        {
            Visibility newVisibility = BtnViewByArea.Visibility == Visibility.Visible
                  ? Visibility.Collapsed
                  : Visibility.Visible;

            BtnViewByArea.Visibility = newVisibility;
            btnShowReccomended.Visibility = newVisibility;
            btnAddEvent.Visibility = newVisibility;

            if (newVisibility == Visibility.Collapsed)
            {
                venueButtonsPanel.Visibility = newVisibility;
                createPostPanel.Visibility = newVisibility;
            }
        }

        private void BtnServiceRequestStatus_Click(object sender, RoutedEventArgs e)
        {
            UpdateServicesList();
        }

        /// <summary>
        /// Updates the post list with filtered or all events
        /// </summary>
        /// <param name="filteredEvents"></param>
        private void UpdateServicesList(List<EventClass> filteredEvents = null)
        {
            if (EventsList != null)
            {
                ClearEventsList();

                LocationTextBlock.Text = filteredEvents == null ? "All Service Requests" : LocationTextBlock.Text;

                var reportsToDisplay = GetReportsToDisplay();

                if (reportsToDisplay.Count == 0)
                {
                    ShowNoReportsMessage();
                    return;
                }

                foreach (var ic in reportsToDisplay)
                {
                    DisplayReport(ic);
                }
            }
        }
        private List<IssueClass> GetReportsToDisplay()
        {
            return IssueManager.issues;
        }

        private void ShowNoReportsMessage()
        {
            NoPostsMessageBox("No Service Issue Reported Yet", "Feel free to report an issue!");
        }

        private void DisplayReport(IssueClass ic)
        {
            DateTime currentDateTime = DateTime.Now;
            string status = displayHelper.DetermineEventStatus(null, ic, currentDateTime, out Border statusBorder);

            Console.WriteLine($"Status for issue '{ic.Category}': {ic.Timestamp}");

            Border eventBorder = displayHelper.CreateEventBorder(null, ic);
            StackPanel eventPanel = displayHelper.CreateEventPanel(statusBorder, null,  ic);

            // Display the post
            AddEventDetailsToPanel(eventPanel, null,  ic);

            eventBorder.Child = eventPanel;

            EventsList.Items.Add(eventBorder);
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
