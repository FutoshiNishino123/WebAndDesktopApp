using Data;
using Data.Models;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismApp.Events;
using System;
using System.Diagnostics;
using System.Windows;
using Unity;
using System.Threading.Tasks;
using PrismApp.Regions;
using PrismApp.Data;

namespace PrismApp.ViewModels
{
    public class OrderDetailViewModel : BindableBase, INavigationAware, IOperationCommands
    {
        [Dependency]
        public IContentRegionManager Region { get; set; }

        [Dependency]
        public IEventPublisher Event { get; set; }

        #region Order property
        private Order? order;
        public Order? Order
        {
            get => order;
            set
            {
                if (SetProperty(ref order, value))
                {
                    Event.RaiseSituationChanged();
                }
            }
        }
        #endregion

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Event.RaiseSituationChanged();
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

        #region IOperationCommands
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

        public bool CanEditItem => Order != null && Order.IsActive;
        public void EditItem()
        {
            if (Order != null)
            {
                Region.Navigate("OrderEdit", Order.Id);
            }
        }

        public bool CanDeleteItem => Order != null && Order.IsActive;
        public async void DeleteItem()
        {
            if (Order != null)
            {
                if (MessageBox.Show("削除しますか？", "確認", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await OrderRepository.DeleteAsync(Order.Id);

                    Region.GoBack();
                }
            }
        }
        #endregion

        private async void Initialize(int? id)
        {
            Order = null;

            var order = id.HasValue ? await OrderRepository.GetByIdAsync(id.Value) : null;
            if (order is null)
            {
                MessageBox.Show("レコードが見つかりません", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Region.GoBack();
                return;
            }

            Order = order;
        }
    }
}