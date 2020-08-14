using ScreenSlicer.Native.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ScreenSlicer.Compatibility
{
    public abstract class ConditionBase<T> : ICondition
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private T _targetValue;

        public T TargetValue
        {
            get => _targetValue;
            set
            {
                if (!object.ReferenceEquals(value, _targetValue))
                {
                    _targetValue = value;
                    NotifyPropertyChanged(nameof(TargetValue));
                }
            }
        }

        public abstract bool Check(ISystemWindow window);

        public abstract object Clone();

        protected abstract T GetValue(ISystemWindow window);
    }

    public sealed class ConditionBase
    {
        public static IEnumerable<ICondition> GetAll()
        {
            var iConditionType = typeof(ICondition);
            return typeof(ConditionBase).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && iConditionType.IsAssignableFrom(t))
                .Select(t => Activator.CreateInstance(t) as ICondition);
        }
    }
}
