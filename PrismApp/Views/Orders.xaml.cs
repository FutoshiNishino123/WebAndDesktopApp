using Data.Models;
using PrismApp.ViewModels;
using System;
using System.Collections.Generic;
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
        /// リストビューのヘッダがクリックされたときにデータをソートします。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainList_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is GridViewColumnHeader header)
            {
                ListViewUtils.SortByProperty(MainList, header);
                MainList.UnselectAll();
            }
        }

        /// <summary>
        /// リストビューの項目がダブルクリックされたときに詳細を表示します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (ListViewItem)sender;
            var order = (Order)item.DataContext;
            (DataContext as OrdersViewModel)?.ShowDetailCommand.Execute(order.Id);
        }
    }
}
