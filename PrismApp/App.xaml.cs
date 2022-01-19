using Prism.Ioc;
using Prism.Regions;
using Prism.Unity;
using PrismApp.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PrismApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell(Window shell)
        {
            base.InitializeShell(shell);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Home>();
            containerRegistry.RegisterForNavigation<Orders>();
            containerRegistry.RegisterForNavigation<OrderEdit>();
            containerRegistry.RegisterForNavigation<OrderDetail>();
            containerRegistry.RegisterForNavigation<Statuses>();
            containerRegistry.RegisterForNavigation<StatusEdit>();
            containerRegistry.RegisterForNavigation<People>();
            containerRegistry.RegisterForNavigation<PersonEdit>();
        }
    }
}
