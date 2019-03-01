using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Mvvm.Service
{
    public class ConsoleMessageService : IMessageService
    {
        public virtual void Error(string message,
                [CallerMemberName] string callerMemberName = "",
                [CallerFilePath]   string callerFilePath = "",
                [CallerLineNumber] int callerLineNumber = 0)
        {
            Console.WriteLine(string.Format("[ERROR][{0:yy/MM/dd HH:mm:ss}][{1}][{2}][{3}]\n{4}", DateTime.Now, callerFilePath, callerMemberName, callerLineNumber, message));
        }

        public virtual void Info(string message,
                [CallerMemberName] string callerMemberName = "",
                [CallerFilePath]   string callerFilePath = "",
                [CallerLineNumber] int callerLineNumber = 0)
        {
            Console.WriteLine(string.Format("[INFO][{0:yy/MM/dd HH:mm:ss}][{1}][{2}][{3}]\n{4}", DateTime.Now, callerFilePath, callerMemberName, callerLineNumber, message));
        }

        public virtual void Debug(string message,
                [CallerMemberName] string callerMemberName = "",
                [CallerFilePath]   string callerFilePath = "",
                [CallerLineNumber] int callerLineNumber = 0)
        {
            Console.WriteLine(string.Format("[DEBUG][{0:yy/MM/dd HH:mm:ss}][{1}][{2}][{3}]\n{4}", DateTime.Now, callerFilePath, callerMemberName, callerLineNumber, message));
        }

        public virtual bool Confirm(string message,
                [CallerMemberName] string callerMemberName = "",
                [CallerFilePath]   string callerFilePath = "",
                [CallerLineNumber] int callerLineNumber = 0)
        {
            Console.WriteLine(string.Format("[CONFIRM][{0:yy/MM/dd HH:mm:ss}][{1}][{2}][{3}]\n{4}", DateTime.Now, callerFilePath, callerMemberName, callerLineNumber, message));
            return true;
        }

        public virtual void Exception(Exception exception,
                [CallerMemberName] string callerMemberName = "",
                [CallerFilePath]   string callerFilePath = "",
                [CallerLineNumber] int callerLineNumber = 0)
        {
            Console.WriteLine(string.Format("[EXCEPTION][{0:yy/MM/dd HH:mm:ss}][{1}][{2}][{3}]\n{4}", DateTime.Now, callerFilePath, callerMemberName, callerLineNumber, exception.ToString()));
        }
    }
}
