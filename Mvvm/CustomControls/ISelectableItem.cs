using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Mvvm.CustomControls
{
    public interface ISelectableItem
    {
        /// <summary>
        /// 選択中かどうか
        /// </summary>
        bool IsSelected
        {
            get;
            set;
        }
    }
}
