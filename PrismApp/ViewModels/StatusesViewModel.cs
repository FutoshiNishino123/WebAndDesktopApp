using Data;
using Data.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismApp.Events;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using Unity;
using System;
using PrismApp.Regions;
using PrismApp.Data;

namespace PrismApp.ViewModels
{
    public class StatusesViewModel : BindableBase, INavigationAware, IRibbon
    {
        [Dependency]
        public IContentRegionManager Region { get; set; }

        [Dependency]
        public IEventPublisher Event { get; set; }

        #region Status property
        private Status? _status;
        public Status? Status
        {
            get => _status;
            set
            {
                if (SetProperty(ref _status, value))
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

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Event.RaiseSituationChanged();

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
        public bool CanRefresh => Statuses != null;
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
                Region.Navigate("StatusEdit");
            }
        }

        public bool CanEditItem => Status != null;
        public void EditItem()
        {
            if (CanEditItem)
            {
                Debug.Assert(Status != null);
                Region.Navigate("StatusEdit", Status.Id);
            }
        }

        public bool CanDeleteItem => Status != null;
        public async void DeleteItem()
        {
            if (CanDeleteItem)
            {
                if (MessageBox.Show("削除しますか？", "確認", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Debug.Assert(Status != null);
                        await StatusesRepository.DeleteAsync(Status.Id);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Statuses?.Remove(Status);
                }
            }
        }
        #endregion

        private async void Initialize()
        {
            var statuses = await StatusesRepository.GetAllAsync();
            Statuses = new ObservableCollection<Status>(statuses);
        }
    }
}