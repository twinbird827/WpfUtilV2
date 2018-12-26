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
    /// UserControlのLoadedｲﾍﾞﾝﾄで任意のｺﾏﾝﾄﾞを実行するためのﾋﾞﾍｲﾋﾞｱです。
    /// </summary>
    public class UserControlLoadedBehavior
    {
        /// <summary>
        /// ｺﾏﾝﾄﾞの依存関係ﾌﾟﾛﾊﾟﾃｨ
        /// </summary>
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(UserControlLoadedBehavior), new UIPropertyMetadata(CommandProperty_Changed));

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
            DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(UserControlLoadedBehavior), new PropertyMetadata());

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
        /// Disposableﾌﾟﾛﾊﾟﾃｨが変更された際の処理
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void CommandProperty_Changed(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var uc = target as UserControl;

            BehaviorUtil.SetEventHandler(uc,
                (fe) => fe.Loaded += UserControl_Loaded,
                (fe) => fe.Loaded -= UserControl_Loaded
            );
        }

        /// <summary>
        /// ﾕｰｻﾞｺﾝﾄﾛｰﾙ初期化の処理
        /// </summary>
        /// <param name="sender">送り先</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void UserControl_Loaded(object sender, EventArgs e)
        {
            var uc = sender as UserControl;

            if (uc == null)
            {
                return;
            }

            ICommand command = uc.GetValue(CommandProperty) as ICommand;
            object commandparameter = uc.GetValue(CommandParameterProperty);

            if (command != null && command.CanExecute(commandparameter))
            {
                command.Execute(commandparameter);
            }
        }
    }
}
