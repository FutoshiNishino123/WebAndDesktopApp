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
using System;

namespace PrismApp.ViewModels
{
    public class StatusEditViewModel : BindableBase, INavigationAware
    {
        [Dependency]
        public IEventAggregator EventAggregator { get; set; }

        #region Status property
        private BindableStatus? _status;
        public BindableStatus? Status
        {
            get => _status;
            set
            {
                if (SetProperty(ref _status, value))
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

            Debug.Assert(Status != null);
            var status = BindableStatus.ToStatus(Status);

            try
            {
                await StatusesRepository.SaveStatusAsync(status);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                SaveExecuted = false;
                return;
            }

            GoBack();
        }

        private bool CanSave()
        {
            return Status != null
                   && !string.IsNullOrEmpty(Status.Text)
                   && !SaveExecuted;
        }
        #endregion

        private async void Initialize(int? id)
        {
            Status = null;
            SaveExecuted = false;

            var status = id.HasValue ? await StatusesRepository.FindStatusAsync(id.Value) : new Status();
            if (status is null)
            {
                MessageBox.Show("レコードが見つかりません", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                GoBack();
                return;
            }

            Status = BindableStatus.FromStatus(status);
        }

        private void GoBack()
        {
            EventAggregator.GetEvent<GoBackEvent>().Publish();
        }

        private void RaiseSituationChanged()
        {
            EventAggregator.GetEvent<SituationChangedEvent>().Publish();
        }

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
    }
}