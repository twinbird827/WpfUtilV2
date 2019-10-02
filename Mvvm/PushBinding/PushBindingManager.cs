using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfUtilV2.Mvvm.Behaviors;

namespace WpfUtilV2.Mvvm.PushBinding
{
    public class PushBindingManager
    {
        public static DependencyProperty PushBindingsProperty = BehaviorUtil.RegisterAttached(
            "PushBindingsInternal", typeof(PushBindingManager), default(PushBindingCollection)
        );

        public static PushBindingCollection GetPushBindings(DependencyObject obj)
        {
            if (obj.GetValue(PushBindingsProperty) == null)
            {
                obj.SetValue(PushBindingsProperty, new PushBindingCollection(obj));
            }
            return (PushBindingCollection)obj.GetValue(PushBindingsProperty);
        }
        public static void SetPushBindings(DependencyObject obj, PushBindingCollection value)
        {
            obj.SetValue(PushBindingsProperty, value);
        }


        public static DependencyProperty StylePushBindingsProperty = BehaviorUtil.RegisterAttached(
            "StylePushBindings", typeof(PushBindingManager), default(PushBindingCollection), StylePushBindingsChanged
        );

        public static PushBindingCollection GetStylePushBindings(DependencyObject obj)
        {
            return (PushBindingCollection)obj.GetValue(StylePushBindingsProperty);
        }
        public static void SetStylePushBindings(DependencyObject obj, PushBindingCollection value)
        {
            obj.SetValue(StylePushBindingsProperty, value);
        }

        public static void StylePushBindingsChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target != null)
            {
                PushBindingCollection stylePushBindings = e.NewValue as PushBindingCollection;
                PushBindingCollection pushBindingCollection = GetPushBindings(target);
                foreach (PushBinding pushBinding in stylePushBindings)
                {
                    PushBinding pushBindingClone = pushBinding.Clone() as PushBinding;
                    pushBindingCollection.Add(pushBindingClone);
                }
            }
        }
    }
}
