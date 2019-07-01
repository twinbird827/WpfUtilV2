using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfUtilV2.Common
{
    public static class SemaphoreManager
    {
        /// <summary>
        /// ｾﾏﾌｫﾘｽﾄ
        /// </summary>
        private static Dictionary<string, SemaphoreSlim> Semaphores { get; set; } = new Dictionary<string, SemaphoreSlim>();

        /// <summary>
        /// 指定したｷｰで待機します。
        /// </summary>
        /// <param name="key">待機するｷｰ</param>
        public static async Task WaitAsync(string key)
        {
            if (!Semaphores.ContainsKey(key))
            {
                Semaphores.Add(key, new SemaphoreSlim(1, 1));
            }
            await Semaphores[key].WaitAsync();
        }

        /// <summary>
        /// 指定したｷｰで待機します。
        /// </summary>
        /// <param name="key">待機するｷｰ</param>
        public static void Wait(string key)
        {
            if (!Semaphores.ContainsKey(key))
            {
                Semaphores.Add(key, new SemaphoreSlim(1, 1));
            }
            Semaphores[key].Wait();
        }

        /// <summary>
        /// 指定したｷｰの待機状態を解除します。
        /// </summary>
        /// <param name="key">解除するｷｰ</param>
        public static int Release(string key)
        {
            return Semaphores[key].Release();
        }
    }
}
