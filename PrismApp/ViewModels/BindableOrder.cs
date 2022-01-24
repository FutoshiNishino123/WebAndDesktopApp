using Data;
using Common.Extensions;
using Data.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismApp.ViewModels
{
    public class BindableOrder : BindableBase
    {
        #region Id property
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        #endregion

        #region CreatedDate property
        private DateTime? _createdDate;
        public DateTime? CreatedDate
        {
            get => _createdDate;
            set => SetProperty(ref _createdDate, value);
        }
        #endregion

        #region UpdatedDate property
        private DateTime? _updatedDate;
        public DateTime? UpdatedDate
        {
            get => _updatedDate;
            set => SetProperty(ref _updatedDate, value);
        }
        #endregion

        #region Number property
        private string? _number;
        public string? Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }
        #endregion

        #region Person property
        private Person? _person;
        public Person? Person
        {
            get => _person;
            set => SetProperty(ref _person, value);
        }
        #endregion

        #region Status property
        private Status? _status;
        public Status? Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }
        #endregion

        #region Remarks property
        private string? _remarks;
        public string? Remarks
        {
            get => _remarks;
            set => SetProperty(ref _remarks, value);
        }
        #endregion

        #region ExpirationDate property
        private DateTime? _expirationDate;
        public DateTime? ExpirationDate
        {
            get => _expirationDate;
            set => SetProperty(ref _expirationDate, value);
        }
        #endregion

        #region IsClosed property
        private bool _isClosed;
        public bool IsClosed
        {
            get => _isClosed;
            set => SetProperty(ref _isClosed, value);
        }
        #endregion

        #region Conversion method
        public static BindableOrder FromOrder(Order order) => order.Copy<BindableOrder>();

        public static Order ToOrder(BindableOrder order) => order.Copy<Order>();
        #endregion
    }
}
