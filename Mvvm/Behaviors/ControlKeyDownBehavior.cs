using System;
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
    /// ControlのKeyDownｲﾍﾞﾝﾄでCommandを実行するためのﾋﾞﾍｲﾋﾞｱです。
    /// Command引数にはKeyDownEventArgsが渡されます。
    /// </summary>
    public class ControlKeyDownBehavior
    {
        /// <summary>
        /// Commandの依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(ControlKeyDownBehavior), new UIPropertyMetadata(OnChangeCommand));

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
        private static void OnChangeCommand(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            Control control = target as Control;
            if (control != null)
            {
                if (e.OldValue == null && e.NewValue != null)
                {
                    control.KeyDown += OnKeyDown;
                }
                else if (e.OldValue != null && e.NewValue == null)
                {
                    control.KeyDown -= OnKeyDown;
                }
            }
        }

        /// <summary>
        /// ｷｰﾀﾞｳﾝ時の処理
        /// </summary>
        /// <param name="sender">送り先</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void OnKeyDown(object sender, KeyEventArgs e)
        {
            Control control = sender as Control;
            if (control != null)
            {
                ICommand command = (ICommand)control.GetValue(CommandProperty);
                if (command.CanExecute(e)) command.Execute(e);
            }
        }
    }
}
