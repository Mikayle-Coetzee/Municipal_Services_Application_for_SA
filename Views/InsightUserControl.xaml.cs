using LiveCharts;
using LiveCharts.Wpf;
using PROG7312_ST10023767.Controllers;
using PROG7312_ST10023767.Models;
using PROG7312_ST10023767.Models.Managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace PROG7312_ST10023767.Views
{
    public partial class InsightUserControl : UserControl
    {
        public ChartValues<int> LineGraphValues { get; private set; }
        public ChartValues<int> BarGraphValues { get; private set; }
        public ObservableCollection<string> LineGraphLabels { get; private set; }
        public ObservableCollection<string> BarGraphLabels { get; private set; }
        public ObservableCollection<IssueClass> IssuesTable { get; private set; }
        public Func<double, string> YAxisFormatter { get; private set; }

        private readonly IssueTracker _issueTracker;
        public SeriesCollection PieChartSeries { get; private set; }

        public InsightUserControl(IssueTracker issueTracker)
        {
            InitializeComponent();

            _issueTracker = issueTracker ?? throw new ArgumentNullException(nameof(issueTracker), "IssueTracker cannot be null.");

            // Initialize properties
            LineGraphValues = new ChartValues<int>();
            BarGraphValues = new ChartValues<int>();
            LineGraphLabels = new ObservableCollection<string>();
            BarGraphLabels = new ObservableCollection<string>();
            IssuesTable = new ObservableCollection<IssueClass>();
            PieChartSeries = new SeriesCollection();

            YAxisFormatter = value => value.ToString("N0");

            // Populate data
            PopulateChartsAndTable();

            DataContext = this;
        }

        private void PopulateChartsAndTable()
        {
            var issues = _issueTracker.GetIssues()?.Where(i => i?.Timestamp != null && !string.IsNullOrEmpty(i?.Category)) ?? Enumerable.Empty<IssueClass>();

            // Populate data table
            foreach (var issue in issues)
            {
                try
                {
                    int statusAsInt = Convert.ToInt32(issue.Status);
                    string name = GetStatusName(statusAsInt);
                    issue.Status = name;
                    IssuesTable.Add(issue);
                }
                catch (FormatException)
                {
                    IssuesTable.Add(issue);
                }
            }

            PopulateLineGraph(issues);
            PopulateBarGraph(issues);
           // PopulatePieChart(issues);
            PopulateDependencyPieChart();
        }

        private void PopulateLineGraph(IEnumerable<IssueClass> issues)
        {
            var groupedByDate = issues
                .GroupBy(i => i.Timestamp.Date)
                .OrderBy(g => g.Key);

            foreach (var group in groupedByDate)
            {
                LineGraphLabels.Add(group.Key.ToString("MMM dd"));
                LineGraphValues.Add(group.Count());
            }
        }

        private void PopulateBarGraph(IEnumerable<IssueClass> issues)
        {
            var groupedByCategory = issues
                .GroupBy(i => i.Category);

            foreach (var group in groupedByCategory)
            {
                BarGraphLabels.Add(group.Key);
                BarGraphValues.Add(group.Count());
            }
        }

        private void PopulatePieChart(IEnumerable<IssueClass> issues)
        {
            var groupedByStatus = issues
                .GroupBy(i => i.Status)
                .Select(group => new { Status = group.Key, Count = group.Count() });

            var colors = new List<SolidColorBrush>
                {
                    new SolidColorBrush(Colors.Blue),
                    (SolidColorBrush)FindResource("greenSolidColorBrush"),
                    new SolidColorBrush(Colors.Gray)
                }; 
            
            int colorIndex = 0;

            foreach (var group in groupedByStatus)
            {
                PieChartSeries.Add(new PieSeries
                {
                    Title = group.Status,
                    Values = new ChartValues<int> { group.Count },
                    DataLabels = true,
                    Fill = colors[colorIndex % colors.Count]
                });

                colorIndex++;
            }
        }

        private void PopulateDependencyPieChart()
        {
            var allDependencies = _issueTracker.GetAllDependencies();

            // Define the list of colors to use in the pie chart slices
            var colors = new List<SolidColorBrush>
                    {
                        new SolidColorBrush(Colors.Blue),
                        (SolidColorBrush)FindResource("greenSolidColorBrush"),
                        new SolidColorBrush(Colors.Gray)
                    };

            int colorIndex = 0;

            foreach (var entry in allDependencies)
            {
                // Skip if the key (issue) is null
                if (entry.Key == null)
                    continue;

                // Get the list of dependencies for this issue; use an empty list if null
                var dependencies = entry.Value ?? new List<IssueClass>();

                // Group the dependencies by their status
                var statusCounts = dependencies
                    .GroupBy(dependency => GetStatusName(Convert.ToInt32(dependency.Status)))
                    .Select(group => new { Status = group.Key, Count = group.Count() });

                foreach (var statusGroup in statusCounts)
                {
                    // Check if a PieSeries already exists for this status
                    var existingSeries = PieChartSeries.FirstOrDefault(p => p.Title.Contains(statusGroup.Status));

                    if (existingSeries != null)
                    {
                        // Update the series with new values (add the count)
                        existingSeries.Values[0] = (int)existingSeries.Values[0] + statusGroup.Count;
                    }
                    else
                    {
                        // Add a new PieSeries for this status if it doesn't exist
                        PieChartSeries.Add(new PieSeries
                        {
                            Title = $"{statusGroup.Status} ({statusGroup.Count})",
                            Values = new ChartValues<int> { statusGroup.Count },
                            DataLabels = true,
                            Fill = colors[colorIndex % colors.Count] // Assign a color from the list
                        });

                        colorIndex++;
                    }
                }

                // Handle the case where the key itself represents an independent issue (with no dependencies)
                if (!dependencies.Any())
                {
                    var statusName = entry.Key.Status ;

                    // Check if a PieSeries for this status already exists
                    var existingSeries = PieChartSeries.FirstOrDefault(p => p.Title.Contains(statusName));

                    if (existingSeries != null)
                    {
                        // Increment the count of the existing series
                        existingSeries.Values[0] = (int)existingSeries.Values[0] + 1;
                    }
                    else
                    {
                        // Add a new PieSeries for this independent issue
                        PieChartSeries.Add(new PieSeries
                        {
                            Title = $"{statusName}",
                            Values = new ChartValues<int> { 1 },
                            DataLabels = true,
                            Fill = colors[colorIndex % colors.Count] // Assign a color
                        });

                        colorIndex++;
                    }
                }
            }
        }


        private string GetStatusName(int status)
        {
            switch (status)
            {
                case 0:
                    return "Pending";
                case 1:
                    return "Active";
                case 2:
                    return "Resolved";
                default:
                    return status.ToString();
            }
        }
    }
}
