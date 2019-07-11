using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfUtilV2.Mvvm.Behaviors
{
    /// <summary>
    /// UserControlのLoadedｲﾍﾞﾝﾄで任意のｺﾏﾝﾄﾞを実行するためのﾋﾞﾍｲﾋﾞｱです。
    /// </summary>
    public class FrameworkElementLoadedBehavior
    {
        /// <summary>
        /// ｺﾏﾝﾄﾞの依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(FrameworkElementLoadedBehavior), new UIPropertyMetadata(OnSetCommandCallback));

        /// <summary>
        /// ｺﾏﾝﾄﾞを設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="value">IDisposable</param>
        public static void SetCommand(DependencyObject target, object value)
        {
            target.SetValue(CommandProperty, value);
        }

        /// <summary>
        /// ｺﾏﾝﾄﾞを取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <returns>IDisposable</returns>
        public static ICommand GetCommand(DependencyObject target)
        {
            return (ICommand)target.GetValue(CommandProperty);
        }

        /// <summary>
        /// ｺﾏﾝﾄﾞﾌﾟﾛﾊﾟﾃｨの依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(FrameworkElementLoadedBehavior), new PropertyMetadata());

        /// <summary>
        /// ｺﾏﾝﾄﾞﾌﾟﾛﾊﾟﾃｨを設定します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="value">IDisposable</param>
        public static void SetCommandParameter(DependencyObject target, object value)
        {
            target.SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        /// ｺﾏﾝﾄﾞﾌﾟﾛﾊﾟﾃｨを取得します（添付ﾋﾞﾍｲﾋﾞｱ）
        /// </summary>
        /// <param name="target">対象</param>
        /// <returns>IDisposable</returns>
        public static object GetCommandParameter(DependencyObject target)
        {
            return target.GetValue(CommandParameterProperty);
        }

        /// <summary>
        /// ｺﾏﾝﾄﾞﾌﾟﾛﾊﾟﾃｨが変更された際の処理
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void OnSetCommandCallback(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var obj = target as DependencyObject;

            if (obj != null)
            {
                BehaviorUtil.Loaded(target as UserControl, UserControl_Loaded);
            }

            //if (uc.IsLoaded)
            //{
            //    UserControl_Loaded(uc, new EventArgs());
            //}
            //else
            //{
            //    Action<object, RoutedEventArgs> unloaded = null;
            //    // ﾕｰｻﾞｺﾝﾄﾛｰﾙのｱﾝﾛｰﾄﾞ処理を定義(ｱﾝﾛｰﾄﾞ時にｲﾍﾞﾝﾄを削除する)
            //    unloaded = (sender, ea) =>
            //    {
            //        var inner = sender as UserControl;

            //        if (unloaded != null)
            //        {
            //            inner.Unloaded -= new RoutedEventHandler(unloaded);
            //            unloaded = null;
            //        }

            //        // ｱﾝﾛｰﾄﾞ時にｲﾍﾞﾝﾄ削除処理
            //        inner.Loaded -= UserControl_Loaded;
            //    };

            //    uc.Loaded += UserControl_Loaded;
            //    uc.Unloaded += new RoutedEventHandler(unloaded);
            //}
            //BehaviorUtil.SetEventHandler(uc,
            //    (fe) => UserControl_Loaded(fe, new EventArgs()),
            //    (fe) => { }
            //);
        }

        /// <summary>
        /// ﾕｰｻﾞｺﾝﾄﾛｰﾙ初期化の処理
        /// </summary>
        /// <param name="sender">送り先</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void UserControl_Loaded(object sender, EventArgs e)
        {
            var obj = sender as DependencyObject;

            if (obj == null)
            {
                return;
            }

            var command = GetCommand(obj);
            var commandparameter = GetCommandParameter(obj);

            if (command != null && command.CanExecute(commandparameter))
            {
                command.Execute(commandparameter);
            }
        }
    }
}
