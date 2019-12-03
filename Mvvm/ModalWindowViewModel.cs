using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfUtilV2.Mvvm
{
    public class ModalWindowViewModel : BindableBase
    {
        /// <summary>
        /// 画面終了時のﾁｪｯｸを実行するかどうか
        /// </summary>
        private bool IsClosingCheck { get; set; } = true;

        /// <summary>
        /// ﾀﾞｲｱﾛｸﾞ結果
        /// </summary>
        public bool? DialogResult
        {
            get { return _DialogResult; }
            set { SetProperty(ref _DialogResult, value); }
        }
        private bool? _DialogResult;

        /// <summary>
        /// OKﾎﾞﾀﾝ押下時処理
        /// </summary>
        public ICommand OnClickOK => new RelayCommand(
            _ => { if (BeforeClickOK(_)) DialogResult = true; },
            e => CanClickOK(e)
        );

        /// <summary>
        /// ｷｬﾝｾﾙﾎﾞﾀﾝ押下時処理
        /// </summary>
        public ICommand OnClickCancel => new RelayCommand(
            _ => { if (BeforeClickCancel(_)) DialogResult = false; },
            e => CanClickCancel(e)
        );

        /// <summary>
        /// OKﾎﾞﾀﾝ押下時処理が実行可能か確認します。
        /// </summary>
        /// <typeparam name="T">ﾊﾟﾗﾒｰﾀの型</typeparam>
        /// <param name="parameter">ﾊﾟﾗﾒｰﾀ</param>
        /// <returns></returns>
        protected virtual bool CanClickOK<T>(T parameter)
        {
            return true;
        }

        /// <summary>
        /// ｷｬﾝｾﾙﾎﾞﾀﾝ押下時処理が実行可能か確認します。
        /// </summary>
        /// <typeparam name="T">ﾊﾟﾗﾒｰﾀの型</typeparam>
        /// <param name="parameter">ﾊﾟﾗﾒｰﾀ</param>
        /// <returns></returns>
        protected virtual bool CanClickCancel<T>(T parameter)
        {
            return true;
        }

        /// <summary>
        /// OKﾎﾞﾀﾝの前処理を実行します。
        /// </summary>
        /// <typeparam name="T">ﾊﾟﾗﾒｰﾀの型</typeparam>
        /// <param name="parameter">ﾊﾟﾗﾒｰﾀ</param>
        protected virtual bool BeforeClickOK<T>(T parameter)
        {
            IsClosingCheck = false;
            return true;
        }

        /// <summary>
        /// ｷｬﾝｾﾙﾎﾞﾀﾝの前処理を実行します。
        /// </summary>
        /// <typeparam name="T">ﾊﾟﾗﾒｰﾀの型</typeparam>
        /// <param name="parameter">ﾊﾟﾗﾒｰﾀ</param>
        protected virtual bool BeforeClickCancel<T>(T parameter)
        {
            if (CheckClosing())
            {
                IsClosingCheck = false;
                return true;
            }
            else
            {
                IsClosingCheck = true;
                return false;
            }
        }

        /// <summary>
        /// 画面終了前処理
        /// </summary>
        public ICommand OnClosing =>
            _OnClosing = _OnClosing ?? new RelayCommand<CancelEventArgs>(e =>
            {
                if (!IsClosingCheck)
                {
                    return;
                }
                else if (!CheckClosing())
                {
                    e.Cancel = true;
                }
            });
        private ICommand _OnClosing;

        /// <summary>
        /// 画面を終了してもよいかどうか確認します。
        /// </summary>
        protected virtual bool CheckClosing()
        {
            return true;
        }
    }
}
