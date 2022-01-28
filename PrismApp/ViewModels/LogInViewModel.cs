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

        #region UserId property
        private string? _userId;
        public string? UserId
        {
            get => _userId;
            set => SetProperty(ref _userId, value);
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

        #region LoginCommand property
        private DelegateCommand? _loginCommand;
        public DelegateCommand LoginCommand => _loginCommand ??= new DelegateCommand(Login, CanLogin)
            .ObservesProperty(() => UserId)
            .ObservesProperty(() => Password);

        private async void Login()
        {
            Debug.Assert(UserId != null);
            Debug.Assert(Password != null);

            User? user;
            try
            {
                var hash = PasswordUtils.GetHashValue(Password);
                user = await UsersRepository.FindUserAsync(UserId, hash);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (user == null)
            {
                MessageBox.Show("ユーザID または パスワード が正しくありません。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                MessageBox.Show("ログインしました。", "情報", MessageBoxButton.OK, MessageBoxImage.Information);
                RaiseLogInUser(user);
                GoBack();
            }
        }

        private bool CanLogin()
        {
            return !string.IsNullOrEmpty(UserId)
                   && !string.IsNullOrEmpty(Password);
        }
        #endregion

        private void RaiseLogInUser(User user)
        {
            EventAggregator.GetEvent<LogInEvent>().Publish(user);
        }

        private void GoBack()
        {
            EventAggregator.GetEvent<GoBackEvent>().Publish();
        }
    }
}