using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace PrismApp.Views.Helpers
{
    public static class ListViewHelper
    {
        /// <summary>
        /// 表示メンバにバインドされたプロパティを基準に ItemsControl をソートします。
        /// </summary>
        /// <param name="control">ソート対象の ItemsControl</param>
        /// <param name="header">ソート対象のヘッダ</param>
        public static void SortByProperty(ItemsControl control, GridViewColumnHeader header)
        {
            var binding = header.Column?.DisplayMemberBinding as Binding;
            var propertyName = binding?.Path?.Path;
            if (propertyName is null)
            {
                return;
            }

            SortByProperty(control, propertyName);
        }

        /// <summary>
        /// プロパティを基準に ItemsControl をソートします。
        /// </summary>
        /// <param name="control">ソート対象の ItemsControl</param>
        /// <param name="propertyName">ソート対象のプロパティ名</param>
        public static void SortByProperty(ItemsControl control, string propertyName)
        {
            if (control.ItemsSource is null)
            {
                return;
            }

            SortByProperty(control.Items, propertyName);
        }

        /// <summary>
        /// プロパティを基準に ItemCollection をソートします。
        /// </summary>
        /// <param name="items">ソート対象の ItemCollection</param>
        /// <param name="propertyName">ソート対象のプロパティ名</param>
        public static void SortByProperty(ItemCollection items, string propertyName)
        {
            ListSortDirection direction;

            if (items.SortDescriptions.Count == 0
                || items.SortDescriptions.Last().Direction == ListSortDirection.Ascending)
            {
                direction = ListSortDirection.Descending;
            }
            else
            {
                direction = ListSortDirection.Ascending;
            }

            items.SortDescriptions.Clear();
            items.SortDescriptions.Add(new SortDescription(propertyName, direction));
        }
    }
}
