using Data;
using Data.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismApp.Models;
using PrismApp.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Unity;

namespace PrismApp.ViewModels
{
    public class OrderEditViewModel : BindableBase, INavigationAware
    {
        [Dependency]
        public IEventAggregator? EventAggregator { get; set; }

        #region Order property
        private BindableOrder? _order;
        public BindableOrder? Order
        {
            get => _order;
            set
            {
                if (SetProperty(ref _order, value))
                {
                    PublishSituationChangedEvent();
                }
            }
        }
        #endregion

        #region People property
        private ObservableCollection<Person>? _people;
        public ObservableCollection<Person>? People
        {
            get => _people;
            set
            {
                if (SetProperty(ref _people, value))
                {
                    PublishSituationChangedEvent();
                }
            }
        }
        #endregion

        #region Statuses property
        private ObservableCollection<Status>? _statuses;
        public ObservableCollection<Status>? Statuses
        {
            get => _statuses;
            set
            {
                if (SetProperty(ref _statuses, value))
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
            .ObservesProperty(() => Order)
            .ObservesProperty(() => Order.Number)
            .ObservesProperty(() => SaveExecuted);

        private async void Save()
        {
            SaveExecuted = true;

            if (Order != null)
            {
                var order = BindableOrder.ToOrder(Order);
                await Models.OrdersRepository.SaveOrderAsync(order);
            }

            PublishGoBackEvent();
        }

        private bool CanSave()
        {
            return Order != null
                   && !string.IsNullOrEmpty(Order.Number)
                   && !SaveExecuted;
        }
        #endregion

        private async void Initialize(int? id)
        {
            Order = null;
            People = null;
            Statuses = null;
            SaveExecuted = false;

            var order = id.HasValue ? await Models.OrdersRepository.GetOrderAsync(id.Value) : new();
            if (order is null)
            {
                MessageBox.Show("レコードが見つかりません", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                PublishGoBackEvent();
                return;
            }
            var people = await Models.PeopleRepository.GetPeopleAsync();
            var statuses = await Models.StatusesRepository.GetStatusesAsync();

            if (order.Person != null)
            {
                order.Person = people.FirstOrDefault(p => p.Id == order.Person.Id);
            }

            if (order.Status != null)
            {
                order.Status = statuses.FirstOrDefault(s => s.Id == order.Status.Id);
            }

            Order = BindableOrder.FromOrder(order);
            People = new ObservableCollection<Person>(people);
            Statuses = new ObservableCollection<Status>(statuses);
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