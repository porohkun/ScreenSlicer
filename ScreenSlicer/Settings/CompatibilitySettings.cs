using Newtonsoft.Json;
using ScreenSlicer.Compatibility;
using ScreenSlicer.Native.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WPFLocalizeExtension.Engine;

namespace ScreenSlicer
{
    public class CompatibilitySettings : SettingsPartWithNotifier
    {
        [JsonProperty]
        public ObservableCollection<Rule> Rules { get; private set; } = new ObservableCollection<Rule>() { };

        public CompatibilitySettings()
        {

        }

        public Rule GetRuleForWindow(ISystemWindow window)
        {
            return Rules.FirstOrDefault((rule) => rule.Conditions.All(c => c.Check(window)));
        }
    }
}
