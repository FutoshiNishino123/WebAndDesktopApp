namespace PrismApp.ViewModels
{
    public interface IContentRegionManager
    {
        /// <summary>
        /// Get the DataContext of the first view of ActiveViews.
        /// </summary>
        object? DataContext { get; }
        bool CanGoBack();
        void GoBack();
        void GoTo(string path);
        void GoToHome();
        void GoToLogIn();
        void GoToLogOut();
    }
}
