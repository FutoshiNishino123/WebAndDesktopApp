﻿using Common.Utils;
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
    public class LogInViewModel : BindableBase, INavigationAware
    {
        [Dependency]
        public IContentRegionManager RegionManager { get; set; }

        [Dependency]
        public IEventPublisher EventPublisher { get; set; }

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
            User? user = null;
            try
            {
                user = await FindUserAsync();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (user is null)
            {
                MessageBox.Show("ユーザID または パスワード が正しくありません。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("ログインしました。", "情報", MessageBoxButton.OK, MessageBoxImage.Information);

            EventPublisher.RaiseLogIn(user);

            RegionManager.GoToHome();
        }

        private bool CanLogIn()
        {
            return !string.IsNullOrEmpty(AccountId)
                   && !string.IsNullOrEmpty(Password);
        }
        #endregion

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            EventPublisher.RaiseSituationChanged();

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

        private void Initialize()
        {
            AccountId = null;
            Password = null;
        }

        private async Task<User?> FindUserAsync()
        {
            Debug.Assert(AccountId != null);
            Debug.Assert(Password != null);

            var hash = PasswordUtils.GetHash(Password);
            var user = await UsersRepository.FindUserAsync(AccountId, hash);

            return user;
        }
    }
}