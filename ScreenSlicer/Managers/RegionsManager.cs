using ScreenSlicer.Commands;
using ScreenSlicer.Native.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ScreenSlicer.Managers
{
    public class RegionsManager
    {
        public ICommand ExitCommand { get; }
        public ICommand EndSliceRegionsCommand { get; }

        public bool IsActive { get; private set; }

        private ScreenRegion[] _regions;
        private Windows.SlicingWindow[] _windows;

        public RegionsManager(ExitCommand exitCommand, EndSliceRegionsCommand endSliceRegionsCommand)
        {
            ExitCommand = exitCommand;
            EndSliceRegionsCommand = endSliceRegionsCommand;
            endSliceRegionsCommand.SetManager(this);
            SyncCurrentPresetWithScreens();
        }

        public void BeginSlice()
        {
            SyncCurrentPresetWithScreens();
            ShowSlicingWindows();
        }

        public void EndSlice(string name)
        {
            Settings.Instance.Regions.CurrentPreset.ScreenRegions = _regions;
            foreach (var window in _windows)
                window.Close();
        }

        private void ShowSlicingWindows()
        {
            _regions = Settings.Instance.Regions.CurrentPreset.ScreenRegions.Select(r => r.Clone() as ScreenRegion).ToArray();
            _windows = _regions.Select(region =>
            {
                var window = new Windows.SlicingWindow(region, EndSliceRegionsCommand);
                window.Show();
                region.Hook();
                return window;
            }).ToArray();
        }

        private void SyncCurrentPresetWithScreens()
        {
            var preset = Settings.Instance.Regions.CurrentPreset;
            if (preset == null)
                preset = GetOrCreateDefaultPreset();
            else if (!IsPresetCorrespondsToScreens(preset, Screen.GetScreens()))
            {
                if (MessageBox.Show(
                    "Current preset is not corresponds to available screens configuration. It will be replaced with empty preset.\r\n\r\nPress 'Cancel' for close app without any changes",
                    "Preset inconsistency",
                    MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                    ExitCommand.Execute(0);
                preset = GetOrCreateDefaultPreset();
            }
        }

        private bool IsPresetCorrespondsToScreens(RegionsPreset preset, Screen[] screens)
        {
            if (preset.ScreenRegions.Length != screens.Length)
                return false;
            var sortedRegions = preset.ScreenRegions.OrderBy(r => r.PhysicalBounds.Top).ThenBy(r => r.PhysicalBounds.Left).ToArray();
            var sortedScreens = screens.OrderBy(s => s.Rect.Top).ThenBy(s => s.Rect.Left).ToArray();

            for (var i = 0; i < sortedRegions.Length; i++)
            {
                var region = sortedRegions[i];
                var screen = sortedScreens[i];
                if (region.PhysicalBounds != screen.Rect)
                    return false;
                region.IsPrimary = screen.IsPrimary;
            }
            return true;
        }

        private RegionsPreset GetOrCreateDefaultPreset()
        {
            var preset = Settings.Instance.Regions.GetPresetByName(string.Empty);
            if (preset != null)
            {
                if (!IsPresetCorrespondsToScreens(preset, Screen.GetScreens()))
                {
                    Settings.Instance.Regions.Presets.Remove(preset);
                }
                Settings.Instance.Regions.CurrentPreset = preset;
            }
            else
            {
                preset = InitializeNewPreset(string.Empty);
                Settings.Instance.Regions.CurrentPreset = preset;
            }

            return preset;
        }

        private RegionsPreset InitializeNewPreset(string name)
        {
            var preset = Settings.Instance.Regions.GetPresetByName(name);
            if (preset != null)
                Settings.Instance.Regions.Presets.Remove(preset);
            preset = new RegionsPreset(name, Screen.GetScreens().Select(s => new ScreenRegion(s.WorkspaceRect, s.Rect, s.IsPrimary)).ToArray());
            Settings.Instance.Regions.Presets.Add(preset);
            return preset;
        }

    }
}
