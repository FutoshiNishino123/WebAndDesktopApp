using Data;
using Data.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismApp.Models;
using PrismApp.Events;
using System.Diagnostics;
using System.Windows;
using Unity;
using Common.Utils;
using System;

namespace PrismApp.ViewModels
{
    public class UserEditViewModel : BindableBase, INavigationAware
    {
        [Dependency]
        public IEventAggregator EventAggregator { get; set; }

        #region User property
        private BindableUser? _user;
        public BindableUser? User
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

        #region SaveExecuted property
        private bool _saveExecuted;
        public bool SaveExecuted
        {
            get => _saveExecuted;
            set => SetProperty(ref _saveExecuted, value);
        }
        #endregion

        #region SaveCommand property
        private DelegateCommand? _saveCommand;
        public DelegateCommand SaveCommand => _saveCommand ??= new DelegateCommand(Save, CanSave)
            .ObservesProperty(() => User)
            .ObservesProperty(() => User.EmailAddress)
            .ObservesProperty(() => User.Password)
            .ObservesProperty(() => User.FirstName)
            .ObservesProperty(() => User.FirstKana)
            .ObservesProperty(() => SaveExecuted);

        private async void Save()
        {
            SaveExecuted = true;

            Debug.Assert(User != null);
            var user = BindableUser.ToUser(User);

            try
            {
                await UsersRepository.SaveUserAsync(user);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                SaveExecuted = false;
                return;
            }

            PublishGoBackEvent();
        }

        private bool CanSave()
        {
            return User != null
                   && !string.IsNullOrEmpty(User.EmailAddress)
                   && !string.IsNullOrEmpty(User.PasswordRaw)
                   && !string.IsNullOrEmpty(User.FirstName)
                   && !string.IsNullOrEmpty(User.FirstKana)
                   && !SaveExecuted;
        }
        #endregion

        private async void Initialize(int? id)
        {
            User = null;
            SaveExecuted = false;

            var user = id.HasValue ? await UsersRepository.FindUserAsync(id.Value) : new User();
            if (user is null)
            {
                MessageBox.Show("レコードが見つかりません", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                PublishGoBackEvent();
                return;
            }

            User = BindableUser.FromUser(user);
        }

        private void PublishGoBackEvent()
        {
            EventAggregator.GetEvent<GoBackEvent>().Publish();
        }

        private void PublishSituationChangedEvent()
        {
            EventAggregator.GetEvent<SituationChangedEvent>().Publish();
        }

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var id = (int?)navigationContext.Parameters["id"];
            Initialize(id);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
        #endregion
    }
}