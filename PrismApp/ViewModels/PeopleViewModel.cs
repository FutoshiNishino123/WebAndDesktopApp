using Data;
using Data.Models;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismApp.Controllers;
using PrismApp.Events;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using Unity;

namespace PrismApp.ViewModels
{
    public class PeopleViewModel : BindableBase, INavigationAware, IRibbon
    {
        [Dependency]
        public IRegionManager? RegionManager { get; set; }

        [Dependency]
        public IEventAggregator? EventAggregator { get; set; }

        #region Person property
        private Person? person;
        public Person? Person
        {
            get => person;
            set
            {
                if (SetProperty(ref person, value))
                {
                    PublishSituationChangedEvent();
                }
            }
        }

        #endregion

        #region People property
        private ObservableCollection<Person>? people;
        public ObservableCollection<Person>? People
        {
            get => people;
            set
            {
                if (SetProperty(ref people, value))
                {
                    PublishSituationChangedEvent();
                }
            }
        }
        #endregion

        private async void Initialize()
        {
            var people = await PersonController.GetPeopleAsync();
            People = new ObservableCollection<Person>(people);
        }

        private void PublishSituationChangedEvent()
        {
            EventAggregator?.GetEvent<SituationChangedEvent>().Publish();
        }

        private void NavigateToPersonEdit(int? id)
        {
            var parameters = new NavigationParameters();
            parameters.Add("id", id);
            RegionManager?.RequestNavigate(RegionNames.ContentRegion, "PersonEdit", parameters);
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

        #region IRibbon
        public bool CanRefresh => true;
        public void Refresh()
        {
            if (CanRefresh)
            {
                Initialize();
            }
        }

        public bool CanAddNewItem => true;
        public void AddNewItem()
        {
            if (CanAddNewItem)
            {
                NavigateToPersonEdit(null);
            }
        }

        public bool CanEditItem => Person != null;
        public void EditItem()
        {
            if (CanEditItem)
            {
                NavigateToPersonEdit(Person?.Id);
            }
        }

        public bool CanDeleteItem => Person != null;
        public async void DeleteItem()
        {
            if (CanDeleteItem)
            {
                if (MessageBox.Show("削除しますか？", "確認", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Debug.Assert(Person != null);
                    await PersonController.DeletePersonAsync(Person.Id);
                    People?.Remove(Person);
                }
            }
        }
        #endregion
    }
}