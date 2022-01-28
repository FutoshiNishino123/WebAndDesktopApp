using Data.Models;
using Prism.Events;

namespace PrismApp.Events
{
    internal class LogInEvent : PubSubEvent<User>
    {
    }
}
