﻿using Data;
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
        public IEventAggregator EventAggregator { get; set; }

        #region Order property
        private BindableOrder? _order;
        public BindableOrder? Order
        {
            get => _order;
            set
            {
                if (SetProperty(ref _order, value))
                {
                    RaiseSituationChanged();
                }
            }
        }
        #endregion

        #region Users property
        private ObservableCollection<User>? _users;
        public ObservableCollection<User>? Users
        {
            get => _users;
            set
            {
                if (SetProperty(ref _users, value))
                {
                    RaiseSituationChanged();
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
                    RaiseSituationChanged();
                }
            }
        }
        #endregion

        #region SaveExecuted property
        private bool _saveExecuted;
        public bool SaveExecuted
        {
            get => _saveExecuted;
            set
            {
                if (SetProperty(ref _saveExecuted, value))
                {
                    RaiseSituationChanged();
                }
            }
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

            Debug.Assert(Order != null);
            var order = BindableOrder.ToOrder(Order);

            try
            {
                await OrdersRepository.SaveOrderAsync(order);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                SaveExecuted = false;
                return;
            }

            RaiseGoBack();
        }

        private bool CanSave()
        {
            return Order != null
                   && !string.IsNullOrEmpty(Order.Number)
                   && !SaveExecuted;
        }
        #endregion

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            RaiseSituationChanged();

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

        private async void Initialize(int? id)
        {
            Order = null;
            Users = null;
            Statuses = null;
            SaveExecuted = false;

            var order = id.HasValue ? await OrdersRepository.FindOrderAsync(id.Value) : new Order();
            if (order is null)
            {
                MessageBox.Show("レコードが見つかりません", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                RaiseGoBack();
                return;
            }

            var users = await UsersRepository.GetUsersAsync();
            var statuses = await StatusesRepository.GetStatusesAsync();

            if (order.User != null)
            {
                order.User = users.FirstOrDefault(p => p.Id == order.User.Id);
            }

            if (order.Status != null)
            {
                order.Status = statuses.FirstOrDefault(s => s.Id == order.Status.Id);
            }

            Order = BindableOrder.FromOrder(order);
            Users = new ObservableCollection<User>(users);
            Statuses = new ObservableCollection<Status>(statuses);
        }

        private void RaiseGoBack()
        {
            EventAggregator.GetEvent<GoBackEvent>().Publish();
        }

        private void RaiseSituationChanged()
        {
            EventAggregator.GetEvent<SituationChangedEvent>().Publish();
        }
    }
}