using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Mvvm.CustomControls
{
    public interface IFocusableItem
    {
        /// <summary>
        /// ﾌｫｰｶｽ取得中かどうか
        /// </summary>
        bool IsFocused
        {
            get;
            set;
        }
    }
}
