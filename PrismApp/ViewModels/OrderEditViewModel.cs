using Data;
using Data.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismApp.Controllers;
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
        private BindableOrder? order;
        public BindableOrder? Order
        {
            get => order;
            set
            {
                if (SetProperty(ref order, value))
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

        #region Statuses property
        private ObservableCollection<Status>? statuses;
        public ObservableCollection<Status>? Statuses
        {
            get => statuses;
            set
            {
                if (SetProperty(ref statuses, value))
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
            .ObservesProperty(() => Order)
#pragma warning disable CS8602 // null 参照の可能性があるものの逆参照です。
            .ObservesProperty(() => Order.Number);
#pragma warning restore CS8602 // null 参照の可能性があるものの逆参照です。

        private async void Save()
        {
            SaveExecuted = true;

            if (Order != null)
            {
                await OrderController.SaveOrderAsync(BindableOrder.ToOrder(Order));
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

        #region SaveExecuted property
        private bool saveExecuted;
        public bool SaveExecuted { get => saveExecuted; set => SetProperty(ref saveExecuted, value); }
        #endregion

        private async void Initialize(int? id)
        {
            Order = null;
            People = null;
            Statuses = null;
            SaveExecuted = false;

            var order = id.HasValue ? await OrderController.GetOrderAsync(id.Value) : new();
            if (order is null)
            {
                MessageBox.Show("レコードが見つかりません", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                PublishGoBackEvent();
                return;
            }
            var people = await PersonController.GetPeopleAsync();
            var statuses = await StatusController.GetStatusesAsync();
            order.Person = people.FirstOrDefault(p => p.Id == order.Person?.Id);
            order.Status = statuses.FirstOrDefault(s => s.Id == order.Status?.Id);

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