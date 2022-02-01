using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismApp.ViewModels.Events;
using PrismApp.Regions;
using System;
using Unity;

namespace PrismApp.ViewModels
{
    public class LogOutViewModel : BindableBase, INavigationAware
    {
        [Dependency]
        public IContentRegionManager RegionManager { get; set; }

        [Dependency]
        public IEventPublisher EventPublisher { get; set; }

        #region LogOutCommand property
        private DelegateCommand? _logOutCommand;
        public DelegateCommand LogOutCommand => _logOutCommand ??= new DelegateCommand(LogOut, CanLogOut);

        private void LogOut()
        {
            EventPublisher.RaiseLogOut();
            
            RegionManager.Navigate("Home");
        }

        private bool CanLogOut()
        {
            return true;
        }
        #endregion

        #region GoBackCommand property
        private DelegateCommand? _goBackCommand;
        public DelegateCommand GoBackCommand => _goBackCommand ??= new DelegateCommand(GoBack, CanGoBack);

        private void GoBack()
        {
            RegionManager.GoBack();
        }

        private bool CanGoBack()
        {
            return RegionManager.CanGoBack();
        }
        #endregion

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            EventPublisher.RaiseSituationChanged();
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