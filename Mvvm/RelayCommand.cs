using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfUtilV2.Common;
using WpfUtilV2.Mvvm.Service;

namespace WpfUtilV2.Mvvm
{
    public class RelayCommand : RelayCommand<object>
    {
        #region Fields

        #endregion // Fields

        #region Constructors

        public RelayCommand(Action<object> execute)
        : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
            : base(execute, canExecute)
        {
        }
        #endregion // Constructors

    }

    public class RelayCommand<T> : ICommand
    {
        #region Fields

        readonly Action<T> _execute;
        readonly Predicate<T> _canExecute;

        #endregion // Fields

        #region Constructors

        public RelayCommand(Action<T> execute)
        : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion // Constructors

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            if (!WpfUtil.IsDesignMode())
            {
                try
                {
                    // 別のRelayCommandから呼び出されたｹｰｽを考慮して、
                    // 本ﾒｿｯﾄﾞ内でもCanExecuteにて実行可否を判断する
                    if (CanExecute(parameter)) _execute((T)parameter);
                }
                catch (Exception ex)
                {
                    ServiceFactory.MessageService.Exception(ex);
                }
            }
        }

        #endregion // ICommand Members
    }
}
