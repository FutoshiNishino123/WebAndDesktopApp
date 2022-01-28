using Common.Utils;
using Data.Models;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using PrismApp.Models;
using System;
using System.Diagnostics;
using System.Windows;
using Unity;

namespace PrismApp.ViewModels
{
    public class LoginWindowViewModel : BindableBase
    {
        #region Title property
        private string? _title = "ログイン";
        public string? Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        #endregion

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

            User? loginUser;
            try
            {
                var hash = PasswordUtils.GetHashValue(Password);
                loginUser = await UsersRepository.FindUserAsync(UserId, hash);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (loginUser != null)
            {
                Debug.WriteLine(loginUser.Name);
            }
        }

        private bool CanLogin()
        {
            return !string.IsNullOrEmpty(UserId)
                   && !string.IsNullOrEmpty(Password);
        }
        #endregion
    }
}