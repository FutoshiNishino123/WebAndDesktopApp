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

        private IRegionNavigationJournal Journal => RegionManager.Regions[RegionName].NavigationService.Journal;

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
            return Journal.CanGoBack;
        }

        public void GoBack()
        {
            Journal.GoBack();
        }

        public void Navigate(string path)
        {
            RegionManager.RequestNavigate(RegionName, path);
        }

        public void Navigate(string path, int id)
        {
            var parms = new NavigationParameters();
            parms.Add("id", id);
            RegionManager.RequestNavigate(RegionName, path, parms);
        }
    }
}
