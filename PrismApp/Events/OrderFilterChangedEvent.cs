using Data.Models;
using Prism.Events;
using PrismApp.ViewModels;
using System;

namespace PrismApp.Events
{
    internal class OrderFilterChangedEvent : PubSubEvent<OrderFilter>
    {
    }
}
