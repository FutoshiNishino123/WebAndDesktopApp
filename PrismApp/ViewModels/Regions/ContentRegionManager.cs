using Prism.Regions;
using PrismApp.ViewModels;
using System.Linq;
using System.Windows;
using Unity;

namespace PrismApp.Regions
{
    public interface IContentRegionManager
    {
        string RegionName { get; }

        /// <summary>
        /// Get the DataContext of the active view.
        /// </summary>
        object? DataContext { get; }

        bool CanGoBack();

        void GoBack();

        void Navigate(string path);

        void Navigate(string path, int id);
    }

    public class ContentRegionManager : IContentRegionManager
    {
        [Dependency]
        public IRegionManager RegionManager { get; set; }

        public string RegionName => RegionNames.ContentRegion;

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
