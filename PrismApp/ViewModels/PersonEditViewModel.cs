using Data;
using Data.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismApp.Models;
using PrismApp.Events;
using System.Diagnostics;
using System.Windows;
using Unity;

namespace PrismApp.ViewModels
{
    public class PersonEditViewModel : BindableBase, INavigationAware
    {
        [Dependency]
        public IEventAggregator? EventAggregator { get; set; }

        #region Person property
        private BindablePerson? _person;
        public BindablePerson? Person
        {
            get => _person;
            set
            {
                if (SetProperty(ref _person, value))
                {
                    PublishSituationChangedEvent();
                }
            }
        }
        #endregion

        #region SaveExecuted property
        private bool _saveExecuted;
        public bool SaveExecuted
        {
            get => _saveExecuted;
            set => SetProperty(ref _saveExecuted, value);
        }
        #endregion

        #region SaveCommand property
        private DelegateCommand? _saveCommand;
        public DelegateCommand SaveCommand => _saveCommand ??= new DelegateCommand(Save, CanSave)
            .ObservesProperty(() => Person)
            .ObservesProperty(() => Person.FirstName)
            .ObservesProperty(() => Person.FirstKana)
            .ObservesProperty(() => SaveExecuted);

        private async void Save()
        {
            SaveExecuted = true;
    
            if (Person != null)
            {
                var person = BindablePerson.ToPerson(Person);
                await Models.PeopleRepository.SavePersonAsync(person);
            }

            PublishGoBackEvent();
        }

        private bool CanSave()
        {
            return Person != null
                   && !string.IsNullOrEmpty(Person.FirstName)
                   && !string.IsNullOrEmpty(Person.FirstKana)
                   && !SaveExecuted;
        }
        #endregion

        private async void Initialize(int? id)
        {
            Person = null;
            SaveExecuted = false;

            var person = id.HasValue ? await Models.PeopleRepository.GetPersonAsync(id.Value) : new();
            if (person is null)
            {
                Debug.WriteLine("レコードが見つかりません");
                PublishGoBackEvent();
                return;
            }

            Person = BindablePerson.FromPerson(person);
        }

        private void PublishGoBackEvent()
        {
            EventAggregator?.GetEvent<GoBackEvent>().Publish();
        }

        private void PublishSituationChangedEvent()
        {
            EventAggregator?.GetEvent<SituationChangedEvent>().Publish();
        }

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var id = (int?)navigationContext.Parameters["id"];
            Initialize(id);
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