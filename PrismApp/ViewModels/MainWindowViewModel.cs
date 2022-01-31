using Data.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismApp.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace PrismApp.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        [Dependency]
        public IContentRegionManager RegionManager { get; set; }

        [Dependency]
        public IEventPublisher EventPublisher { get; set; }

        #region Title property
        private string _title = "Demo";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        #endregion

        #region LogInUser property
        private User? _logInUser;
        public User? LogInUser
        {
            get => _logInUser;
            set => SetProperty(ref _logInUser, value);
        }
        #endregion

        #region OrdersEnabled property
        private bool _ordersEnabled;
        public bool OrdersEnabled
        {
            get => _ordersEnabled;
            set => SetProperty(ref _ordersEnabled, value);
        }
        #endregion

        #region OrderFilter property
        private OrderFilter _orderFilter = new OrderFilter();
        public OrderFilter OrderFilter
        {
            get => _orderFilter;
            set => SetProperty(ref _orderFilter, value);
        }
        #endregion

        #region OrderFilterChangedCommand property
        private DelegateCommand? _orderFilterChangedCommand;
        public DelegateCommand OrderFilterChangedCommand => _orderFilterChangedCommand ??= new DelegateCommand(OrderFilterChanged, CanOrderFilterChanged)
            .ObservesProperty(() => OrderFilter);

        private void OrderFilterChanged()
        {
            EventPublisher.RaiseOrderFilterChanged(OrderFilter);
        }

        private bool CanOrderFilterChanged()
        {
            return OrderFilter != null;
        }
        #endregion

        #region NavigateCommand property
        private DelegateCommand<string>? _navigateCommand;
        public DelegateCommand<string> NavigateCommand => _navigateCommand ??= new DelegateCommand<string>(Navigate, CanNavigate);

        private void Navigate(string path)
        {
            RegionManager.Navigate(path);
        }

        private bool CanNavigate(string path)
        {
            return true;
        }
        #endregion

        #region GoBackCommand property
        private DelegateCommand? _goBackCommand;
        public DelegateCommand GoBackCommand => _goBackCommand ??= new DelegateCommand(GoBack, CanGoBack);

        private void GoBack()
        {
            RegionManager.GoBack();
        }

        private bool CanGoBack()
        {
            return RegionManager.CanGoBack();
        }
        #endregion

        #region RefreshCommand property
        private DelegateCommand? _refreshCommand;
        public DelegateCommand RefreshCommand => _refreshCommand ??= new DelegateCommand(Refresh, CanRefresh);

        private void Refresh()
        {
            (RegionManager.DataContext as IRibbon)?.Refresh();
        }

        private bool CanRefresh()
        {
            return (RegionManager.DataContext as IRibbon)?.CanRefresh ?? false;
        }
        #endregion

        #region AddNewItemCommand property
        private DelegateCommand? _addNewItemCommand;
        public DelegateCommand AddNewItemCommand => _addNewItemCommand ??= new DelegateCommand(AddNewItem, CanAddNewItem);

        private void AddNewItem()
        {
            (RegionManager.DataContext as IRibbon)?.AddNewItem();
        }

        private bool CanAddNewItem()
        {
            return (RegionManager.DataContext as IRibbon)?.CanAddNewItem ?? false;
        }
        #endregion

        #region EditItemCommand property
        private DelegateCommand? _editItemCommand;
        public DelegateCommand EditItemCommand => _editItemCommand ??= new DelegateCommand(EditItem, CanEditItem);

        private void EditItem()
        {
            (RegionManager.DataContext as IRibbon)?.EditItem();
        }

        private bool CanEditItem()
        {
            return (RegionManager.DataContext as IRibbon)?.CanEditItem ?? false;
        }
        #endregion

        #region DeleteItemCommand property
        private DelegateCommand? _deleteItemCommand;
        public DelegateCommand DeleteItemCommand => _deleteItemCommand ??= new DelegateCommand(DeleteItem, CanDeleteItem);

        private void DeleteItem()
        {
            (RegionManager.DataContext as IRibbon)?.DeleteItem();
        }

        private bool CanDeleteItem()
        {
            return (RegionManager.DataContext as IRibbon)?.CanDeleteItem ?? false;
        }
        #endregion

        public MainWindowViewModel(IEventAggregator ea)
        {
            ea.GetEvent<SituationChangedEvent>().Subscribe(RefreshCommands);
            ea.GetEvent<OrdersActivatedEvent>().Subscribe(() => OrdersEnabled = true);
            ea.GetEvent<OrdersInactivatedEvent>().Subscribe(() => OrdersEnabled = false);
            ea.GetEvent<LogInEvent>().Subscribe(user => LogInUser = user);
            ea.GetEvent<LogOutEvent>().Subscribe(() => LogInUser = null);

            // 管理者として起動
            ea.GetEvent<LogInEvent>().Publish(new User { Account = new Account { IsAdmin = true } });
        }

        private void RefreshCommands()
        {
            GoBackCommand.RaiseCanExecuteChanged();
            RefreshCommand.RaiseCanExecuteChanged();
            AddNewItemCommand.RaiseCanExecuteChanged();
            EditItemCommand.RaiseCanExecuteChanged();
            DeleteItemCommand.RaiseCanExecuteChanged();
        }
    }
}
