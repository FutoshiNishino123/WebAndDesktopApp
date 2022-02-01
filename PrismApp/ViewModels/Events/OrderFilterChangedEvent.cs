using Data.Models;
using Prism.Events;
using PrismApp.Models;
using System;

namespace PrismApp.ViewModels.Events
{
    public class OrderFilterChangedEvent : PubSubEvent<OrderFilter>
    {
    }
}
