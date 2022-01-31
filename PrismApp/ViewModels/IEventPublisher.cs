using Data.Models;

namespace PrismApp.ViewModels
{
    public interface IEventPublisher
    {
        void RaiseSituationChanged();

        void RaiseLogIn(User user);
        
        void RaiseLogOut();
        
        void RaiseOrdersInactivated();
        
        void RaiseOrdersActivated();

        void RaiseOrderFilterChanged(OrderFilter filter);
    }
}
