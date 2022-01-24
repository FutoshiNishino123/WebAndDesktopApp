using Data;
using Data.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismApp.Controllers;
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
        public IRegionManager? RegionManager { get; set; }

        [Dependency]
        public IEventAggregator? EventAggregator { get; set; }

        #region Order property
        private Order? _order;
        public Order? Order
        {
            get => _order;
            set
            {
                if (SetProperty(ref _order, value))
                {
                    PublishSituationChangedEvent();
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
                    PublishSituationChangedEvent();
                }
            }
        }
        #endregion

        #region HideClosedOrders property
        private bool _hideClosedOrders = true;
        public bool HideClosedOrders
        {
            get => _hideClosedOrders;
            set
            {
                if (SetProperty(ref _hideClosedOrders, value))
                {
                    Initialize();
                }
            }
        }
        #endregion

        #region Filter property
        public Func<Order, bool> Filter
        {
            get
            {
                return order =>
                {
                    if (HideClosedOrders && order.IsClosed)
                    {
                        return false;
                    }
                    return true;
                };
            }
        }
        #endregion

        #region ShowDetailCommand property
        private DelegateCommand? _showDetailCommand;
        public DelegateCommand ShowDetailCommand => _showDetailCommand ??= new DelegateCommand(ShowDetail, CanShowDetail);

        private void ShowDetail()
        {
            if (Order != null)
            {
                NavigateToOrderDetail(Order.Id);
            }
        }

        private bool CanShowDetail()
        {
            return true;
        }
        #endregion

        #region SaveCommand property
        private DelegateCommand<Order>? _saveCommand;
        public DelegateCommand<Order> SaveCommand => _saveCommand ??= new DelegateCommand<Order>(Save, CanSave);

        private async void Save(Order order)
        {
            await OrderController.SaveOrderAsync(order);
            PublishSituationChangedEvent();
        }

        private bool CanSave(Order order)
        {
            return true;
        }
        #endregion

        public OrdersViewModel(IEventAggregator ea)
        {
            ea.GetEvent<HideClosedOrdersEvent>().Subscribe(value => HideClosedOrders = value);
        }

        public async void Initialize()
        {
            var orders = await OrderController.GetOrdersAsync();
            orders = orders.Where(Filter);
            Orders = new ObservableCollection<Order>(orders);
        }

        private void PublishSituationChangedEvent()
        {
            EventAggregator?.GetEvent<SituationChangedEvent>().Publish();
        }

        private void NavigateToOrderDetail(int? id)
        {
            var parameters = new NavigationParameters();
            parameters.Add("id", id);
            RegionManager?.RequestNavigate(RegionNames.ContentRegion, "OrderDetail", parameters);
        }

        private void NavigateToOrderEdit(int? id)
        {
            var parameters = new NavigationParameters();
            parameters.Add("id", id);
            RegionManager?.RequestNavigate(RegionNames.ContentRegion, "OrderEdit", parameters);
        }

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Initialize();
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
                NavigateToOrderEdit(null);
            }
        }

        public bool CanEditItem => Order != null && !Order.IsClosed;
        public void EditItem()
        {
            if (CanEditItem)
            {
                NavigateToOrderEdit(Order?.Id);
            }
        }

        public bool CanDeleteItem => Order != null && !Order.IsClosed;
        public async void DeleteItem()
        {
            if (CanDeleteItem)
            {
                if (MessageBox.Show("削除しますか？", "確認", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Debug.Assert(Order != null);
                    await OrderController.DeleteOrderAsync(Order.Id);
                    Orders?.Remove(Order);
                }
            }
        }
        #endregion
    }
}