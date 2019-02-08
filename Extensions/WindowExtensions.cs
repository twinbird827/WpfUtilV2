using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WpfUtilV2.Extensions
{
    public static class WindowExtensions
    {
        public static bool ShowDialog(this Window window, Point position)
        {
            window.Top = position.Y;
            window.Left = position.X;
            return (bool)window.ShowDialog();
        }

        public static bool ShowDialog(this Window window, MouseButtonEventArgs e)
        {
            return window.ShowDialog(e.GetPosition(Window.GetWindow(e.Source as DependencyObject)));
        }
    }
}
