using ScreenSlicer.Compatibility;
using ScreenSlicer.Compatibility.Actions;
using ScreenSlicer.Native.Windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace ScreenSlicer.Native.Compatibility
{
    public static class Action
    {
        static Dictionary<Type, MethodInfo> _methods;

        static Action()
        {
            _methods = typeof(Action).GetMethods(BindingFlags.Public | BindingFlags.Static).Where(m =>
            {
                if (m.Name != nameof(Apply)) return false;
                var parameters = m.GetParameters();
                if (parameters.Length != 3) return false;
                if (!typeof(IActionData).IsAssignableFrom(parameters[0].ParameterType)) return false;
                if (parameters[1].ParameterType != typeof(ISystemWindow)) return false;
                if (parameters[2].ParameterType != typeof(Rectangle).MakeByRefType()) return false;
                if (!parameters[2].ParameterType.IsByRef) return false;
                return true;
            }).ToDictionary(m => m.GetParameters()[0].ParameterType);
        }

        public static void Apply(this IActionData data, ISystemWindow window, ref Rectangle region)
        {
            var type = data.GetType();
            if (_methods.TryGetValue(type, out var method))
            {
                var parameters = new object[] { data, window, region };
                method.Invoke(null, parameters);
                region = (Rectangle)parameters[2];
            }
            else
                throw new ArgumentException($"Not found suitable method for '{type}' type");
        }


        public static void Apply(this CorrectTargetRegionData data, ISystemWindow window, ref Rectangle region)
        {
            var expand = Size.Subtract(window.Position.Size, window.ClientRectangle.Size);
            var offset = new Point(-expand.Width / 2, 0);
            region.Offset(offset);
            region.Size = Size.Add(region.Size, new Size(expand.Width, 0));
        }

        public static void Apply(this ModifyTargetRegionData data, ISystemWindow window, ref Rectangle region)
        {
            region.Offset(new Point(data.Left, data.Top));
            region.Size = Size.Add(region.Size, new Size(data.Right - data.Left, data.Bottom - data.Top));
        }

        public static void Apply(this User32MoveWindowData data, ISystemWindow window, ref Rectangle region)
        {
            Methods.MoveWindow(window.Handle, region.Left, region.Top, region.Width, region.Height, data.ShouldRepaint);
        }

        public static void Apply(this User32PostMessageData data, ISystemWindow window, ref Rectangle region)
        {
            Methods.PostMessage(window.Handle, data.Message, default, default);
        }

        public static void Apply(this User32SetWindowPosData data, ISystemWindow window, ref Rectangle region)
        {
            Methods.SetWindowPos(window.Handle, default, region.Left, region.Top, region.Width, region.Height, data.Flags);
        }

        public static void Apply(this User32ShowWindowData data, ISystemWindow window, ref Rectangle region)
        {
            Methods.ShowWindow(window.Handle, data.Command);
        }
    }
}
