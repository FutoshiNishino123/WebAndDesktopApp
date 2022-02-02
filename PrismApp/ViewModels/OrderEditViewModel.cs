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
using PrismApp.Regions;

namespace PrismApp.ViewModels
{
    public class OrderEditViewModel : BindableBase, INavigationAware
    {
        [Dependency]
        public IContentRegionManager Region { get; set; }

        [Dependency]
        public IEventPublisher Event { get; set; }

        [Dependency]
        public AppData Data { get; set; }

        #region Order property
        private BindableOrder? _order;
        public BindableOrder? Order
        {
            get => _order;
            set
            {
                if (SetProperty(ref _order, value))
                {
                    Event.RaiseSituationChanged();
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
                    Event.RaiseSituationChanged();
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
                    Event.RaiseSituationChanged();
                }
            }
        }
        #endregion

        #region SaveCommand property
        private DelegateCommand? _saveCommand;
        public DelegateCommand SaveCommand => _saveCommand ??= new DelegateCommand(Save, CanSave)
            .ObservesProperty(() => Order)
            .ObservesProperty(() => Order.Number);

        private bool _saving;

        private void Save()
        {
            if (_saving) { return; }

            _saving = true;

            Save(true);

            _saving = false;
        }

        private async void Save(bool _)
        {
            if (Order != null)
            {
                var order = BindableOrder.ToOrder(Order);

                try
                {
                    await OrdersRepository.SaveAsync(order);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Region.GoBack();
            }
        }

        private bool CanSave()
        {
            return Order != null
                   && !string.IsNullOrEmpty(Order.Number);
        }
        #endregion

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Event.RaiseSituationChanged();

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

            var order = id.HasValue ? await OrdersRepository.FindAsync(id.Value) : new Order();
            if (order is null)
            {
                MessageBox.Show("レコードが見つかりません", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Region.GoBack();
                return;
            }
            
            var number = id.HasValue ? null : await OrderNumberGenerator.NextAsync();
            if (number is not null)
            {
                order.Number = number;
            }

            var users = await UsersRepository.GetAllAsync();
            var statuses = await StatusesRepository.GetAllAsync();

            if (order.User != null)
            {
                order.User = users.FirstOrDefault(u => u.Id == order.User.Id);
            }
            else if (Data.LogInUser != null)
            {
                order.User = users.FirstOrDefault(u => u.Id == Data.LogInUser.Id);
            }

            if (order.Status != null)
            {
                order.Status = statuses.FirstOrDefault(s => s.Id == order.Status.Id);
            }
            else
            {
                order.Status = statuses.FirstOrDefault();
            }

            Order = BindableOrder.FromOrder(order);
            Users = new ObservableCollection<User>(users);
            Statuses = new ObservableCollection<Status>(statuses);
        }
    }
}