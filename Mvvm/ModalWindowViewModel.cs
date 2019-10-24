using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfUtilV2.Mvvm
{
    public class ModalWindowViewModel : BindableBase
    {
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
            _ => DialogResult = true,
            e => CanClickOK(e)
        );

        /// <summary>
        /// ｷｬﾝｾﾙﾎﾞﾀﾝ押下時処理
        /// </summary>
        public ICommand OnClickCancel => new RelayCommand(
            _ => DialogResult = false,
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
    }
}
