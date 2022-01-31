using Data.Models;

namespace PrismApp.ViewModels
{
    public interface IEventPublisher
    {
        void RaiseSituationChanged();
        void RaiseGoBack();
        void RaiseLogIn(User user);
        void RaiseLogOut();
        void RaiseOrderFilterChanged(OrderFilter filter);
    }
}
