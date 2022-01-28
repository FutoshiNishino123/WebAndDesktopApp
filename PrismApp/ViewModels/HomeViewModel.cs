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

namespace PrismApp.ViewModels
{
    internal class HomeViewModel : BindableBase, INavigationAware
    {
        [Dependency]
        public IRegionManager RegionManager { get; set; }

        [Dependency]
        public IEventAggregator EventAggregator { get; set; }

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

        #region NavigateCommand property
        private DelegateCommand<string>? _navigateCommand;
        public DelegateCommand<string> NavigateCommand => _navigateCommand ??= new DelegateCommand<string>(Navigate, CanNavigate);

        private void Navigate(string path)
        {
            RegionManager.RequestNavigate(RegionNames.ContentRegion, path);
        }

        private bool CanNavigate(string path)
        {
            return true;
        }
        #endregion

        public async void Initialize()
        {
            var about = await AboutRepository.GetAboutAsync();
            Description = about.Description;
            Version = about.Version;
        }

        private void RaiseSituationChanged()
        {
            EventAggregator.GetEvent<SituationChangedEvent>().Publish();
        }

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            RaiseSituationChanged();

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
    }
}
