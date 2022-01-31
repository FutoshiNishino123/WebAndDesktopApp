using Data.Models;
using Prism.Mvvm;

namespace PrismApp.ViewModels
{
    public class OrderFilter : BindableBase
    {
        #region ShowClosed property
        private bool _showClosed;
        public bool ShowClosed
        {
            get => _showClosed;
            set => SetProperty(ref _showClosed, value);
        }
        #endregion

        public bool Filter(Order order)
        {
            if (!ShowClosed && order.IsClosed)
            {
                return false;
            }

            return true;
        }
    }
}
