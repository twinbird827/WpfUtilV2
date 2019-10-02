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
    public class ControlMouseDownBehavior
    {
        /// <summary>
        /// Commandの依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty CommandProperty = BehaviorUtil.RegisterAttached(
            "Command", typeof(ControlMouseDownBehavior), default(ICommand), OnSetCommandCallback
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
            var control = target as Control;

            BehaviorUtil.SetEventHandler(control,
                (fe) => fe.MouseDown += Control_MouseDown,
                (fe) => fe.MouseDown -= Control_MouseDown
            );
        }

        /// <summary>
        /// ﾏｳｽｸﾘｯｸ時の処理
        /// </summary>
        /// <param name="sender">送り先</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void Control_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Control control = sender as Control;
            if (control != null)
            {
                ICommand command = (ICommand)control.GetValue(CommandProperty);
                if (command != null)
                {
                    command.Execute(e);
                    e.Handled = true;
                }
            }
        }
    }
}
