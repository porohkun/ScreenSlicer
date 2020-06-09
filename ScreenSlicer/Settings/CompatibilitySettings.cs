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
    [Serializable]
    public class CompatibilitySettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [XmlArray]
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
