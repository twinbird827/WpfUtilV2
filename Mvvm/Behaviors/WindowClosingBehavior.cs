using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WpfUtilV2.Mvvm.Behaviors
{
    public class WindowClosingBehavior
    {
        /// <summary>
        /// Commandの依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty CommandProperty = BehaviorUtil.RegisterAttached(
            "Command", typeof(WindowClosingBehavior), default(ICommand), OnSetCommandCallback
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
            var win = target as Window;

            BehaviorUtil.SetEventHandler(win,
                (fe) => fe.Closing += Window_Closing,
                (fe) => fe.Closing -= Window_Closing
            );
        }

        /// <summary>
        /// ｳｨﾝﾄﾞｳを閉じた時の処理
        /// </summary>
        /// <param name="sender">送り先</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void Window_Closing(object sender, CancelEventArgs e)
        {
            var win = sender as Window;

            if (win == null)
            {
                return;
            }

            var command = GetCommand(win);

            if (command != null)
            {
                command.Execute(e);
            }
        }

    }
}
