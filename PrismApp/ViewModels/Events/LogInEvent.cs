using Data.Models;
using Prism.Events;

namespace PrismApp.ViewModels.Events
{
    public class LogInEvent : PubSubEvent<User>
    {
    }
}
