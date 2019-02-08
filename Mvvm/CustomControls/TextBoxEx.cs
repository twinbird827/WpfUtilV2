using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfUtilV2.Mvvm.Behaviors;

namespace WpfUtilV2.Mvvm.CustomControls
{
    public class TextBoxEx : TextBox
    {
        public TextBoxEx() : base()
        {
            Action<object, RoutedEventArgs> loaded = null;
            Action<object, RoutedEventArgs> unloaded = null;

            // ｴﾚﾒﾝﾄのﾛｰﾄﾞ処理を定義(ｲﾍﾞﾝﾄ追加)
            loaded = (sender, e) =>
            {
                var fe = sender as TextBoxEx;

                if (loaded != null)
                {
                    fe.Loaded -= new RoutedEventHandler(loaded);
                    loaded = null;
                }

                // ｲﾍﾞﾝﾄ追加処理
                fe.GotFocus += TextBox_GotFocus;
                fe.PreviewMouseLeftButtonDown += TextBox_PreviewMouseLeftButtonDown;

                // ｱﾝﾛｰﾄﾞｲﾍﾞﾝﾄ追加
                fe.Unloaded += new RoutedEventHandler(unloaded);
            };

            // ｴﾚﾒﾝﾄのｱﾝﾛｰﾄﾞ処理を定義(ｱﾝﾛｰﾄﾞ時にｲﾍﾞﾝﾄを削除する)
            unloaded = (sender, e) =>
            {
                var fe = sender as TextBoxEx;

                if (unloaded != null)
                {
                    fe.Unloaded -= new RoutedEventHandler(unloaded);
                    unloaded = null;
                }

                // ｱﾝﾛｰﾄﾞ時にｲﾍﾞﾝﾄ削除処理
                fe.GotFocus -= TextBox_GotFocus;
                fe.PreviewMouseLeftButtonDown -= TextBox_PreviewMouseLeftButtonDown;
            };

            // ﾛｰﾄﾞｲﾍﾞﾝﾄ追加orﾛｰﾄﾞ済みの場合は直接実行
            if (this.IsLoaded)
            {
                loaded(this, new RoutedEventArgs());
            }
            else
            {
                this.Loaded += new RoutedEventHandler(loaded);
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SelectAll();
        }

        private void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsFocused)
            {
                Focus();
                e.Handled = true;
            }
        }
    }
}
