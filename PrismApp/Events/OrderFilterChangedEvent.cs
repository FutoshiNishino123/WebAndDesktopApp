using Data.Models;
using Prism.Events;
using PrismApp.ViewModels;
using System;

namespace PrismApp.Events
{
    public class OrderFilterChangedEvent : PubSubEvent<OrderFilter>
    {
    }
}
