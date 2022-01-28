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
using System.Threading.Tasks;

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

        #region Account property
        private BindableAccount? _account;
        public BindableAccount? Account
        {
            get => _account;
            set => SetProperty(ref _account, value);
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
            .ObservesProperty(() => User.FirstName)
            .ObservesProperty(() => User.FirstKana)
            .ObservesProperty(() => Account.Id)
            .ObservesProperty(() => Account.RawPassword)
            .ObservesProperty(() => Account.Password)
            .ObservesProperty(() => SaveExecuted);

        private async void Save()
        {
            SaveExecuted = true;

            Debug.Assert(User != null);
            var user = BindableUser.ToUser(User);

            Debug.Assert(Account != null);
            var account = BindableAccount.ToAccount(Account);

            user.Account = account;

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
                   && Account != null
                   && !string.IsNullOrEmpty(User.FirstName)
                   && !string.IsNullOrEmpty(User.FirstKana)
                   && !string.IsNullOrEmpty(Account.Id)
                   && !string.IsNullOrEmpty(Account.Password)
                   && !string.IsNullOrEmpty(Account.RawPassword)
                   && !SaveExecuted;
        }
        #endregion

        private async void Initialize(int? id)
        {
            User = null;
            SaveExecuted = false;

            var user = id.HasValue ? await UsersRepository.FindUserWithAccountAsync(id.Value) : new User();
            if (user is null)
            {
                MessageBox.Show("レコードが見つかりません", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                PublishGoBackEvent();
                return;
            }

            User = BindableUser.FromUser(user);
            Account = BindableAccount.FromAccount(user.Account ?? new Account());
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