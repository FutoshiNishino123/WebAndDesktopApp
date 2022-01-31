using Data.Models;
using Prism.Events;
using PrismApp.Events;
using Unity;

namespace PrismApp.ViewModels
{
    public class EventPublisher : IEventPublisher
    {
        [Dependency]
        public IEventAggregator EventAggregator { get; set; }

        public void RaiseSituationChanged()
        {
            EventAggregator.GetEvent<SituationChangedEvent>().Publish();
        }

        public void RaiseGoBack()
        {
            EventAggregator.GetEvent<GoBackEvent>().Publish();
        }

        public void RaiseLogIn(User user)
        {
            EventAggregator.GetEvent<LogInEvent>().Publish(user);
        }

        public void RaiseLogOut()
        {
            EventAggregator.GetEvent<LogOutEvent>().Publish();
        }

        public void RaiseOrderFilterChanged(OrderFilter filter)
        {
            EventAggregator.GetEvent<OrderFilterChangedEvent>().Publish(filter);
        }
    }
}
