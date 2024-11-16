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

namespace PROG7312_ST10023767.Views
{
    public partial class InsightUserControl : UserControl
    {
        public ChartValues<int> LineGraphValues { get; set; }
        public ChartValues<int> BarGraphValues { get; set; }
        public ObservableCollection<string> LineGraphLabels { get; set; }
        public ObservableCollection<string> BarGraphLabels { get; set; }
        public ObservableCollection<IssueClass> IssuesTable { get; set; }
        public Func<double, string> YAxisFormatter { get; set; }

        private readonly IssueTracker _issueTracker;

        public InsightUserControl(IssueTracker issueTracker)
        {
            InitializeComponent();

            if (issueTracker == null)
                throw new ArgumentNullException(nameof(issueTracker), "IssueTracker cannot be null.");

            _issueTracker = issueTracker;

            // Initialize properties
            LineGraphValues = new ChartValues<int>();
            BarGraphValues = new ChartValues<int>();
            LineGraphLabels = new ObservableCollection<string>();
            BarGraphLabels = new ObservableCollection<string>();
            IssuesTable = new ObservableCollection<IssueClass>();

            YAxisFormatter = value => value.ToString("N0");

            // Populate data
            PopulateChartsAndTable();

            DataContext = this;
        }


        private void PopulateChartsAndTable()
        {
            // Get issues and ensure the list is not null
            var issues = _issueTracker.GetIssues() ?? new List<IssueClass>();

            // Filter out issues where Timestamp or Category is null
            var filteredIssues = issues.Where(i => i?.Timestamp != null && !string.IsNullOrEmpty(i?.Category)).ToList();

            // Populate data table
            foreach (var issue in filteredIssues)
            {
                IssuesTable.Add(issue);
            }

            // Populate Line Graph (e.g., Issues per Day)
            var groupedByDate = filteredIssues
                                .GroupBy(i => i.Timestamp.Date) // Grouping by the Date part of Timestamp
                                .OrderBy(g => g.Key)
                                .ToList(); // To List for debugging

            foreach (var group in groupedByDate)
            {
                LineGraphLabels.Add(group.Key.ToString("MMM dd"));
                int count = group.Count();
                LineGraphValues.Add(count);

                // Debugging: check what values are being added
                Console.WriteLine($"Date: {group.Key}, Count: {count}");
            }

            // Populate Bar Graph (e.g., Issues per Category)
            var groupedByCategory = filteredIssues
                                    .GroupBy(i => i.Category) // Grouping by Category
                                    .ToList(); // To List for debugging

            foreach (var group in groupedByCategory)
            {
                BarGraphLabels.Add(group.Key);
                int count = group.Count();
                BarGraphValues.Add(count);

                // Debugging: check what values are being added
                Console.WriteLine($"Category: {group.Key}, Count: {count}");
            }
        }


    }
}
