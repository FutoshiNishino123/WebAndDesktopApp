using Data;
using Data.Models;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismApp.Models;
using PrismApp.Events;
using System;
using System.Diagnostics;
using System.Windows;
using Unity;
using System.Threading.Tasks;
using PrismApp.Regions;

namespace PrismApp.ViewModels
{
    public class OrderDetailViewModel : BindableBase, INavigationAware, IRibbon
    {
        [Dependency]
        public IContentRegionManager RegionManager { get; set; }

        [Dependency]
        public IEventPublisher EventPublisher { get; set; }

        #region Order property
        private Order? order;
        public Order? Order
        {
            get => order;
            set
            {
                if (SetProperty(ref order, value))
                {
                    EventPublisher.RaiseSituationChanged();
                }
            }
        }
        #endregion

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            EventPublisher.RaiseSituationChanged();

            var id = (int?)navigationContext.Parameters["id"];
            Initialize(id);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
        #endregion

        #region IRibbon
        public bool CanRefresh => Order != null;
        public void Refresh()
        {
            if (Order != null)
            {
                Initialize(Order.Id);
            }
        }

        public bool CanAddNewItem => false;
        public void AddNewItem()
        {
            throw new NotSupportedException();
        }

        public bool CanEditItem => Order != null && !Order.IsClosed;
        public void EditItem()
        {
            if (Order != null)
            {
                RegionManager.Navigate("OrderEdit", Order.Id);
            }
        }

        public bool CanDeleteItem => Order != null && !Order.IsClosed;
        public async void DeleteItem()
        {
            if (Order != null)
            {
                if (MessageBox.Show("削除しますか？", "確認", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await OrdersRepository.DeleteOrderAsync(Order.Id);

                    RegionManager.GoBack();
                }
            }
        }
        #endregion

        private async void Initialize(int? id)
        {
            Order = null;

            var order = id.HasValue ? await OrdersRepository.FindOrderAsync(id.Value) : null;
            if (order is null)
            {
                MessageBox.Show("レコードが見つかりません", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                RegionManager.GoBack();
                return;
            }

            Order = order;
        }
    }
}