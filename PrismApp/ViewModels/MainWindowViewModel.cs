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
        public IRegionManager RegionManager { get; set; }

        [Dependency]
        public IEventAggregator EventAggregator { get; set; }

        private object? ContentRegionDataContext
        {
            get
            {
                var view = RegionManager.Regions[RegionNames.ContentRegion].ActiveViews.FirstOrDefault();
                return (view as FrameworkElement)?.DataContext;
            }
        }

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

        #region UseOrdersFunction property
        private bool _useOrdersFunction;
        public bool UseOrdersFunction
        {
            get => _useOrdersFunction;
            set => SetProperty(ref _useOrdersFunction, value);
        }
        #endregion

        #region ShowClosed property
        private bool _showClosed;
        public bool ShowClosed
        {
            get => _showClosed;
            set
            {
                if (SetProperty(ref _showClosed, value))
                {
                    PublishOrderFilterChangedEvent();
                }
            }
        }
        #endregion

        #region NavigateCommand property
        private DelegateCommand<string>? _navigateCommand;
        public DelegateCommand<string> NavigateCommand => _navigateCommand ??= new DelegateCommand<string>(Navigate, CanNavigate);

        private void Navigate(string path)
        {
            RegionManager.RequestNavigate(RegionNames.ContentRegion, path);
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
            RegionManager.Regions[RegionNames.ContentRegion].NavigationService.Journal.GoBack();
        }

        private bool CanGoBack()
        {
            return RegionManager.Regions[RegionNames.ContentRegion].NavigationService.Journal.CanGoBack;
        }
        #endregion

        #region RefreshCommand property
        private DelegateCommand? _refreshCommand;
        public DelegateCommand RefreshCommand => _refreshCommand ??= new DelegateCommand(Refresh, CanRefresh);

        private void Refresh()
        {
            (ContentRegionDataContext as IRibbon)?.Refresh();
        }

        private bool CanRefresh()
        {
            return (ContentRegionDataContext as IRibbon)?.CanRefresh ?? false;
        }
        #endregion

        #region AddNewItemCommand property
        private DelegateCommand? _addNewItemCommand;
        public DelegateCommand AddNewItemCommand => _addNewItemCommand ??= new DelegateCommand(AddNewItem, CanAddNewItem);

        private void AddNewItem()
        {
            (ContentRegionDataContext as IRibbon)?.AddNewItem();
        }

        private bool CanAddNewItem()
        {
            return (ContentRegionDataContext as IRibbon)?.CanAddNewItem ?? false;
        }
        #endregion

        #region EditItemCommand property
        private DelegateCommand? _editItemCommand;
        public DelegateCommand EditItemCommand => _editItemCommand ??= new DelegateCommand(EditItem, CanEditItem);

        private void EditItem()
        {
            (ContentRegionDataContext as IRibbon)?.EditItem();
        }

        private bool CanEditItem()
        {
            return (ContentRegionDataContext as IRibbon)?.CanEditItem ?? false;
        }
        #endregion

        #region DeleteItemCommand property
        private DelegateCommand? _deleteItemCommand;
        public DelegateCommand DeleteItemCommand => _deleteItemCommand ??= new DelegateCommand(DeleteItem, CanDeleteItem);

        private void DeleteItem()
        {
            (ContentRegionDataContext as IRibbon)?.DeleteItem();
        }

        private bool CanDeleteItem()
        {
            return (ContentRegionDataContext as IRibbon)?.CanDeleteItem ?? false;
        }
        #endregion

        public MainWindowViewModel(IEventAggregator ea)
        {
            ea.GetEvent<GoBackEvent>().Subscribe(GoBack);
            ea.GetEvent<SituationChangedEvent>().Subscribe(RefreshCommands);
            ea.GetEvent<NavigatedToOrdersEvent>().Subscribe(() => UseOrdersFunction = true);
            ea.GetEvent<NavigatedFromOrdersEvent>().Subscribe(() => UseOrdersFunction = false);
            ea.GetEvent<LogInEvent>().Subscribe(user => LogInUser = user);
            ea.GetEvent<LogOutEvent>().Subscribe(() => LogInUser = null);

            // 管理者として起動
            //ea.GetEvent<LogInEvent>().Publish(new User { Account = new Account { IsAdmin = true } });   
        }

        private void PublishOrderFilterChangedEvent()
        {
            EventAggregator.GetEvent<OrderFilterChangedEvent>().Publish(new OrderFilter
            {
                ShowClosed = ShowClosed,
            });
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
