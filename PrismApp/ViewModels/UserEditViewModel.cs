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
using PrismApp.Regions;
using PrismApp.Data;

namespace PrismApp.ViewModels
{
    public class UserEditViewModel : BindableBase, INavigationAware
    {
        [Dependency]
        public IContentRegionManager Region { get; set; }

        [Dependency]
        public IEventPublisher Event { get; set; }

        #region User property
        private BindableUser? _user;
        public BindableUser? User
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

        #region Account property
        private BindableAccount? _account;
        public BindableAccount? Account
        {
            get => _account;
            set
            {
                if (SetProperty(ref _account, value))
                {
                    Event.RaiseSituationChanged();
                }
            }
        }
        #endregion

        #region SaveCommand property
        private DelegateCommand? _saveCommand;
        public DelegateCommand SaveCommand => _saveCommand ??= new DelegateCommand(Save, CanSave)
            .ObservesProperty(() => User.LastName)
            .ObservesProperty(() => User.LastKana)
            .ObservesProperty(() => Account.Name)
            .ObservesProperty(() => Account.Password);

        private bool _saving;

        private void Save()
        {
            if (_saving) { return; }

            _saving = true;

            Save(true);

            _saving = false;
        }

        private async void Save(bool _)
        {
            Debug.Assert(User != null);
            var user = BindableUser.ToUser(User);

            Debug.Assert(Account != null);
            var account = BindableAccount.ToAccount(Account);

            user.Account = account;

            try
            {
                if (user.Id == 0)
                {
                    await UserRepository.InsertAsync(user);
                }
                else
                {
                    await UserRepository.UpdateAsync(user);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Region.GoBack();
        }

        private bool CanSave()
        {
            return User != null
                   && Account != null
                   && !string.IsNullOrEmpty(User.LastName)
                   && !string.IsNullOrEmpty(User.LastKana)
                   && !string.IsNullOrEmpty(Account.Name)
                   && !string.IsNullOrEmpty(Account.Password);
        }
        #endregion

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Event.RaiseSituationChanged();

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
            Account = null;

            var user = id.HasValue ? await UserRepository.GetById(id.Value) : new User();
            if (user is null)
            {
                MessageBox.Show("レコードが見つかりません", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Region.GoBack();
                return;
            }

            User = BindableUser.FromUser(user);
            Account = BindableAccount.FromAccount(user.Account ?? new Account());
        }
    }
}