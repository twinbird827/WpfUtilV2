using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using WpfUtilV2.Mvvm.Services;

namespace WpfUtilV2.Mvvm.Service
{
    public static class ServiceFactory
    {
        /// <summary>
        /// ﾒｯｾｰｼﾞ表示用ｻｰﾋﾞｽ
        /// </summary>
        public static IMessageService MessageService { get; set; } = new ConsoleMessageService();

        /// <summary>
        /// ﾘｿｰｽﾏﾈｰｼﾞｬ
        /// </summary>
        public static ResourceService ResourceService { get; set; }

    }
}
