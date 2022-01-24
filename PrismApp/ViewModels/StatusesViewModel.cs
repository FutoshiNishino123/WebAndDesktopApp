using Data;
using Data.Models;
using Prism.Commands;
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
    public class StatusesViewModel : BindableBase, INavigationAware, IRibbon
    {
        [Dependency]
        public IRegionManager? RegionManager { get; set; }

        [Dependency]
        public IEventAggregator? EventAggregator { get; set; }

        #region Status property
        private Status? _status;
        public Status? Status
        {
            get => _status;
            set
            {
                if (SetProperty(ref _status, value))
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

        private async void Initialize()
        {
            var statuses = await StatusController.GetStatusesAsync();
            Statuses = new ObservableCollection<Status>(statuses);
        }

        private void PublishSituationChangedEvent()
        {
            EventAggregator?.GetEvent<SituationChangedEvent>().Publish();
        }

        private void NavigateToStatusEdit(int? id)
        {
            var parameters = new NavigationParameters();
            parameters.Add("id", id);
            RegionManager?.RequestNavigate(RegionNames.ContentRegion, "StatusEdit", parameters);
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
                NavigateToStatusEdit(null);
            }
        }

        public bool CanEditItem => Status != null;
        public void EditItem()
        {
            if (CanEditItem)
            {
                Debug.Assert(Status != null);
                NavigateToStatusEdit(Status.Id);
            }
        }

        public bool CanDeleteItem => Status != null;
        public async void DeleteItem()
        {
            if (CanDeleteItem)
            {
                if (MessageBox.Show("削除しますか？", "確認", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Debug.Assert(Status != null);
                    await StatusController.DeleteStatusAsync(Status.Id);
                    Statuses?.Remove(Status);
                }
            }
        }
        #endregion
    }
}