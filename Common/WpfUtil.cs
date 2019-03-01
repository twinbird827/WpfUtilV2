using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Windows.Interop;

namespace WpfUtilV2.Common
{
    public static class WpfUtil
    {
        public static bool IsDesignMode()
        {
            // Check for design mode. 
            return (bool)DesignerProperties
                .IsInDesignModeProperty
                .GetMetadata(typeof(DependencyObject))
                .DefaultValue;
        }

        /// <summary>
        /// システムから画面の DPI 設定を取得します。
        /// </summary>
        /// <param name="orientation">画面の方向</param>
        /// <returns></returns>
        public static Int32 GetDpi(Orientation orientation)
        {
            Int32 capIndex = (orientation == Orientation.Horizontal) ? 0x58 : 90;
            using (DCSafeHandle handle = UnsafeNativeMethods.CreateDC("DISPLAY"))
            {
                return (handle.IsInvalid ? 0x60 : UnsafeNativeMethods.GetDeviceCaps(handle, capIndex));
            }
        }

        public static T[] ThinningOut<T>(T[] data, int afterCount)
        {
            var lineCount = data.Length;
            if (afterCount < lineCount)
            {
                return Enumerable.Range(0, afterCount)
                    .Select(i => (int)Math.Ceiling((double)(i * lineCount / afterCount)))
                    .Select(i => data[i])
                    .ToArray();
            }
            else
            {
                return data;
            }
        }

        public static ImageSource ToImageSource(Bitmap bitmap)
        {
            using (var ms = new WrappingStream(new MemoryStream()))
            {
                // MemoryStreamに書き出す
                bitmap.Save(ms, ImageFormat.Png);
                // MemoryStreamをシーク
                ms.Seek(0, SeekOrigin.Begin);
                // MemoryStreamからBitmapFrameを作成
                // (BitmapFrameはBitmapSourceを継承しているのでそのまま渡せばOK)
                return BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad).GetAsFrozen() as BitmapFrame;
            }
        }

        public static ImageSource ToImageSource(Icon icon)
        {
            return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()).GetAsFrozen() as BitmapSource;
        }
    }

    internal sealed class DCSafeHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        private DCSafeHandle() : base(true) { }

        protected override Boolean ReleaseHandle()
        {
            return UnsafeNativeMethods.DeleteDC(base.handle);
        }
    }

    [System.Security.SuppressUnmanagedCodeSecurity]
    internal static class UnsafeNativeMethods
    {
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern Boolean DeleteDC(IntPtr hDC);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern Int32 GetDeviceCaps(DCSafeHandle hDC, Int32 nIndex);

        [DllImport("gdi32.dll", EntryPoint = "CreateDC", CharSet = CharSet.Auto)]
        public static extern DCSafeHandle IntCreateDC(String lpszDriver,
            String lpszDeviceName, String lpszOutput, IntPtr devMode);

        public static DCSafeHandle CreateDC(String lpszDriver)
        {
            return UnsafeNativeMethods.IntCreateDC(lpszDriver, null, null, IntPtr.Zero);
        }
    }
}
