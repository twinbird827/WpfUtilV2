using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfUtilV2.Common;

namespace WpfUtilV2.Mvvm.Service
{
    public class ConsoleMessageService : IMessageService
    {
        public virtual void Error(string message,
                [CallerMemberName] string callerMemberName = "",
                [CallerFilePath]   string callerFilePath = "",
                [CallerLineNumber] int callerLineNumber = 0)
        {
            Console.WriteLine(GetString(EventLogEntryType.Error, message, callerMemberName, callerFilePath, callerLineNumber));
        }

        public virtual void Info(string message,
                [CallerMemberName] string callerMemberName = "",
                [CallerFilePath]   string callerFilePath = "",
                [CallerLineNumber] int callerLineNumber = 0)
        {
            Console.WriteLine(GetString(EventLogEntryType.Information, message, callerMemberName, callerFilePath, callerLineNumber));
        }

        public virtual void Debug(string message,
                [CallerMemberName] string callerMemberName = "",
                [CallerFilePath]   string callerFilePath = "",
                [CallerLineNumber] int callerLineNumber = 0)
        {
            Console.WriteLine(GetString(EventLogEntryType.Information, message, callerMemberName, callerFilePath, callerLineNumber));
        }

        public virtual bool Confirm(string message,
                [CallerMemberName] string callerMemberName = "",
                [CallerFilePath]   string callerFilePath = "",
                [CallerLineNumber] int callerLineNumber = 0)
        {
            Console.WriteLine(GetString(EventLogEntryType.Information, message, callerMemberName, callerFilePath, callerLineNumber));
            return true;
        }

        public virtual void Exception(Exception exception,
                [CallerMemberName] string callerMemberName = "",
                [CallerFilePath]   string callerFilePath = "",
                [CallerLineNumber] int callerLineNumber = 0)
        {
            Console.WriteLine(GetString(EventLogEntryType.Error, exception.ToString(), callerMemberName, callerFilePath, callerLineNumber));
        }

        public virtual string SelectedSaveFile(string initializePath,
                string filter,
                [CallerMemberName] string callerMemberName = "",
                [CallerFilePath]   string callerFilePath = "",
                [CallerLineNumber] int callerLineNumber = 0)
        {
            return Console.ReadLine();
        }

        private static object LockObject = new object();

        private string GetString(EventLogEntryType type, 
                string message,
                string callerMemberName,
                string callerFilePath,
                int callerLineNumber)
        {
            var txt = $"[{type}][{DateTime.Now.ToString("yy/MM/dd HH:mm:ss.fff")}][{callerFilePath}][{callerMemberName}][{callerLineNumber}]\n{message}";

            if (type == EventLogEntryType.Error)
            {
                lock (LockObject)
                {
                    var dir = WpfUtil.RelativePathToAbsolutePath("log");
                    var tmp = Path.Combine(dir, $"{DateTime.Now.ToString("yyyy-MM-dd")}.log");

                    // ﾃﾞｨﾚｸﾄﾘを作成
                    Directory.CreateDirectory(dir);

                    File.AppendAllText(tmp, txt);
                }
            }

            return txt;
        }
    }
}
