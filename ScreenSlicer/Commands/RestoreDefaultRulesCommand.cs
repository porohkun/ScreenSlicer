using Newtonsoft.Json;
using ScreenSlicer.Compatibility;
using ScreenSlicer.Properties;
using ScreenSlicer.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace ScreenSlicer.Commands
{
    /// <summary>
    /// Parameter means "forced restore, without dialog"
    /// </summary>
    public class RestoreDefaultRulesCommand : InjectableCommand<RestoreDefaultRulesCommand, bool>
    {
        protected override bool CanExecuteInternal(bool parameter)
        {
            return true;
        }

        protected override void ExecuteInternal(bool parameter)
        {
            Rule[] rules = null;


            using (var reader = new JsonTextReader(new StringReader(Resources.DefaultRules)))
            {
                var serializer = Settings.GetSerializer();
                try
                {
                    rules = serializer.Deserialize<Rule[]>(reader);
                    if (rules == null)
                        throw new SerializationException("cant read DefaultRules.json");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "default rules deserialization fault");
                }
            }

            var rulesTuple = rules.Select(r => new SelectingRule() { Rule = r, IsSelected = true }).ToArray();
            if (!parameter)
            {
                var window = new SelectRulesWindow(rulesTuple);
                var dialogResult = window.ShowDialog();
                if (!dialogResult.GetValueOrDefault())
                    return;
            }

            foreach (var rule in rulesTuple.Where(r => r.IsSelected))
            {
                var oldRule = Settings.Instance.Compatibility.Rules.FirstOrDefault(r => r.Name == rule.Rule.Name);
                int index = oldRule == null ? 0 : Settings.Instance.Compatibility.Rules.IndexOf(oldRule);
                if (oldRule != null)
                    Settings.Instance.Compatibility.Rules.Remove(oldRule);
                Settings.Instance.Compatibility.Rules.Insert(index, rule.Rule);
            }
        }

        public class SelectingRule
        {
            public Rule Rule { get; set; }
            public bool IsSelected { get; set; }
        }
    }
}
