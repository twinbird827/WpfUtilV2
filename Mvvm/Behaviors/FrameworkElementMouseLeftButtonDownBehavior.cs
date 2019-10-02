using System;
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
        /// DoubleClickの依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty DoubleClickProperty = BehaviorUtil.RegisterAttached(
            "DoubleClick", typeof(FrameworkElementMouseLeftButtonDownBehavior), default(ICommand), OnSetCommandCallback
        );

        /// <summary>
        /// ｺﾏﾝﾄﾞを設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="value">コマンド</param>
        public static void SetDoubleClick(DependencyObject target, object value)
        {
            target.SetValue(DoubleClickProperty, value);
        }

        /// <summary>
        /// ｺﾏﾝﾄﾞを取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <returns>ｺﾏﾝﾄﾞ</returns>
        public static ICommand GetDoubleClick(DependencyObject target)
        {
            return (ICommand)target.GetValue(DoubleClickProperty);
        }

        /// <summary>
        /// SingleClickの依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty SingleClickProperty = BehaviorUtil.RegisterAttached(
            "SingleClick", typeof(FrameworkElementMouseLeftButtonDownBehavior), default(ICommand), OnSetCommandCallback
        );

        /// <summary>
        /// ｺﾏﾝﾄﾞを設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="value">コマンド</param>
        public static void SetSingleClick(DependencyObject target, object value)
        {
            target.SetValue(SingleClickProperty, value);
        }

        /// <summary>
        /// ｺﾏﾝﾄﾞを取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <returns>ｺﾏﾝﾄﾞ</returns>
        public static ICommand GetSingleClick(DependencyObject target)
        {
            return (ICommand)target.GetValue(SingleClickProperty);
        }

        /// <summary>
        /// Commandﾌﾟﾛﾊﾟﾃｨが変更された際の処理
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void OnSetCommandCallback(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var control = target as FrameworkElement;
            
            BehaviorUtil.SetEventHandler(control,
                (fe) => fe.MouseLeftButtonDown += FrameworkElement_MouseLeftButtonDown,
                (fe) => fe.MouseLeftButtonDown -= FrameworkElement_MouseLeftButtonDown
            );
        }

        /// <summary>
        /// ﾏｳｽﾀﾞﾌﾞﾙｸﾘｯｸ時の処理
        /// </summary>
        /// <param name="sender">送り先</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void FrameworkElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var fe = sender as FrameworkElement;

            if (fe == null) return;

            // ｸﾘｯｸ数によって実行するｺﾏﾝﾄﾞを変更する。
            var command = e.ClickCount == 1
                ? GetSingleClick(fe)
                : e.ClickCount == 2
                ? GetDoubleClick(fe)
                : null;

            if (command == null) return;

            // ｺﾏﾝﾄﾞ実行
            command.Execute(e);

            // 処理済にする。
            e.Handled = true;
        }
    }
}
