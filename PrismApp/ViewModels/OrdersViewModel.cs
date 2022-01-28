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
        public IRegionManager RegionManager { get; set; }

        [Dependency]
        public IEventAggregator EventAggregator { get; set; }

        #region Order property
        private Order? _order;
        public Order? Order
        {
            get => _order;
            set
            {
                if (SetProperty(ref _order, value))
                {
                    RaiseSituationChanged();
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
                    RaiseSituationChanged();
                }
            }
        }
        #endregion

        public OrderFilter? Filter { get; set; }

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
            await OrdersRepository.SaveOrderAsync(order);
            RaiseSituationChanged();
        }

        private bool CanSave(Order order)
        {
            return true;
        }
        #endregion

        public OrdersViewModel(IEventAggregator ea)
        {
            ea.GetEvent<OrderFilterChangedEvent>().Subscribe(filter =>
            {
                Filter = filter;
                Initialize();
            });
        }

        public async void Initialize()
        {
            var orders = await OrdersRepository.GetOrdersAsync();

            if (Filter != null)
            {
                orders = orders.Where(Filter.Filter);
            }

            Orders = new ObservableCollection<Order>(orders);
        }

        private void RaiseSituationChanged()
        {
            EventAggregator.GetEvent<SituationChangedEvent>().Publish();
        }

        private void PublishNavigatedToOrdersEvent()
        {
            EventAggregator.GetEvent<NavigatedToOrdersEvent>().Publish();
        }

        private void PublishNavigatedFromOrdersEvent()
        {
            EventAggregator.GetEvent<NavigatedFromOrdersEvent>().Publish();
        }

        private void NavigateToOrderDetail(int? id)
        {
            var parameters = new NavigationParameters();
            parameters.Add("id", id);
            RegionManager.RequestNavigate(RegionNames.ContentRegion, "OrderDetail", parameters);
        }

        private void NavigateToOrderEdit(int? id)
        {
            var parameters = new NavigationParameters();
            parameters.Add("id", id);
            RegionManager.RequestNavigate(RegionNames.ContentRegion, "OrderEdit", parameters);
        }

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            RaiseSituationChanged();
            PublishNavigatedToOrdersEvent();

            Initialize();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            PublishNavigatedFromOrdersEvent();
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
                Debug.Assert(Order != null);
                NavigateToOrderEdit(Order.Id);
            }
        }

        public bool CanDeleteItem => Order != null && !Order.IsClosed;
        public async void DeleteItem()
        {
            if (!CanDeleteItem)
            {
                return;
            }

            if (MessageBox.Show("削除しますか？", "確認", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }
            
            Debug.Assert(Order != null);

            try
            {
                await OrdersRepository.DeleteOrderAsync(Order.Id);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Orders?.Remove(Order);
        }
        #endregion
    }
}