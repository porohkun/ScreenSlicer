using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ScreenSlicer.Compatibility.Actions
{
    public abstract class ActionDataBase : IActionData
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static IEnumerable<IActionData> GetAll()
        {
            var iActionDataType = typeof(IActionData);
            return typeof(ActionDataBase).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && iActionDataType.IsAssignableFrom(t))
                .Select(t => Activator.CreateInstance(t) as IActionData);
        }
    }
}
