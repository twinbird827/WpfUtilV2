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
    public static class FrameworkElementUpdateSourceWhenEnterBehavior
    {
        public static readonly DependencyProperty TargetProperty = DependencyProperty.RegisterAttached(
            "Target", typeof(DependencyProperty), typeof(FrameworkElementUpdateSourceWhenEnterBehavior), new UIPropertyMetadata(TargetProperty_Changed));

        public static void SetTarget(DependencyObject dp, DependencyProperty value)
        {
            dp.SetValue(TargetProperty, value);
        }

        public static DependencyProperty GetTarget(DependencyObject dp)
        {
            return (DependencyProperty)dp.GetValue(TargetProperty);
        }

        private static void TargetProperty_Changed(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = dp as FrameworkElement;

            BehaviorUtil.SetEventHandler(element,
                (fe) => fe.PreviewKeyDown += FrameworkElement_PreviewKeyDown,
                (fe) => fe.PreviewKeyDown -= FrameworkElement_PreviewKeyDown
            );
        }

        private static void FrameworkElement_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            //var element = e.Source as FrameworkElement;
            var element = sender as FrameworkElement;

            if (element == null)
            {
                return;
            }

            DependencyProperty property = GetTarget(element);

            if (property == null)
            {
                return;
            }

            BindingExpression binding = BindingOperations.GetBindingExpression(element, property);

            if (binding != null)
            {
                binding.UpdateSource();
            }
        }
    }
}
