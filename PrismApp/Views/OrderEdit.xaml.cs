using System;
using System.Collections.Generic;
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
    /// OrderEdit.xaml の相互作用ロジック
    /// </summary>
    public partial class OrderEdit : UserControl
    {
        public OrderEdit()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // 日付選択時の初期値に前回の設定値が入ってしまうのでリセット
            if (ExpirationDate.SelectedDate == null)
            {
                ExpirationDate.SelectedDate = DateTime.Now;
                ExpirationDate.SelectedDate = null;
            }
        }
    }
}
