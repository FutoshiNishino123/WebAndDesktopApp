using Data.Models;
using Prism.Events;

namespace PrismApp.Events
{
    public class LogInEvent : PubSubEvent<User>
    {
    }
}
