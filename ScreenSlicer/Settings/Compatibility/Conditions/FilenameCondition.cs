﻿using ScreenSlicer.Native.Windows;
using System.IO;

namespace ScreenSlicer.Compatibility
{
    public class FilenameCondition : StringCondition
    {
        private bool _fullPath;

        public bool FullPath
        {
            get => _fullPath;
            set
            {
                if (_fullPath != value)
                {
                    _fullPath = value;
                    NotifyPropertyChanged(nameof(FullPath));
                }
            }
        }

        public override object Clone()
        {
            return new FilenameCondition()
            {
                FullPath = FullPath,
                RegularExpression = RegularExpression,
                TargetValue = TargetValue
            };
        }

        protected override string GetValue(ISystemWindow window)
        {
            return FullPath ? window.ProcessName : Path.GetFileName(window.ProcessName);
        }
    }
}
