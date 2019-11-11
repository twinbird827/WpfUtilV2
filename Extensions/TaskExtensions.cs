using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Extensions
{
    public static class TaskExtensions
    {
        /// <summary>
        /// 非同期ﾀｽｸを実行し、指定した時間で処理が完了しない場合は例外を発生させます。
        /// </summary>
        /// <param name="task">非同期ﾀｽｸ</param>
        /// <param name="timeout">ﾀｲﾑｱｳﾄ時間</param>
        /// <returns></returns>
        public static async Task Timeout(this Task task, TimeSpan timeout)
        {
            var delay = Task.Delay(timeout);
            if (await Task.WhenAny(task, delay).ConfigureAwait(false) == delay)
            {
                throw new TimeoutException();
            }
        }

        /// <summary>
        /// 非同期ﾀｽｸを実行し、指定した時間で処理が完了しない場合は例外を発生させます。
        /// </summary>
        /// <param name="task">非同期ﾀｽｸ</param>
        /// <param name="timeout">ﾀｲﾑｱｳﾄ時間</param>
        /// <returns></returns>
        public static async Task<T> Timeout<T>(this Task<T> task, TimeSpan timeout)
        {
            await ((Task)task).Timeout(timeout);
            return await task;
        }

        /// <summary>
        /// 非同期ﾀｽｸをすべて実行します。
        /// </summary>
        /// <param name="tasks">非同期ﾀｽｸﾘｽﾄ</param>
        /// <returns></returns>
        public static Task WhenAll(this IEnumerable<Task> tasks)
        {
            return Task.WhenAll(tasks);
        }

        /// <summary>
        /// 非同期ﾀｽｸをすべて実行します。
        /// </summary>
        /// <param name="tasks">非同期ﾀｽｸﾘｽﾄ</param>
        /// <returns></returns>
        public static Task<T[]> WhenAll<T>(this IEnumerable<Task<T>> tasks)
        {
            return Task.WhenAll(tasks);
        }
    }
}
