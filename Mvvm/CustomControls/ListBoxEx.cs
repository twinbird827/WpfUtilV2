using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfUtilV2.Mvvm.CustomControls
{
    public class ListBoxEx : ListBox
    {
        public ListBoxEx() : base()
        {
            SelectionChanged += lb_SelectionChanged;
        }

        /// <summary>
        /// 選択行変更時ｲﾍﾞﾝﾄ
        /// </summary>
        private void lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.RemovedItems.Cast<ISelectableItem>())
                item.IsSelected = false;
            foreach (var item in e.AddedItems.Cast<ISelectableItem>())
                item.IsSelected = true;
        }

        /// <summary>
        /// ItemsSource項目数変更時ｲﾍﾞﾝﾄ
        /// </summary>
        private void lvw_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Items.Count == 0) return;

            ScrollIntoView(Items[0]);
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            if (!object.Equals(oldValue, newValue) && newValue != null)
            {
                (newValue as INotifyCollectionChanged).CollectionChanged += 
                    new NotifyCollectionChangedEventHandler(lvw_CollectionChanged);
            }

            base.OnItemsSourceChanged(oldValue, newValue);
        }
    }
}
