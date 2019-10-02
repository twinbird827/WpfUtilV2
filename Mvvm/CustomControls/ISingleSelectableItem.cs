using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Mvvm.CustomControls
{
    interface ISingleSelectableItem : ISelectableItem
    {
        IEnumerable<ISelectableItem> SelectableItems { get; }
    }
}
