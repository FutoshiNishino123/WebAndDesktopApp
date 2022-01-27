using Data;
using Data.Models;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismApp.Models;
using PrismApp.Events;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using Unity;
using Prism.Commands;
using System;

namespace PrismApp.ViewModels
{
    public class UsersViewModel : BindableBase, INavigationAware, IRibbon
    {
        [Dependency]
        public IRegionManager RegionManager { get; set; }

        [Dependency]
        public IEventAggregator EventAggregator { get; set; }

        #region User property
        private User? _user;
        public User? User
        {
            get => _user;
            set
            {
                if (SetProperty(ref _user, value))
                {
                    PublishSituationChangedEvent();
                }
            }
        }

        #endregion

        #region Users property
        private ObservableCollection<User>? _users;
        public ObservableCollection<User>? Users
        {
            get => _users;
            set
            {
                if (SetProperty(ref _users, value))
                {
                    PublishSituationChangedEvent();
                }
            }
        }
        #endregion

        #region SaveCommand property
        private DelegateCommand<User>? _saveCommand;
        public DelegateCommand<User> SaveCommand => _saveCommand ??= new DelegateCommand<User>(Save, CanSave);

        private async void Save(User user)
        {
            await UsersRepository.SaveUserAsync(user);
            PublishSituationChangedEvent();
        }

        private bool CanSave(User user)
        {
            return true;
        }
        #endregion

        private async void Initialize()
        {
            var users = await UsersRepository.GetUsersAsync();
            Users = new ObservableCollection<User>(users);
        }

        private void PublishSituationChangedEvent()
        {
            EventAggregator.GetEvent<SituationChangedEvent>().Publish();
        }

        private void NavigateToUserEdit(int? id)
        {
            var parameters = new NavigationParameters();
            parameters.Add("id", id);
            RegionManager.RequestNavigate(RegionNames.ContentRegion, "UserEdit", parameters);
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
        public bool CanRefresh => Users != null;
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
                NavigateToUserEdit(null);
            }
        }

        public bool CanEditItem => User != null;
        public void EditItem()
        {
            if (CanEditItem)
            {
                Debug.Assert(User != null);
                NavigateToUserEdit(User.Id);
            }
        }

        public bool CanDeleteItem => User != null;
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

            Debug.Assert(User != null);

            try
            {
                await UsersRepository.DeleteUserAsync(User.Id);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Users?.Remove(User);
        }
        #endregion
    }
}