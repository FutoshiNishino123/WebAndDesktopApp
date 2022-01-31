using Common.Utils;
using Data.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using PrismApp.Events;
using PrismApp.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace PrismApp.ViewModels
{
    public class LogInViewModel : BindableBase
    {
        [Dependency]
        public IRegionManager RegionManager { get; set; }

        [Dependency]
        public IEventAggregator EventAggregator { get; set; }

        #region AccountId property
        private string? _accountId;
        public string? AccountId
        {
            get => _accountId;
            set => SetProperty(ref _accountId, value);
        }
        #endregion

        #region Password property
        private string? _password;
        public string? Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        #endregion

        #region LogInCommand property
        private DelegateCommand? _logInCommand;
        public DelegateCommand LogInCommand => _logInCommand ??= new DelegateCommand(LogIn, CanLogIn)
            .ObservesProperty(() => AccountId)
            .ObservesProperty(() => Password);

        private async void LogIn()
        {
            var user = await FindUserAsync();
            if (user == null)
            {
                return;
            }

            RaiseLogIn(user);
            MessageBox.Show("ログインしました。", "情報", MessageBoxButton.OK, MessageBoxImage.Information);

            RaiseGoBack();
        }

        private bool CanLogIn()
        {
            return !string.IsNullOrEmpty(AccountId)
                   && !string.IsNullOrEmpty(Password);
        }
        #endregion

        private async Task<User?> FindUserAsync()
        {
            Debug.Assert(AccountId != null);
            Debug.Assert(Password != null);

            var hash = PasswordUtils.GetHash(Password);

            User? user = null;
            try
            {
                user = await UsersRepository.FindUserAsync(AccountId, hash);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            if (user == null)
            {
                MessageBox.Show("ユーザID または パスワード が正しくありません。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            return user;
        }

        private void RaiseLogIn(User user)
        {
            EventAggregator.GetEvent<LogInEvent>().Publish(user);
        }

        private void RaiseGoBack()
        {
            EventAggregator.GetEvent<GoBackEvent>().Publish();
        }
    }
}