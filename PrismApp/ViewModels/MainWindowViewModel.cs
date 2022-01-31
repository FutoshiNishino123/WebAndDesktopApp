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
        public IContentRegionManager ContentRegionManager { get; set; }

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
                    var filter = new OrderFilter
                    {
                        ShowClosed = ShowClosed,
                    };
                    EventPublisher.RaiseOrderFilterChanged(filter);
                }
            }
        }
        #endregion

        #region NavigateCommand property
        private DelegateCommand<string>? _navigateCommand;
        public DelegateCommand<string> NavigateCommand => _navigateCommand ??= new DelegateCommand<string>(Navigate, CanNavigate);

        private void Navigate(string path)
        {
            ContentRegionManager.GoTo(path);
        }

        private bool CanNavigate(string path)
        {
            return true;
        }
        #endregion

        #region GoHomeCommand property
        private DelegateCommand? _goHomeCommand;
        public DelegateCommand GoHomeCommand => _goHomeCommand ??= new DelegateCommand(GoHome, CanGoHome);

        private void GoHome()
        {
            ContentRegionManager.GoToHome();
        }

        private bool CanGoHome()
        {
            return true;
        }
        #endregion

        #region GoBackCommand property
        private DelegateCommand? _goBackCommand;
        public DelegateCommand GoBackCommand => _goBackCommand ??= new DelegateCommand(GoBack, CanGoBack);

        private void GoBack()
        {
            ContentRegionManager.GoBack();
        }

        private bool CanGoBack()
        {
            return ContentRegionManager.CanGoBack();
        }
        #endregion

        #region RefreshCommand property
        private DelegateCommand? _refreshCommand;
        public DelegateCommand RefreshCommand => _refreshCommand ??= new DelegateCommand(Refresh, CanRefresh);

        private void Refresh()
        {
            (ContentRegionManager.DataContext as IRibbon)?.Refresh();
        }

        private bool CanRefresh()
        {
            return (ContentRegionManager.DataContext as IRibbon)?.CanRefresh ?? false;
        }
        #endregion

        #region AddNewItemCommand property
        private DelegateCommand? _addNewItemCommand;
        public DelegateCommand AddNewItemCommand => _addNewItemCommand ??= new DelegateCommand(AddNewItem, CanAddNewItem);

        private void AddNewItem()
        {
            (ContentRegionManager.DataContext as IRibbon)?.AddNewItem();
        }

        private bool CanAddNewItem()
        {
            return (ContentRegionManager.DataContext as IRibbon)?.CanAddNewItem ?? false;
        }
        #endregion

        #region EditItemCommand property
        private DelegateCommand? _editItemCommand;
        public DelegateCommand EditItemCommand => _editItemCommand ??= new DelegateCommand(EditItem, CanEditItem);

        private void EditItem()
        {
            (ContentRegionManager.DataContext as IRibbon)?.EditItem();
        }

        private bool CanEditItem()
        {
            return (ContentRegionManager.DataContext as IRibbon)?.CanEditItem ?? false;
        }
        #endregion

        #region DeleteItemCommand property
        private DelegateCommand? _deleteItemCommand;
        public DelegateCommand DeleteItemCommand => _deleteItemCommand ??= new DelegateCommand(DeleteItem, CanDeleteItem);

        private void DeleteItem()
        {
            (ContentRegionManager.DataContext as IRibbon)?.DeleteItem();
        }

        private bool CanDeleteItem()
        {
            return (ContentRegionManager.DataContext as IRibbon)?.CanDeleteItem ?? false;
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
