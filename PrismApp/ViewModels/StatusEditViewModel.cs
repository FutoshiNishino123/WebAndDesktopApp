using Data;
using Data.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismApp.Models;
using PrismApp.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Unity;

namespace PrismApp.ViewModels
{
    public class StatusEditViewModel : BindableBase, INavigationAware
    {
        [Dependency]
        public IEventAggregator? EventAggregator { get; set; }

        #region Status property
        private BindableStatus? _status;
        public BindableStatus? Status
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
            .ObservesProperty(() => Status)
            .ObservesProperty(() => Status.Text)
            .ObservesProperty(() => SaveExecuted);

        private async void Save()
        {
            SaveExecuted = true;

            if (Status != null)
            {
                var status = BindableStatus.ToStatus(Status);
                await Models.StatusesRepository.SaveStatusAsync(status);
            }

            PublishGoBackEvent();
        }

        private bool CanSave()
        {
            return Status != null
                   && Statuses != null
                   && !string.IsNullOrEmpty(Status.Text)
                   && Statuses.All(s => s.Text != Status.Text)
                   && !SaveExecuted;
        }
        #endregion

        private async void Initialize(int? id)
        {
            Status = null;
            Statuses = null;
            SaveExecuted = false;

            var status = id.HasValue ? await Models.StatusesRepository.GetStatusAsync(id.Value) : new();
            if (status is null)
            {
                MessageBox.Show("レコードが見つかりません", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                PublishGoBackEvent();
                return;
            }
            var statuses = await Models.StatusesRepository.GetStatusesAsync();

            Status = BindableStatus.FromStatus(status);
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