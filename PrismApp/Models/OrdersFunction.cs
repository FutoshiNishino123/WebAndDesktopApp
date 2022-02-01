using Data.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using PrismApp.Events;
using Unity;

namespace PrismApp.Models
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
            private set => SetProperty(ref _isEnabled, value);
        }
        #endregion

        #region Filter property
        private OrderFilter _filter = new OrderFilter();
        public OrderFilter Filter
        {
            get => _filter;
            private set => SetProperty(ref _filter, value);
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

        public OrdersFunction(IEventAggregator ea)
        {
            ea.GetEvent<OrdersActivatedEvent>().Subscribe(() => IsEnabled = true);
            ea.GetEvent<OrdersInactivatedEvent>().Subscribe(() => IsEnabled = false);
        }
    }
}
