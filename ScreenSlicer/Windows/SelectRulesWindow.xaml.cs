using NuGet;
using ScreenSlicer.Commands;
using ScreenSlicer.Compatibility;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace ScreenSlicer.Windows
{
    public class SelectRulesWindowDummy : SelectRulesWindowViewModel
    {
        public SelectRulesWindowDummy()
        {
            Rules = new ObservableCollection<RestoreDefaultRules.SelectingRule>()
            {
                new RestoreDefaultRules.SelectingRule()
                {
                    Rule = new Rule() { Name = "Rule 1" },
                    IsSelected = true
                },
                new RestoreDefaultRules.SelectingRule()
                {
                    Rule = new Rule() { Name = "Rule 2" },
                    IsSelected = true
                },
                new RestoreDefaultRules.SelectingRule()
                {
                    Rule = new Rule() { Name = "Rule 3" },
                    IsSelected = true
                }
            };
        }
    }

    public class SelectRulesWindowViewModel
    {
        public ObservableCollection<RestoreDefaultRules.SelectingRule> Rules { get; protected set; }

        public SelectRulesWindowViewModel()
        {
            Rules = new ObservableCollection<RestoreDefaultRules.SelectingRule>();
        }
    }

    /// <summary>
    /// Interaction logic for SelectRulesWindow.xaml
    /// </summary>
    public partial class SelectRulesWindow : Window
    {
        private RestoreDefaultRules.SelectingRule[] rulesTuple;

        public SelectRulesWindow()
        {
            DataContext = new SelectRulesWindowViewModel();
            InitializeComponent();
        }

        public SelectRulesWindow(RestoreDefaultRules.SelectingRule[] rulesTuple) : this()
        {
            var rules = (DataContext as SelectRulesWindowViewModel).Rules;
            rules.Clear();
            rules.AddRange(rulesTuple);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
