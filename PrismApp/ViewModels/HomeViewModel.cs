using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismApp.Controllers;
using PrismApp.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace PrismApp.ViewModels
{
    internal class HomeViewModel : BindableBase, INavigationAware
    {
        [Dependency]
        public IEventAggregator? EventAggregator { get; set; }

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

        public async void Initialize()
        {
            var about = await AboutController.GetAboutAsync();
            Description = about.Description;
            Version = about.Version;

            PublishSituationChangedEvent();
        }

        private void PublishSituationChangedEvent()
        {
            EventAggregator?.GetEvent<SituationChangedEvent>().Publish();
        }

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
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
