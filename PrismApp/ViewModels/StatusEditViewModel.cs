using Data;
using Data.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismApp.Controllers;
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
        private BindableStatus? status;
        public BindableStatus? Status
        {
            get => status;
            set
            {
                if (SetProperty(ref status, value))
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
            .ObservesProperty(() => Status)
#pragma warning disable CS8602 // null 参照の可能性があるものの逆参照です。
            .ObservesProperty(() => Status.Text);
#pragma warning restore CS8602 // null 参照の可能性があるものの逆参照です。

        private async void Save()
        {
            SaveExecuted = true;

            if (Status != null)
            {
                await StatusController.SaveStatusAsync(BindableStatus.ToStatus(Status));
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

        #region SaveExecuted property
        private bool saveExecuted;
        public bool SaveExecuted { get => saveExecuted; set => SetProperty(ref saveExecuted, value); }
        #endregion

        private async void Initialize(int? id)
        {
            Status = null;
            Statuses = null;
            SaveExecuted = false;

            var status = id.HasValue ? await StatusController.GetStatusAsync(id.Value) : new();
            if (status is null)
            {
                MessageBox.Show("レコードが見つかりません", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                PublishGoBackEvent();
                return;
            }
            var statuses = await StatusController.GetStatusesAsync();

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