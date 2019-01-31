using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfUtilV2.Mvvm.Behaviors
{
    /// <summary>
    /// PasswordBoxｺﾝﾄﾛｰﾙのPasswordﾌﾟﾛﾊﾟﾃｨを処理します。
    /// </summary>
    public class PasswordBoxChangedBehavior
    {
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached(
            "Password",
            typeof(string),
            typeof(PasswordBoxChangedBehavior),
            new FrameworkPropertyMetadata(
                default(string), 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                PasswordProperty_Changed)
        );

        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }

        private static void PasswordProperty_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var passwordbox = sender as PasswordBox;

            if (passwordbox == null)
            {
                return;
            }

            BehaviorUtil.SetEventHandler(passwordbox,
                (fe) => fe.PasswordChanged += PasswordBox_PasswordChanged,
                (fe) => fe.PasswordChanged -= PasswordBox_PasswordChanged
            );

            var newv = e.NewValue as string;
            var oldv = passwordbox.Password;

            if (newv != oldv)
            {
                // 新旧で異なる値ならﾊﾟｽﾜｰﾄﾞﾎﾞｯｸｽに通知
                passwordbox.Password = newv;
            }

        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordbox = sender as PasswordBox;

            if (passwordbox == null)
            {
                return;
            }

            var newv = passwordbox.Password;
            var oldv = GetPassword(passwordbox);

            if (newv != oldv)
            {
                // 新旧で異なる値ならﾌﾟﾛﾊﾟﾃｨに通知
                SetPassword(passwordbox, newv);
            }
        }
    }

}
