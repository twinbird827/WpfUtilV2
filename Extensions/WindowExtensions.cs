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
        /// <summary>
        /// ﾀﾞｲｱﾛｸﾞを表示します。
        /// </summary>
        /// <param name="window">ﾀﾞｲｱﾛｸﾞ</param>
        /// <returns></returns>
        public static bool ShowModalWindow(this Window window)
        {
            return window.ShowModalWindow(Mouse.PrimaryDevice.GetPosition(Application.Current.MainWindow));
        }

        /// <summary>
        /// 指定した位置にﾀﾞｲｱﾛｸﾞを表示します。
        /// </summary>
        /// <param name="window">ﾀﾞｲｱﾛｸﾞ</param>
        /// <param name="position">ﾀﾞｲｱﾛｸﾞを表示する位置</param>
        /// <returns></returns>
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

        /// <summary>
        /// ﾀﾞｲｱﾛｸﾞを表示します。
        /// </summary>
        /// <param name="window">ﾀﾞｲｱﾛｸﾞ</param>
        /// <param name="e">ﾀﾞｲｱﾛｸﾞ表示時のｶｰｿﾙ位置情報</param>
        /// <returns></returns>
        public static bool ShowModalWindow(this Window window, MouseButtonEventArgs e)
        {
            return window.ShowModalWindow(e.GetPosition(Window.GetWindow(e.Source as DependencyObject)));
        }

        /// <summary>
        /// ﾌﾟﾛｸﾞﾚｽ用のｳｨﾝﾄﾞｳを表示します。
        /// </summary>
        /// <param name="window">ﾌﾟﾛｸﾞﾚｽ用ｳｨﾝﾄﾞｳ</param>
        /// <returns></returns>
        public static bool ShowProgressWindow(this Window window)
        {
            try
            {
                var owner = Application.Current.MainWindow;

                window.Owner = owner;
                window.Width = owner.ActualWidth - 2;

                return (bool)window.ShowDialog();
            }
            finally
            {
                window.Close();
            }
        }
    }
}
