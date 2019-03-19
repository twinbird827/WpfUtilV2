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
        public static bool ShowModalWindow(this Window window)
        {
            return window.ShowModalWindow(Mouse.PrimaryDevice.GetPosition(Application.Current.MainWindow));
        }

        public static bool ShowModalWindow(this Window window, Point position)
        {
            try
            {
                var owner = Application.Current.MainWindow;
                var ot = owner.WindowState == WindowState.Maximized ? 0 : owner.Top;
                var ol = owner.WindowState == WindowState.Maximized ? 0 : owner.Left;

                window.Owner = owner;
                window.Top = ot + position.Y;
                window.Left = ol + position.X;

                return (bool)window.ShowDialog();
            }
            finally
            {
                window.Close();
            }
        }

        public static bool ShowModalWindow(this Window window, MouseButtonEventArgs e)
        {
            return window.ShowModalWindow(e.GetPosition(Window.GetWindow(e.Source as DependencyObject)));
        }
    }
}
