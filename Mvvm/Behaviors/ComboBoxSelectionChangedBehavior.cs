﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfUtilV2.Mvvm.Behaviors
{
    /// <summary>
    /// ｺﾝﾎﾞﾎﾞｯｸｽの選択行変更時に実行したいｲﾍﾞﾝﾄを定義できる添付ﾋﾞﾍｲﾋﾞｱ
    /// </summary>
    public class ComboBoxSelectionChangedBehavior
    {
        /// <summary>
        /// Commandの依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty CommandProperty = BehaviorUtil.RegisterAttached(
            "Command", typeof(ComboBoxSelectionChangedBehavior), default(ICommand), OnSetCommandCallback
        );

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
        private static void OnSetCommandCallback(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            ComboBox control = target as ComboBox;

            BehaviorUtil.SetEventHandler(control,
                (fe) => fe.SelectionChanged += ComboBox_SelectionChanged,
                (fe) => fe.SelectionChanged -= ComboBox_SelectionChanged
            );
        }

        /// <summary>
        /// 選択行変更時の処理
        /// </summary>
        /// <param name="sender">送り先</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox control = sender as ComboBox;
            if (control != null)
            {
                ICommand command = (ICommand)control.GetValue(CommandProperty);
                if (command.CanExecute(e)) command.Execute(e);
            }
        }
    }
}
