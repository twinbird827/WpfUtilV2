using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfUtilV2.Mvvm.Behaviors
{
    class BehaviorUtil
    {
        private static readonly DependencyProperty IsAttachedProperty = 
            DependencyProperty.RegisterAttached("IsAttached", typeof(bool), typeof(BehaviorUtil), new PropertyMetadata(false));

        private static bool GetIsAttached(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsAttachedProperty);
        }

        private static void SetIsAttached(DependencyObject dp, bool value)
        {
            dp.SetValue(IsAttachedProperty, value);
        }

        public static void SetEventHandler<T>(T element, Action<T> add, Action<T> remove) where T: FrameworkElement
        {
            if (element == null)
            {
                return;
            }

            if (GetIsAttached(element))
            {
                // ｱﾀｯﾁ済
                return;
            }

            // ｱﾀｯﾁ完了を保存
            SetIsAttached(element, true);

            // 既存のｲﾍﾞﾝﾄを削除(ｲﾍﾞﾝﾄ登録されていない場合でも例外は発生しないのでとりあえず呼び出す)
            remove(element);

            // ｴﾚﾒﾝﾄのｱﾝﾛｰﾄﾞ処理を定義(ｱﾝﾛｰﾄﾞ時にｲﾍﾞﾝﾄを削除する)
            Action<object, RoutedEventArgs> unloaded = null; unloaded = (sender, e) =>
            {
                var fe = sender as FrameworkElement;

                if (unloaded != null)
                {
                    fe.Unloaded -= new RoutedEventHandler(unloaded);
                    unloaded = null;

                }

                // ｱﾝﾛｰﾄﾞ時にｲﾍﾞﾝﾄ削除処理
                remove((T)fe);
            };

            // ｴﾚﾒﾝﾄのﾛｰﾄﾞ処理を定義(ｲﾍﾞﾝﾄ追加)
            Action<object, RoutedEventArgs> loaded = null; loaded = (sender, e) =>
            {
                var fe = sender as FrameworkElement;

                if (loaded != null)
                {
                    fe.Loaded -= new RoutedEventHandler(loaded);
                    loaded = null;

                }

                // ｲﾍﾞﾝﾄ追加処理
                add((T)fe);

                // ｱﾝﾛｰﾄﾞｲﾍﾞﾝﾄ追加
                fe.Unloaded += new RoutedEventHandler(unloaded);
            };

            // ﾛｰﾄﾞｲﾍﾞﾝﾄ追加orﾛｰﾄﾞ済みの場合は直接実行
            if (element.IsLoaded)
            {
                loaded(element, new RoutedEventArgs());
            }
            else
            {
                element.Loaded += new RoutedEventHandler(loaded);
            }
        }
    }
}
