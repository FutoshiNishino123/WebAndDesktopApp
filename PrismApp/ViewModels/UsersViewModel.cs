using Data;
using Data.Models;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismApp.Events;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using Unity;
using Prism.Commands;
using System;
using PrismApp.Regions;
using PrismApp.Data;

namespace PrismApp.ViewModels
{
    public class UsersViewModel : BindableBase, INavigationAware, IRibbon
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
            await UserRepository.UpdateAsync(user);
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
                        await UserRepository.DeleteAsync(User.Id);
                        Users?.Remove(User);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        #endregion

        private async void Initialize()
        {
            var users = await UserRepository.List();
            Users = new ObservableCollection<User>(users);
        }
    }
}