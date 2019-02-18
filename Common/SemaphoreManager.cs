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
        private const string DefaultKey = "default";

        private static Dictionary<string, SemaphoreSlim> Semaphores { get; set; } = new Dictionary<string, SemaphoreSlim>();

        public static async Task WaitAsync()
        {
            await WaitAsync(DefaultKey);
        }

        public static async Task WaitAsync(string key)
        {
            if (!Semaphores.ContainsKey(key))
            {
                Semaphores.Add(key, new SemaphoreSlim(1, 1));
            }
            await Semaphores[key].WaitAsync();
        }

        public static int Release()
        {
            return Release(DefaultKey);
        }

        public static int Release(string key)
        {
            return Semaphores[key].Release();
        }
    }
}
