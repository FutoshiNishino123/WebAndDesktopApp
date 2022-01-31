namespace PrismApp.ViewModels
{
    public interface IContentRegionManager
    {
        /// <summary>
        /// Get the DataContext of the active view.
        /// </summary>
        object? DataContext { get; }

        bool CanGoBack();
        
        void GoBack();
        
        void Navigate(string path);

        void Navigate(string path, int id);
    }
}
