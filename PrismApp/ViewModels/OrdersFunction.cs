using Data.Models;
using Prism.Commands;
using Prism.Mvvm;
using PrismApp.Events;
using PrismApp.Models;
using Unity;

namespace PrismApp.ViewModels
{
    public class OrdersFunction : BindableBase
    {
        [Dependency]
        public IEventPublisher EventPublisher { get; set; }

        #region IsEnabled property
        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
        #endregion

        #region Filter property
        private OrderFilter _filter = new OrderFilter();
        public OrderFilter Filter
        {
            get => _filter;
            set => SetProperty(ref _filter, value);
        }
        #endregion

        #region FilterChangedCommand property
        private DelegateCommand? _filterChangedCommand;
        public DelegateCommand FilterChangedCommand => _filterChangedCommand ??= new DelegateCommand(FilterChanged, CanFilterChanged);

        private void FilterChanged()
        {
            EventPublisher.RaiseOrderFilterChanged(Filter);
        }

        private bool CanFilterChanged()
        {
            return true;
        }
        #endregion
    }
}
