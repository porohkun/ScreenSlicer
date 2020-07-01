using ScreenSlicer.Compatibility;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ScreenSlicer.Commands
{
    public class NewCompatibilityRuleCommand : InjectableCommand<NewCompatibilityRuleCommand>
    {
        const string NewRuleName = "New Rule";
        const string NewRuleName2More = "New Rule ({0})";
        const string NewRuleNamePattern = "New Rule \\(\\d{1,9}\\)";
        const int startIndex = 10;
        const int lengthOffset = 11;

        protected override bool CanExecuteInternal(object parameter)
        {
            return true;
        }

        protected override void ExecuteInternal(object parameter)
        {
            var newRuleIndex = 0;
            foreach (var rule in Settings.Instance.Compatibility.Rules)
            {
                if (rule.Name == NewRuleName)
                    newRuleIndex = Math.Max(newRuleIndex, 1);
                else if (Regex.IsMatch(rule.Name, NewRuleNamePattern))
                    newRuleIndex = Math.Max(newRuleIndex, int.Parse(rule.Name.Substring(startIndex, rule.Name.Length - lengthOffset)));
            }
            newRuleIndex++;
            Settings.Instance.Compatibility.Rules.Add(new Rule()
            {
                Name = newRuleIndex == 1 ? NewRuleName : string.Format(NewRuleName2More, newRuleIndex)
            });
        }
    }
}
