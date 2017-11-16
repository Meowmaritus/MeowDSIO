using System.Windows.Controls;
using System.Windows.Media;

namespace MeowTaeEditorGui
{
    public static class ControlExtensions
    {
        public static void SeSetEnabled(this Control ctrl, bool enabled)
        {
            //ctrl.IsEnabled = enabled;
            //ctrl.Opacity = enabled ? 1.0 : 0.25;
            ctrl.OpacityMask = enabled ? null : new SolidColorBrush(Color.FromArgb(255 / 4, 255 / 4, 255 / 4, 255 / 4));
            ctrl.IsHitTestVisible = enabled;
        }
    }
}
