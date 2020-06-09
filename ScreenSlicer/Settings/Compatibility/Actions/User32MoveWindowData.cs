using ScreenSlicer.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSlicer.Compatibility.Actions
{
    public class User32MoveWindowData : IActionData
    {
        public bool ShouldRepaint { get; set; }
    }
}
