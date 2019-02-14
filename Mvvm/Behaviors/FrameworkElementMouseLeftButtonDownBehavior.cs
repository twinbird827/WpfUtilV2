﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WpfUtilV2.Mvvm.Behaviors
{
    public class FrameworkElementMouseLeftButtonDownBehavior
    {
        /// <summary>
        /// Commandの依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(FrameworkElementMouseLeftButtonDownBehavior), new UIPropertyMetadata(CommandProperty_Changed));

        /// <summary>
        /// ｺﾏﾝﾄﾞを設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="value">コマンド</param>
        public static void SetCommand(DependencyObject target, object value)
        {
            target.SetValue(CommandProperty, value);
        }

        /// <summary>
        /// ｺﾏﾝﾄﾞを取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <returns>ｺﾏﾝﾄﾞ</returns>
        public static ICommand GetCommand(DependencyObject target)
        {
            return (ICommand)target.GetValue(CommandProperty);
        }

        /// <summary>
        /// Commandﾌﾟﾛﾊﾟﾃｨが変更された際の処理
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void CommandProperty_Changed(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var control = target as FrameworkElement;

            BehaviorUtil.SetEventHandler(control,
                (fe) => fe.MouseLeftButtonDown += Control_MouseLeftButtonDown,
                (fe) => fe.MouseLeftButtonDown -= Control_MouseLeftButtonDown
            );
        }

        /// <summary>
        /// ﾏｳｽﾀﾞﾌﾞﾙｸﾘｯｸ時の処理
        /// </summary>
        /// <param name="sender">送り先</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void Control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var control = sender as FrameworkElement;
            if (control != null)
            {
                ICommand command = (ICommand)control.GetValue(CommandProperty);
                if (command != null) command.Execute(e);
            }
        }
    }
}