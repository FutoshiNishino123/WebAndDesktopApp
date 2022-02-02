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
using PrismApp.Regions;

namespace PrismApp.ViewModels
{
    public class UsersViewModel : BindableBase, INavigationAware, IAppAction
    {
        [Dependency]
        public IContentRegionManager Region { get; set; }

        [Dependency]
        public IEventPublisher Event { get; set; }

        #region User property
        private User? _user;
        public User? User
        {
            get => _user;
            set
            {
                if (SetProperty(ref _user, value))
                {
                    Event.RaiseSituationChanged();
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
                    Event.RaiseSituationChanged();
                }
            }
        }
        #endregion

        #region SaveCommand property
        private DelegateCommand<User>? _saveCommand;
        public DelegateCommand<User> SaveCommand => _saveCommand ??= new DelegateCommand<User>(Save, CanSave);

        private async void Save(User user)
        {
            await UsersRepository.SaveAsync(user);

            Event.RaiseSituationChanged();
        }

        private bool CanSave(User user)
        {
            return true;
        }
        #endregion

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Event.RaiseSituationChanged();

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
                Region.Navigate("UserEdit");
            }
        }

        public bool CanEditItem => User != null;
        public void EditItem()
        {
            if (CanEditItem)
            {
                Debug.Assert(User != null);
                Region.Navigate("UserEdit", User.Id);
            }
        }

        public bool CanDeleteItem => User != null;
        public async void DeleteItem()
        {
            if (CanDeleteItem)
            {
                if (MessageBox.Show("削除しますか？", "確認", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Debug.Assert(User != null);
                        await UsersRepository.DeleteAsync(User.Id);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Users?.Remove(User);
                }
            }
        }
        #endregion

        private async void Initialize()
        {
            var users = await UsersRepository.GetAllAsync();
            Users = new ObservableCollection<User>(users);
        }
    }
}