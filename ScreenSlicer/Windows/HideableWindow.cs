using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace ScreenSlicer.Windows
{
    public abstract class HideableWindow : Window
    {
        protected override void OnClosing(CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
            base.OnClosing(e);
        }
    }
}
