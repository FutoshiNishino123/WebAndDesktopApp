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
        public IContentRegionManager RegionManager { get; set; }

        [Dependency]
        public IEventPublisher EventPublisher { get; set; }

        #region Status property
        private BindableStatus? _status;
        public BindableStatus? Status
        {
            get => _status;
            set
            {
                if (SetProperty(ref _status, value))
                {
                    EventPublisher.RaiseSituationChanged();
                }
            }
        }
        #endregion

        #region SaveCommand property
        private DelegateCommand? _saveCommand;
        public DelegateCommand SaveCommand => _saveCommand ??= new DelegateCommand(Save, CanSave)
            .ObservesProperty(() => Status)
            .ObservesProperty(() => Status.Text);

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
            if (Status == null) { throw new InvalidOperationException(); }

            var status = BindableStatus.ToStatus(Status);

            try
            {
                await StatusesRepository.SaveStatusAsync(status);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) { e = e.InnerException; }
                MessageBox.Show(e.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            RegionManager.GoBack();
        }

        private bool CanSave()
        {
            return Status != null
                   && !string.IsNullOrEmpty(Status.Text);
        }
        #endregion

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            EventPublisher.RaiseSituationChanged();

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
            Status = null;

            var status = id.HasValue ? await StatusesRepository.FindStatusAsync(id.Value) : new Status();
            if (status is null)
            {
                MessageBox.Show("レコードが見つかりません", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                RegionManager.GoBack();
                return;
            }

            Status = BindableStatus.FromStatus(status);
        }
    }
}