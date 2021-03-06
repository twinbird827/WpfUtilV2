﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Reflection;
using WpfUtilV2.Extensions;
using System.Windows.Threading;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;

namespace WpfUtilV2.Common
{
    public static class WpfUtil
    {
        /// <summary>
        /// ﾃﾞｻﾞｲﾝﾓｰﾄﾞかどうか確認します。
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// BitmapをImageSourceに変換します。
        /// </summary>
        /// <param name="bitmap">Bitmapｲﾒｰｼﾞ</param>
        /// <returns></returns>
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
                return BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad).Frozen();
            }
        }

        /// <summary>
        /// IconをImageSourceに変換します。
        /// </summary>
        /// <param name="icon">Iconｲﾒｰｼﾞ</param>
        /// <returns></returns>
        public static ImageSource ToImageSource(Icon icon)
        {
            return Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty, 
                BitmapSizeOptions.FromEmptyOptions()
            ).Frozen();
        }

        /// <summary>
        /// 定義済色ﾘｽﾄ
        /// </summary>
        public static SolidColorBrush[] Brushes
        {
            get
            {
                if (_Brushes != null) return _Brushes;

                var rgb = Enumerable.Range(1, 5).Select(i => i * 50).ToArray();

                _Brushes = rgb.SelectMany(r => rgb.SelectMany(g => rgb.Select(b => new { r, g, b })))
                    .Select(row => new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, (byte)row.r, (byte)row.g, (byte)row.b)))
                    .OrderBy(brush => brush.Color.ToString())
                    .ToArray();

                return _Brushes;
            }

        }
        private static SolidColorBrush[] _Brushes;

        /// <summary>
        /// 現在使用中のﾒﾓﾘを取得します。
        /// </summary>
        public static long TotalMemory
        {
            get { return GC.GetTotalMemory(false); }
        }

        /// <summary>
        /// 現在使用中のﾒﾓﾘ(KB換算)を文字列表現で取得します。
        /// </summary>
        public static string TotalKBString
        {
            get { return string.Concat((TotalMemory / 1024).ToString("#,0"), " KB"); }
        }

        /// <summary>
        /// 引数のうち、Null、または空文字ではない最初の文字列を取得します。
        /// </summary>
        /// <param name="values">条件ﾘｽﾄ</param>
        /// <returns>空文字ではない文字列</returns>
        public static string GetIsNotNull(params object[] values)
        {
            return values
                .Where(value => value != null)
                .Select(value => value is string ? (string)value : value.ToString())
                .FirstOrDefault(value => !string.IsNullOrWhiteSpace(value));
        }

        /// <summary>
        /// 処理を開始します。
        /// </summary>
        /// <param name="action">処理(非同期可)</param>
        public static void BeginInvoke(Action action)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(action);
        }

        /// <summary>
        /// Enum値へ変換します。
        /// </summary>
        /// <typeparam name="T">変換後のEnum型</typeparam>
        /// <param name="value">Enum型の文字列表現</param>
        /// <returns></returns>
        public static T ToEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Enum値へ変換します。
        /// </summary>
        /// <typeparam name="T">変換後のEnum型</typeparam>
        /// <param name="value">Enum型の文字列表現</param>
        /// <returns></returns>
        public static T ToEnum<T>(int value)
        {
            return ToEnum<T>(value.ToString());
        }

        /// <summary>
        /// Color値へ変換します。
        /// </summary>
        /// <param name="color">色を示す文字列</param>
        /// <returns></returns>
        public static System.Windows.Media.Color ToColor(string color)
        {
            return (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(color);
        }

        /// <summary>
        /// 単色Brushへ変換します。
        /// </summary>
        /// <param name="color">色を示す文字列</param>
        /// <returns></returns>
        public static SolidColorBrush ToBrush(string color)
        {
            return new SolidColorBrush(ToColor(color)).Frozen();
        }

        /// <summary>
        /// 対象文字配列のうち最初の空文字以外の文字を取得します。
        /// </summary>
        /// <param name="args">対象文字配列</param>
        public static string Nvl(params string[] args)
        {
            return args.FirstOrDefault(s => !string.IsNullOrEmpty(s)) ?? string.Empty;
        }

        /// <summary>
        /// 対象のパス名に使用できない文字が含まれていないか確認します。
        /// </summary>
        /// <param name="file">ファイル名</param>
        /// <returns></returns>
        public static bool IsSalePathInvalidChars(string path)
        {
            return !Path.GetInvalidPathChars().Any(c => path.Contains(c));
        }

        /// <summary>
        /// 対象のファイル名に使用できない文字が含まれていないか確認します。
        /// </summary>
        /// <param name="file">ファイル名</param>
        /// <returns></returns>
        public static bool IsSaleFileInvalidChars(string file)
        {
            return !Path.GetInvalidFileNameChars().Any(c => file.Contains(c));
        }

        /// <summary>
        /// 対象のファイル名に不正な文字が含まれていないか確認します。
        /// </summary>
        /// <param name="file">ファイル名</param>
        /// <returns></returns>
        public static bool IsSaleFileRegex(string file)
        {
            var regex = new Regex("[\\x00-\\x1f<>:\"/\\\\|?*]|^(CON|PRN|AUX|NUL|COM[0-9]|LPT[0-9]|CLOCK\\$)(\\.|$)|[\\. ]$", RegexOptions.IgnoreCase);

            return !regex.IsMatch(file);
        }

        public static string GetDelimiter(string file)
        {
            switch (Path.GetExtension(file).ToLower())
            {
                case ".tsv":
                    return "\t";
                case ".csv":
                    return ",";
                default:
                    return ",";
            }
        }

        /// <summary>
        /// 相対ﾊﾟｽを絶対ﾊﾟｽに変換します。
        /// </summary>
        /// <param name="relative">相対ﾊﾟｽ</param>
        /// <returns></returns>
        public static string RelativePathToAbsolutePath(string relative)
        {
            var work = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
            return Path.Combine(work, relative);
        }

        /// <summary>
        /// 非同期でｷｬﾝｾﾙ可能な待機処理を行います。
        /// </summary>
        /// <param name="delay">待機時間(ﾐﾘ秒)</param>
        /// <param name="token">ｷｬﾝｾﾙﾄｰｸﾝ</param>
        public static async Task<bool> Delay(int delay, CancellationToken token)
        {
            try
            {
                await Task.Delay(delay, token);
                return true;
            }
            catch (TaskCanceledException)
            {
                return false;
            }
        }
    }

    /// <summary>
    /// DPI取得用ｸﾗｽ
    /// </summary>
    internal sealed class DCSafeHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        private DCSafeHandle() : base(true) { }

        protected override Boolean ReleaseHandle()
        {
            return UnsafeNativeMethods.DeleteDC(base.handle);
        }
    }

    /// <summary>
    /// DPI取得用ｸﾗｽ
    /// </summary>
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
