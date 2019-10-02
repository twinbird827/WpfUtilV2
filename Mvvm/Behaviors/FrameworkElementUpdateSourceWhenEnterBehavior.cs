using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace WpfUtilV2.Mvvm.Behaviors
{
    /// <summary>
    /// 対象ｴﾚﾒﾝﾄでEnterｷｰ押下時に指定したDependencyPropertyのUpdateSourceを実行するための添付ﾋﾞﾍｲﾋﾞｱ
    /// </summary>
    public static class FrameworkElementUpdateSourceWhenEnterBehavior
    {
        public static readonly DependencyProperty TargetProperty = BehaviorUtil.RegisterAttached(
            "Target", typeof(FrameworkElementUpdateSourceWhenEnterBehavior), default(DependencyProperty), OnSetTargetCallback
        );

        public static void SetTarget(DependencyObject dp, DependencyProperty value)
        {
            dp.SetValue(TargetProperty, value);
        }

        public static DependencyProperty GetTarget(DependencyObject dp)
        {
            return (DependencyProperty)dp.GetValue(TargetProperty);
        }

        /// <summary>
        /// Targetﾌﾟﾛﾊﾟﾃｨ変更時ｲﾍﾞﾝﾄ (ｲﾍﾞﾝﾄを設定します)
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void OnSetTargetCallback(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = dp as FrameworkElement;
            
            BehaviorUtil.SetEventHandler(element,
                (fe) => fe.PreviewKeyDown += FrameworkElement_PreviewKeyDown,
                (fe) => fe.PreviewKeyDown -= FrameworkElement_PreviewKeyDown
            );
        }

        /// <summary>
        /// ｴﾚﾒﾝﾄ PreviewKeyDown ｲﾍﾞﾝﾄ (Binding.UpdateSourceを実行します。)
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="e">ｲﾍﾞﾝﾄ情報</param>
        private static void FrameworkElement_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                // Enterｷｰ以外は中断
                return;
            }

            var element = sender as FrameworkElement;

            if (element == null)
            {
                // 対応するｴﾚﾒﾝﾄが許容する型ではない場合は中断
                return;
            }

            DependencyProperty property = GetTarget(element);

            if (property == null)
            {
                // 指定したﾌﾟﾛﾊﾟﾃｨが存在しない場合は中断
                return;
            }

            BindingExpression binding = BindingOperations.GetBindingExpression(element, property);

            if (binding != null)
            {
                // 指定したﾌﾟﾛﾊﾟﾃｨにﾊﾞｲﾝﾃﾞｨﾝｸﾞが設定されている場合に実行
                binding.UpdateSource();
            }
        }
    }
}
