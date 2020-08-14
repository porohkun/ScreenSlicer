using ScreenSlicer.Native.Windows;
using System;
using System.Diagnostics;

namespace ScreenSlicer.Compatibility
{
    public class FileInfoPropertyCondition : StringCondition
    {
        private FileVersionInfoProperties _property;

        public FileVersionInfoProperties Property
        {
            get => _property;
            set
            {
                if (_property != value)
                {
                    _property = value;
                    NotifyPropertyChanged(nameof(Property));
                }
            }
        }

        public override object Clone()
        {
            return new FileInfoPropertyCondition()
            {
                Property = Property,
                RegularExpression = RegularExpression,
                TargetValue = TargetValue
            };
        }

        protected override string GetValue(ISystemWindow window)
        {
            var info = FileVersionInfo.GetVersionInfo(window.ProcessName);
            return typeof(FileVersionInfo).GetProperty(Property.ToString())?.GetValue(info) as string;
        }
    }

    public enum FileVersionInfoProperties
    {
        ProductName,
        PrivateBuild,
        OriginalFilename,
        LegalTrademarks,
        LegalCopyright,
        Language,
        //TODO: make product and file version diapason
        ProductVersion,
        FileVersion,
        SpecialBuild,
        InternalName,
        FileName,
        FileDescription,
        CompanyName,
        Comments
    }
}
