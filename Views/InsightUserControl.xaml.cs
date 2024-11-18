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
        /// <summary>
        /// Stores data values for the line graph.
        /// </summary>
        public ChartValues<int> LineGraphValues { get; private set; }

        /// <summary>
        ///  Stores data values for the bar graph.
        /// </summary>
        public ChartValues<int> BarGraphValues { get; private set; }

        /// <summary>
        /// Stores labels for the line graph 
        /// </summary>
        public ObservableCollection<string> LineGraphLabels { get; private set; }

        /// <summary>
        ///  Stores labels for the bar graph 
        /// </summary>
        public ObservableCollection<string> BarGraphLabels { get; private set; }

        /// <summary>
        /// Stores issue data displayed in the table.
        /// </summary>
        public ObservableCollection<IssueClass> IssuesTable { get; private set; }

        /// <summary>
        /// Formats the Y-axis labels for graphs.
        /// </summary>
        public Func<double, string> YAxisFormatter { get; private set; }

        /// <summary>
        /// The issue tracker providing data for the graphs and table.
        /// </summary>
        private readonly IssueTracker _issueTracker;

        /// <summary>
        /// Stores data series for the pie chart.
        /// </summary>
        public SeriesCollection PieChartSeries { get; private set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        ///  Initializes the InsightUserControl with the specified IssueTracker instance.
        /// </summary>
        /// <param name="issueTracker"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public InsightUserControl(IssueTracker issueTracker)
        {
            InitializeComponent();

            _issueTracker = issueTracker ?? throw new ArgumentNullException(nameof(issueTracker), "IssueTracker cannot be null.");

            LineGraphValues = new ChartValues<int>();
            BarGraphValues = new ChartValues<int>();
            LineGraphLabels = new ObservableCollection<string>();
            BarGraphLabels = new ObservableCollection<string>();
            IssuesTable = new ObservableCollection<IssueClass>();
            PieChartSeries = new SeriesCollection();

            YAxisFormatter = value => value.ToString("N0");

            PopulateChartsAndTable();

            DataContext = this;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        ///  Populates charts and the issue table with data from the issue tracker.
        /// </summary>
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

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Populates the line graph with the number of issues grouped by date.
        /// </summary>
        /// <param name="issues"></param>
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

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Populates the bar graph with the number of issues grouped by category.
        /// </summary>
        /// <param name="issues"></param>
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

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Populates the pie chart showing issue statuses.
        /// </summary>
        /// <param name="issues"></param>
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

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Populates the dependency-based pie chart showing issue relationships and using Graph
        /// </summary>
        private void PopulateDependencyPieChart()
        {
            var allDependencies = _issueTracker.GetAllDependencies();

            var colors = new List<SolidColorBrush>
                    {
                        new SolidColorBrush(Colors.Blue),
                        (SolidColorBrush)FindResource("greenSolidColorBrush"),
                        new SolidColorBrush(Colors.Gray)
                    };

            int colorIndex = 0;

            foreach (var entry in allDependencies)
            {
                if (entry.Key == null)
                    continue;

                var dependencies = entry.Value ?? new List<IssueClass>();

                var statusCounts = dependencies
                    .GroupBy(dependency => GetStatusName(Convert.ToInt32(dependency.Status)))
                    .Select(group => new { Status = group.Key, Count = group.Count() });

                foreach (var statusGroup in statusCounts)
                {
                    var existingSeries = PieChartSeries.FirstOrDefault(p => p.Title.Contains(statusGroup.Status));

                    if (existingSeries != null)
                    {
                        existingSeries.Values[0] = (int)existingSeries.Values[0] + statusGroup.Count;
                    }
                    else
                    {
                        PieChartSeries.Add(new PieSeries
                        {
                            Title = $"{statusGroup.Status} ({statusGroup.Count})",
                            Values = new ChartValues<int> { statusGroup.Count },
                            DataLabels = true,
                            Fill = colors[colorIndex % colors.Count]
                        });

                        colorIndex++;
                    }
                }

                if (!dependencies.Any())
                {
                    var statusName = entry.Key.Status ;

                    var existingSeries = PieChartSeries.FirstOrDefault(p => p.Title.Contains(statusName));

                    if (existingSeries != null)
                    {
                        existingSeries.Values[0] = (int)existingSeries.Values[0] + 1;
                    }
                    else
                    {
                        PieChartSeries.Add(new PieSeries
                        {
                            Title = $"{statusName}",
                            Values = new ChartValues<int> { 1 },
                            DataLabels = true,
                            Fill = colors[colorIndex % colors.Count]
                        });

                        colorIndex++;
                    }
                }
            }
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Converts a status code to its corresponding status name.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
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
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//

