using Data;
using Data.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismApp.Controllers;
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
        private BindablePerson? person;
        public BindablePerson? Person
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

        #region SaveCommand property
        private DelegateCommand? saveCommand;
        public DelegateCommand SaveCommand => saveCommand ??= new DelegateCommand(Save, CanSave)
            .ObservesProperty(() => SaveExecuted)
            .ObservesProperty(() => Person)
            .ObservesProperty(() => Person.FirstName)
            .ObservesProperty(() => Person.FirstKana);

        private async void Save()
        {
            SaveExecuted = true;
    
            if (Person != null)
            {
                await PersonController.SavePersonAsync(BindablePerson.ToPerson(Person));
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

        #region SaveExecuted property
        private bool saveExecuted;
        public bool SaveExecuted { get => saveExecuted; set => SetProperty(ref saveExecuted, value); }
        #endregion

        private async void Initialize(int? id)
        {
            Person = null;
            SaveExecuted = false;

            var person = id.HasValue ? await PersonController.GetPersonAsync(id.Value) : new();
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