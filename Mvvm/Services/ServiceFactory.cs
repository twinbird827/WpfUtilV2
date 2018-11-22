using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Mvvm.Service
{
    public static class ServiceFactory
    {
        /// <summary>
        /// ﾒｯｾｰｼﾞ表示用ｻｰﾋﾞｽ
        /// </summary>
        public static IMessageService MessageService { get; set; } = new ConsoleMessageService();
    }
}
