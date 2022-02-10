using Data.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using PrismApp.Data;
using PrismApp.Events;
using PrismApp.Models;
using PrismApp.Regions;
using System;
using System.Windows;
using Unity;

namespace PrismApp.ViewModels
{
    public class StatusEditViewModel : BindableBase, INavigationAware
    {
        [Dependency]
        public IContentRegionManager Region { get; set; }

        [Dependency]
        public IEventPublisher Event { get; set; }

        #region Status property
        private BindableStatus? _status;
        public BindableStatus? Status
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
                if (status.Id == 0)
                {
                    await StatusRepository.InsertAsync(status);
                }
                else
                {
                    await StatusRepository.UpdateAsync(status);
                }
            }
            catch (Exception e)
            {
                while (e.InnerException != null) { e = e.InnerException; }
                MessageBox.Show(e.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Region.GoBack();
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
            Status = null;

            var status = id.HasValue ? await StatusRepository.GetByIdAsync(id.Value) : new Status();
            if (status is null)
            {
                MessageBox.Show("レコードが見つかりません", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Region.GoBack();
                return;
            }

            Status = BindableStatus.FromStatus(status);
        }
    }
}