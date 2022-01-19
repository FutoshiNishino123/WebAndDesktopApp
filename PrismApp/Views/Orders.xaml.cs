using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrismApp.Views
{
    /// <summary>
    /// Orders.xaml の相互作用ロジック
    /// </summary>
    public partial class Orders : UserControl
    {
        public Orders()
        {
            InitializeComponent();
        }

        /// <summary>
        /// リストビューのヘッダーをクリックしてデータをソートします。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainList_Click(object sender, RoutedEventArgs e)
        {
            if (MainList.ItemsSource is null)
            {
                return;
            }

            var header = e.OriginalSource as GridViewColumnHeader;
            if (header is null || header.Column is null)
            {
                return;
            }

            // 列の表示メンバにバインドされている（またはヘッダーにタグ付けされている）プロパティ名を取得
            var binding = header.Column.DisplayMemberBinding as Binding;
            var propertyName = binding?.Path?.Path;
            if (propertyName is null)
            {
                propertyName = (string)header.Tag;
            }

            ListSortDirection sortDirection;

            if (MainList.Items.SortDescriptions.Count == 0
                || MainList.Items.SortDescriptions.Last().Direction == ListSortDirection.Ascending)
            {
                sortDirection = ListSortDirection.Descending;
            }
            else
            {
                sortDirection = ListSortDirection.Ascending;
            }

            MainList.Items.SortDescriptions.Clear();
            MainList.Items.SortDescriptions.Add(new SortDescription(propertyName, sortDirection));
            MainList.UnselectAll();
        }
    }
}
