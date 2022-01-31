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
                    RaiseSituationChanged();
                }
            }
        }
        #endregion

        #region Account property
        private BindableAccount? _account;
        public BindableAccount? Account
        {
            get => _account;
            set
            {
                if (SetProperty(ref _account, value))
                {
                    RaiseSituationChanged();
                }
            }
        }
        #endregion

        #region SaveExecuted property
        private bool _saveExecuted;
        public bool SaveExecuted
        {
            get => _saveExecuted;
            set
            {
                if (SetProperty(ref _saveExecuted, value))
                {
                    RaiseSituationChanged();
                }
            }
        }
        #endregion

        #region SaveCommand property
        private DelegateCommand? _saveCommand;
        public DelegateCommand SaveCommand => _saveCommand ??= new DelegateCommand(Save, CanSave)
            .ObservesProperty(() => User.FirstName)
            .ObservesProperty(() => User.FirstKana)
            .ObservesProperty(() => Account.Id)
            .ObservesProperty(() => Account.RawPassword)
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

            RaiseGoBack();
        }

        private bool CanSave()
        {
            return !SaveExecuted
                   && User != null
                   && Account != null
                   && !string.IsNullOrEmpty(User.FirstName)
                   && !string.IsNullOrEmpty(User.FirstKana)
                   && !string.IsNullOrEmpty(Account.Id)
                   && !string.IsNullOrEmpty(Account.RawPassword);
        }
        #endregion

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            RaiseSituationChanged();

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

        private async void Initialize(int? id)
        {
            User = null;
            SaveExecuted = false;

            var user = id.HasValue ? await UsersRepository.FindUserAsync(id.Value) : new User();
            if (user is null)
            {
                MessageBox.Show("レコードが見つかりません", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                RaiseGoBack();
                return;
            }

            User = BindableUser.FromUser(user);
            Account = BindableAccount.FromAccount(user.Account ?? new Account());
        }

        private void RaiseGoBack()
        {
            EventAggregator.GetEvent<GoBackEvent>().Publish();
        }

        private void RaiseSituationChanged()
        {
            EventAggregator.GetEvent<SituationChangedEvent>().Publish();
        }
    }
}