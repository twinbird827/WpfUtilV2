﻿using System;
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
        public static readonly DependencyProperty PasswordProperty = BehaviorUtil.RegisterAttached(
            "Password", typeof(PasswordBoxChangedBehavior), default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSetPasswordCallback
        );

        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }

        /// <summary>
        /// Password ﾌﾟﾛﾊﾟﾃｨ変更時ｲﾍﾞﾝﾄ
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void OnSetPasswordCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
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

        /// <summary>
        /// PasswordBox ﾊﾟｽﾜｰﾄﾞ変更時ｲﾍﾞﾝﾄ
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
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
