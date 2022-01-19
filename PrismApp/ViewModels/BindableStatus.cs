using Data;
using Common.Extensions;
using Data.Models;
using Prism.Mvvm;

namespace PrismApp.ViewModels
{
    public class BindableStatus : BindableBase
    {
        #region Id property
        private int id;
        public int Id { get => id; set => SetProperty(ref id, value); }
        #endregion

        #region Text property
        private string? text;
        public string? Text { get => text; set => SetProperty(ref text, value); }
        #endregion

        #region Conversion method
        public static BindableStatus FromStatus(Status status) => status.Copy<BindableStatus>();

        public static Status ToStatus(BindableStatus status) => status.Copy<Status>();
        #endregion
    }
}
