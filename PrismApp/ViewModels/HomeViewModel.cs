using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismApp.Models;
using PrismApp.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Prism.Commands;
using System.Windows;
using PrismApp.Regions;
using PrismApp.Data;

namespace PrismApp.ViewModels
{
    internal class HomeViewModel : BindableBase, INavigationAware
    {
        [Dependency]
        public IContentRegionManager Region { get; set; }

        [Dependency]
        public IEventPublisher Event { get; set; }

        [Dependency]
        public AppFunction App { get; set; }

        #region Greeting property
        private static readonly string DefaultGreeting = "ようこそ";
        private string _greeting = DefaultGreeting;
        public string Greeting
        {
            get => _greeting;
            set => SetProperty(ref _greeting, value);
        }
        #endregion

        #region Description property
        private string? _description;
        public string? Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        #endregion

        #region Version property
        private string? _version;
        public string? Version
        {
            get => _version;
            set => SetProperty(ref _version, value);
        }
        #endregion

        #region LogInCommand property
        private DelegateCommand? _logInCommand;
        public DelegateCommand LogInCommand => _logInCommand ??= new DelegateCommand(LogIn, CanLogIn);

        private void LogIn()
        {
            Region.Navigate("LogIn");
        }

        private bool CanLogIn()
        {
            return true;
        }
        #endregion

        #region LogOutCommand property
        private DelegateCommand? _logOutCommand;
        public DelegateCommand LogOutCommand => _logOutCommand ??= new DelegateCommand(LogOut, CanLogOut);

        private void LogOut()
        {
            Region.Navigate("LogOut");
        }

        private bool CanLogOut()
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

        public async void Initialize()
        {
            var about = await AboutRepository.GetAboutAsync();
            Description = about.Description;
            Version = about.Version;
            Greeting = GetGreeting();
        }

        private string GetGreeting()
        {
            var user = App.User;
            if (user is null)
            {
                return "ようこそ";
            }
            else
            {
                var hour = DateTime.Now.Hour;
                var greeting = hour < 12 ? "おはようございます"
                               : hour < 18 ? "こんにちは"
                               : "こんばんは";

                return $"{greeting}、{user.Name} さん";
            }
        }
    }
}
