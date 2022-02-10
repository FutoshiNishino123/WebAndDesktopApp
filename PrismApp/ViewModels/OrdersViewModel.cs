using Data;
using Data.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismApp.Data;
using PrismApp.Events;
using PrismApp.Models;
using PrismApp.Regions;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using Unity;

namespace PrismApp.ViewModels
{
    public class OrdersViewModel : BindableBase, INavigationAware, IRibbon
    {
        [Dependency]
        public IContentRegionManager Region { get; set; }

        [Dependency]
        public IEventPublisher Event { get; set; }

        [Dependency]
        public OrdersFunction OrdersFunction { get; set; }

        #region Order property
        private Order? _order;
        public Order? Order
        {
            get => _order;
            set
            {
                if (SetProperty(ref _order, value))
                {
                    Event.RaiseSituationChanged();
                }
            }
        }
        #endregion

        #region Orders property
        private ObservableCollection<Order>? _orders;
        public ObservableCollection<Order>? Orders
        {
            get => _orders;
            set
            {
                if (SetProperty(ref _orders, value))
                {
                    Event.RaiseSituationChanged();
                }
            }
        }
        #endregion

        #region ShowDetailCommand property
        private DelegateCommand<int?>? _showDetailCommand;
        public DelegateCommand<int?> ShowDetailCommand => _showDetailCommand ??= new DelegateCommand<int?>(ShowDetail, CanShowDetail)
            .ObservesProperty(() => Orders);

        private void ShowDetail(int? id)
        {
            if (id.HasValue)
            {
                Region.Navigate("OrderDetail", id.Value);
            }
        }

        private bool CanShowDetail(int? id)
        {
            return id.HasValue;
        }
        #endregion

        #region SaveCommand property
        private DelegateCommand<Order>? _saveCommand;
        public DelegateCommand<Order> SaveCommand => _saveCommand ??= new DelegateCommand<Order>(Save, CanSave);

        private async void Save(Order order)
        {
            await OrderRepository.UpdateAsync(order);
            Event.RaiseSituationChanged();
        }

        private bool CanSave(Order order)
        {
            return true;
        }
        #endregion

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Event.RaiseSituationChanged();
            Event.RaiseOrdersActivated();
            Initialize();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Event.RaiseOrdersInactivated();
        }
        #endregion

        #region IRibbon
        public bool CanRefresh => Orders != null;
        public void Refresh()
        {
            if (CanRefresh)
            {
                Initialize();
            }
        }

        public bool CanAddNewItem => true;
        public void AddNewItem()
        {
            if (CanAddNewItem)
            {
                Region.Navigate("OrderEdit");
            }
        }

        public bool CanEditItem => Order != null && Order.IsActive;
        public void EditItem()
        {
            if (CanEditItem)
            {
                Debug.Assert(Order != null);
                Region.Navigate("OrderEdit", Order.Id);
            }
        }

        public bool CanDeleteItem => Order != null && Order.IsActive;
        public async void DeleteItem()
        {
            if (CanDeleteItem)
            {
                if (MessageBox.Show("削除しますか？", "確認", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Debug.Assert(Order != null);
                        await OrderRepository.DeleteAsync(Order.Id);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Orders?.Remove(Order);
                }
            }
        }
        #endregion

        public OrdersViewModel(IEventAggregator ea)
        {
            ea.GetEvent<OrderFilterChangedEvent>().Subscribe(_ => Initialize());
        }

        public async void Initialize()
        {
            var orders = await OrderRepository.ListAsync(OrdersFunction.Filter);
            Orders = new ObservableCollection<Order>(orders);
        }
    }
}