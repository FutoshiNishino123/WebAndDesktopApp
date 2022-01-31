using Prism.Regions;
using System.Linq;
using System.Windows;
using Unity;

namespace PrismApp.ViewModels
{
    public class ContentRegionManager : IContentRegionManager
    {
        private static readonly string RegionName = RegionNames.ContentRegion;

        [Dependency]
        public IRegionManager RegionManager { get; set; }

        public object? DataContext
        {
            get
            {
                var view = RegionManager.Regions[RegionName].ActiveViews.FirstOrDefault();
                return (view as FrameworkElement)?.DataContext;
            }
        }

        public bool CanGoBack()
        {
            return RegionManager.Regions[RegionName].NavigationService.Journal.CanGoBack;
        }

        public void GoBack()
        {
            RegionManager.Regions[RegionName].NavigationService.Journal.GoBack();
        }

        public void GoTo(string path)
        {
            RegionManager.RequestNavigate(RegionName, path);
        }

        public void GoToHome()
        {
            RegionManager.RequestNavigate(RegionName, "Home");
        }

        public void GoToLogIn()
        {
            RegionManager.RequestNavigate(RegionName, "LogIn");
        }

        public void GoToLogOut()
        {
            RegionManager.RequestNavigate(RegionName, "LogOut");
        }
    }
}
