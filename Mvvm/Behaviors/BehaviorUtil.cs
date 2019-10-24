using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfUtilV2.Mvvm.Behaviors
{
    class BehaviorUtil
    {
        private static readonly DependencyProperty IsAttachedProperty = 
            RegisterAttached("IsAttached", typeof(BehaviorUtil), false);

        private static bool GetIsAttached(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsAttachedProperty);
        }

        private static void SetIsAttached(DependencyObject dp, bool value)
        {
            dp.SetValue(IsAttachedProperty, value);
        }

        /// <summary>
        /// 添付ﾌﾟﾛﾊﾟﾃｨを登録します。
        /// </summary>
        /// <typeparam name="T">添付ﾌﾟﾛﾊﾟﾃｨのﾃﾞｰﾀ型</typeparam>
        /// <param name="name">名前</param>
        /// <param name="owner">添付ﾌﾟﾛﾊﾟﾃｨを保持するｸﾗｽの型</param>
        /// <param name="defaultValue">ﾃﾞﾌｫﾙﾄ値</param>
        /// <param name="callback">値が変更された際に呼ばれる処理</param>
        /// <param name="bindingOption">添付ﾌﾟﾛﾊﾟﾃｨのﾊﾞｲﾝﾃﾞｨﾝｸﾞ方法</param>
        /// <returns></returns>
        public static DependencyProperty RegisterAttached<T>(
                    string name, Type owner, T defaultValue, PropertyChangedCallback callback = null, FrameworkPropertyMetadataOptions bindingOption = FrameworkPropertyMetadataOptions.None
            )
        {
            return DependencyProperty.RegisterAttached(
                name, typeof(T), owner, new FrameworkPropertyMetadata(defaultValue, bindingOption, callback)
            );
        }

        /// <summary>
        /// 添付ﾌﾟﾛﾊﾟﾃｨのﾊﾞｲﾝﾃﾞｨﾝｸﾞ方法を明示して添付ﾌﾟﾛﾊﾟﾃｨを登録します。
        /// </summary>
        /// <typeparam name="T">添付ﾌﾟﾛﾊﾟﾃｨのﾃﾞｰﾀ型</typeparam>
        /// <param name="name">名前</param>
        /// <param name="owner">添付ﾌﾟﾛﾊﾟﾃｨを保持するｸﾗｽの型</param>
        /// <param name="defaultValue">ﾃﾞﾌｫﾙﾄ値</param>
        /// <param name="bindingOption">添付ﾌﾟﾛﾊﾟﾃｨのﾊﾞｲﾝﾃﾞｨﾝｸﾞ方法</param>
        /// <param name="callback">値が変更された際に呼ばれる処理</param>
        /// <returns></returns>
        public static DependencyProperty RegisterAttached<T>(
                    string name, Type owner, T defaultValue, FrameworkPropertyMetadataOptions bindingOption, PropertyChangedCallback callback = null
            )
        {
            return RegisterAttached(name, owner, defaultValue, callback, bindingOption);
        }

        /// <summary>
        /// 依存関係ﾌﾟﾛﾊﾟﾃｨを登録します。
        /// </summary>
        /// <typeparam name="T">依存関係ﾌﾟﾛﾊﾟﾃｨのﾃﾞｰﾀ型</typeparam>
        /// <param name="name">名前</param>
        /// <param name="owner">依存関係ﾌﾟﾛﾊﾟﾃｨを保持するｸﾗｽの型</param>
        /// <param name="defaultValue">ﾃﾞﾌｫﾙﾄ値</param>
        /// <param name="callback">値変更時の処理</param>
        /// <param name="validate">値変更時の検証処理</param>
        /// <param name="bindingOption">依存関係ﾌﾟﾛﾊﾟﾃｨのﾊﾞｲﾝﾃﾞｨﾝｸﾞ方法</param>
        /// <returns></returns>
        public static DependencyProperty Register<T>(
                    string name, Type owner, T defaultValue, PropertyChangedCallback callback = null, ValidateValueCallback validate = null, FrameworkPropertyMetadataOptions bindingOption = FrameworkPropertyMetadataOptions.None
            )
        {
            return DependencyProperty.Register(
                name, typeof(T), owner, new FrameworkPropertyMetadata(defaultValue, bindingOption, callback), validate
            );
        }

        /// <summary>
        /// 依存関係ﾌﾟﾛﾊﾟﾃｨのﾊﾞｲﾝﾃﾞｨﾝｸﾞ方法を明示して依存関係ﾌﾟﾛﾊﾟﾃｨを登録します。
        /// </summary>
        /// <typeparam name="T">依存関係ﾌﾟﾛﾊﾟﾃｨのﾃﾞｰﾀ型</typeparam>
        /// <param name="name">名前</param>
        /// <param name="owner">依存関係ﾌﾟﾛﾊﾟﾃｨを保持するｸﾗｽの型</param>
        /// <param name="defaultValue">ﾃﾞﾌｫﾙﾄ値</param>
        /// <param name="bindingOption">依存関係ﾌﾟﾛﾊﾟﾃｨのﾊﾞｲﾝﾃﾞｨﾝｸﾞ方法</param>
        /// <param name="callback">値変更時の処理</param>
        /// <param name="validate">値変更時の検証処理</param>
        /// <returns></returns>
        public static DependencyProperty Register<T>(
                    string name, Type owner, T defaultValue, FrameworkPropertyMetadataOptions bindingOption, PropertyChangedCallback callback = null, ValidateValueCallback validate = null
            )
        {
            return Register(name, owner, defaultValue, callback, validate, bindingOption);
        }

        /// <summary>
        /// 指定したｵﾌﾞｼﾞｪｸﾄにｲﾍﾞﾝﾄを追加します。
        /// </summary>
        /// <typeparam name="T">ｵﾌﾞｼﾞｪｸﾄの型</typeparam>
        /// <param name="element">ｵﾌﾞｼﾞｪｸﾄ</param>
        /// <param name="add">ｲﾍﾞﾝﾄを追加するためのｱｸｼｮﾝ</param>
        /// <param name="del">ｲﾍﾞﾝﾄを削除するためのｱｸｼｮﾝ</param>
        public static void SetEventHandler<T>(T element, Action<T> add, Action<T> del) where T: FrameworkElement
        {
            if (element == null)
            {
                return;
            }

            //if (GetIsAttached(element))
            //{
            //    // ｱﾀｯﾁ済
            //    return;
            //}

            // ｱﾀｯﾁ完了を保存
            SetIsAttached(element, true);

            // 既存のｲﾍﾞﾝﾄを削除(ｲﾍﾞﾝﾄ登録されていない場合でも例外は発生しないのでとりあえず呼び出す)
            del?.Invoke(element);

            Action<object, RoutedEventArgs> loaded = null;
            Action<object, RoutedEventArgs> unloaded = null;

            // ｴﾚﾒﾝﾄのﾛｰﾄﾞ処理を定義(ｲﾍﾞﾝﾄ追加)
            loaded = (sender, e) =>
            {
                var fe = sender as FrameworkElement;

                if (loaded != null)
                {
                    fe.Loaded -= new RoutedEventHandler(loaded);
                    loaded = null;
                }

                // ｲﾍﾞﾝﾄ追加処理
                add?.Invoke((T)fe);

                // ｱﾝﾛｰﾄﾞｲﾍﾞﾝﾄ追加
                fe.Unloaded += new RoutedEventHandler(unloaded);

            };

            // ｴﾚﾒﾝﾄのｱﾝﾛｰﾄﾞ処理を定義(ｱﾝﾛｰﾄﾞ時にｲﾍﾞﾝﾄを削除する)
            unloaded = (sender, e) =>
            {
                var fe = sender as FrameworkElement;

                if (unloaded != null)
                {
                    fe.Unloaded -= new RoutedEventHandler(unloaded);
                    unloaded = null;
                }

                // ｱﾝﾛｰﾄﾞ時にｲﾍﾞﾝﾄ削除処理
                del?.Invoke((T)fe);

                // ｱﾀｯﾁ削除を保存
                SetIsAttached(element, false);

            };

            // 読込時処理の実行
            Loaded(element, new RoutedEventHandler(loaded));
        }

        /// <summary>
        /// ｵﾌﾞｼﾞｪｸﾄ読込時の処理を実行します。
        /// </summary>
        /// <param name="element">対象ｵﾌﾞｼﾞｪｸﾄ</param>
        /// <param name="handler">読込時処理</param>
        public static void Loaded(FrameworkElement element, RoutedEventHandler handler)
        {
            // ﾛｰﾄﾞｲﾍﾞﾝﾄ追加orﾛｰﾄﾞ済みの場合は直接実行
            if (element.IsLoaded)
            {
                element.Dispatcher.BeginInvoke(
                    new Action(() => handler(element, new RoutedEventArgs())),
                    DispatcherPriority.Loaded
                );
            }
            else
            {
                element.Loaded += handler;
            }
        }

        public static FormattedText GetFormattedText(TextBlock block, string text)
        {
            return new FormattedText(text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(block.FontFamily, block.FontStyle, block.FontWeight, block.FontStretch),
                block.FontSize,
                block.Foreground
            );
        }

        public static FormattedText GetFormattedText(Label label, string text)
        {
            return new FormattedText(text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(label.FontFamily, label.FontStyle, label.FontWeight, label.FontStretch),
                label.FontSize,
                label.Foreground
            );
        }

        public static FormattedText GetFormattedText(TextBlock block)
        {
            return GetFormattedText(block, block.Text);
        }

        public static FormattedText GetFormattedText(Label label)
        {
            return GetFormattedText(label, (string)label.Content);
        }
    }
}
