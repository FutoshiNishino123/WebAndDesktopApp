using Data;
using Common.Extensions;
using Data.Models;
using Prism.Mvvm;

namespace PrismApp.ViewModels
{
    public class BindableStatus : BindableBase
    {
        #region Id property
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        #endregion

        #region Text property
        private string? _text;
        public string? Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }
        #endregion

        #region Mapper method
        public static BindableStatus FromStatus(Status status) => status.Copy<BindableStatus>();

        public static Status ToStatus(BindableStatus status) => status.Copy<Status>();
        #endregion
    }
}
