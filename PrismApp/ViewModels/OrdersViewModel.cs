using Data;
using Data.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismApp.Models;
using PrismApp.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Unity;

namespace PrismApp.ViewModels
{
    public class OrdersViewModel : BindableBase, INavigationAware, IRibbon
    {
        [Dependency]
        public IContentRegionManager RegionManager { get; set; }

        [Dependency]
        public IEventPublisher EventPublisher { get; set; }

        #region Order property
        private Order? _order;
        public Order? Order
        {
            get => _order;
            set
            {
                if (SetProperty(ref _order, value))
                {
                    EventPublisher.RaiseSituationChanged();
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
                    EventPublisher.RaiseSituationChanged();
                }
            }
        }
        #endregion

        #region OrderFilter property
        private OrderFilter? _orderFilter = new OrderFilter();
        public OrderFilter? OrderFilter
        {
            get => _orderFilter;
            set => SetProperty(ref _orderFilter, value);
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
                RegionManager.Navigate("OrderDetail", id.Value);
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
            await OrdersRepository.SaveOrderAsync(order);

            EventPublisher.RaiseSituationChanged();
        }

        private bool CanSave(Order order)
        {
            return true;
        }
        #endregion

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            EventPublisher.RaiseSituationChanged();
            EventPublisher.RaiseOrdersActivated();

            Initialize();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            EventPublisher.RaiseOrdersInactivated();
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
                RegionManager.Navigate("OrderEdit");
            }
        }

        public bool CanEditItem => Order != null && !Order.IsClosed;
        public void EditItem()
        {
            if (CanEditItem)
            {
                Debug.Assert(Order != null);
                RegionManager.Navigate("OrderEdit", Order.Id);
            }
        }

        public bool CanDeleteItem => Order != null && !Order.IsClosed;
        public async void DeleteItem()
        {
            if (CanDeleteItem)
            {
                if (MessageBox.Show("削除しますか？", "確認", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Debug.Assert(Order != null);
                        await OrdersRepository.DeleteOrderAsync(Order.Id);
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
            ea.GetEvent<OrderFilterChangedEvent>().Subscribe(filter =>
            {
                OrderFilter = filter;
                Initialize();
            });
        }

        public async void Initialize()
        {
            var orders = await OrdersRepository.GetOrdersAsync();

            if (OrderFilter != null)
            {
                orders = orders.Where(OrderFilter.Filter);
            }

            Orders = new ObservableCollection<Order>(orders);
        }
    }
}