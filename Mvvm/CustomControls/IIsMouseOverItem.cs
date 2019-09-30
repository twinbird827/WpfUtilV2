using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Mvvm.CustomControls
{
    public interface IIsMouseOverItem
    {
        /// <summary>
        /// ﾏｳｽｵｰﾊﾞｰ中かどうか
        /// </summary>
        bool IsMouseOver
        {
            get;
            set;
        }
    }
}
