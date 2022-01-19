using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismApp.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace PrismApp.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        [Dependency]
        public IRegionManager? RegionManager { get; set; }

        [Dependency]
        public IEventAggregator? EventAggregator { get; set; }

        private object? ContentRegionDataContext
        {
            get
            {
                var view = RegionManager?.Regions[RegionNames.ContentRegion].ActiveViews.FirstOrDefault();
                return (view as FrameworkElement)?.DataContext;
            }
        }

        #region Title property
        private string? title = "Demo";
        public string? Title { get => title; set => SetProperty(ref title, value); }
        #endregion

        #region HideClosedOrders property
        private bool hideClosedOrders = true;
        public bool HideClosedOrders
        {
            get => hideClosedOrders;
            set
            {
                if (SetProperty(ref hideClosedOrders, value))
                {
                    EventAggregator?.GetEvent<HideClosedOrdersEvent>().Publish(value);
                }
            }
        }
        #endregion

        #region NavigateCommand property
        private DelegateCommand<string>? navigateCommand;
        public DelegateCommand<string> NavigateCommand => navigateCommand ??= new(Navigate, CanNavigate);

        private void Navigate(string path)
        {
            RegionManager?.RequestNavigate(RegionNames.ContentRegion, path);
        }

        private bool CanNavigate(string path)
        {
            return true;
        }
        #endregion

        #region GoBackCommand property
        private DelegateCommand? goBackCommand;
        public DelegateCommand GoBackCommand => goBackCommand ??= new(GoBack, CanGoBack);

        private void GoBack()
        {
            RegionManager?.Regions[RegionNames.ContentRegion].NavigationService.Journal.GoBack();
        }

        private bool CanGoBack()
        {
            return RegionManager?.Regions[RegionNames.ContentRegion].NavigationService.Journal.CanGoBack ?? false;
        }
        #endregion

        #region RefreshCommand property
        private DelegateCommand? refreshCommand;
        public DelegateCommand RefreshCommand => refreshCommand ??= new(Refresh, CanRefresh);

        private void Refresh()
        {
            (ContentRegionDataContext as IRibbon)?.Refresh();
        }

        private bool CanRefresh()
        {
            return (ContentRegionDataContext as IRibbon)?.CanRefresh ?? false;
        }
        #endregion

        #region AddNewItemCommand property
        private DelegateCommand? addNewItemCommand;
        public DelegateCommand AddNewItemCommand => addNewItemCommand ??= new(AddNewItem, CanAddNewItem);

        private void AddNewItem()
        {
            (ContentRegionDataContext as IRibbon)?.AddNewItem();
        }

        private bool CanAddNewItem()
        {
            return (ContentRegionDataContext as IRibbon)?.CanAddNewItem ?? false;
        }
        #endregion

        #region EditItemCommand property
        private DelegateCommand? editItemCommand;
        public DelegateCommand EditItemCommand => editItemCommand ??= new(EditItem, CanEditItem);

        private void EditItem()
        {
            (ContentRegionDataContext as IRibbon)?.EditItem();
        }

        private bool CanEditItem()
        {
            return (ContentRegionDataContext as IRibbon)?.CanEditItem ?? false;
        }
        #endregion

        #region DeleteItemCommand property
        private DelegateCommand? deleteItemCommand;
        public DelegateCommand DeleteItemCommand => deleteItemCommand ??= new(DeleteItem, CanDeleteItem);

        private void DeleteItem()
        {
            (ContentRegionDataContext as IRibbon)?.DeleteItem();
        }

        private bool CanDeleteItem()
        {
            return (ContentRegionDataContext as IRibbon)?.CanDeleteItem ?? false;
        }
        #endregion

        public MainWindowViewModel(IEventAggregator ea)
        {
            ea.GetEvent<GoBackEvent>().Subscribe(GoBack);
            ea.GetEvent<SituationChangedEvent>().Subscribe(RefreshRibbon);
        }

        private void RefreshRibbon()
        {
            GoBackCommand.RaiseCanExecuteChanged();
            RefreshCommand.RaiseCanExecuteChanged();
            AddNewItemCommand.RaiseCanExecuteChanged();
            EditItemCommand.RaiseCanExecuteChanged();
            DeleteItemCommand.RaiseCanExecuteChanged();
        }
    }
}
