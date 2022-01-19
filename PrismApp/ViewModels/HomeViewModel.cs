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
        private string? description;
        public string? Description { get => description; set => SetProperty(ref description, value); }
        #endregion

        #region Version property
        private string? version;
        public string? Version { get => version; set => SetProperty(ref version, value); }
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
