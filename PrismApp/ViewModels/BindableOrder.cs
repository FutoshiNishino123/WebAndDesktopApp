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

        #region Created property
        private DateTime? _created;
        public DateTime? Created
        {
            get => _created;
            set => SetProperty(ref _created, value);
        }
        #endregion

        #region Updated property
        private DateTime? _updated;
        public DateTime? Updated
        {
            get => _updated;
            set => SetProperty(ref _updated, value);
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

        #region Expiration property
        private DateTime? _expiration;
        public DateTime? Expiration
        {
            get => _expiration;
            set => SetProperty(ref _expiration, value);
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

        #region Mapper method
        public static BindableOrder FromOrder(Order order) => order.Copy<BindableOrder>();

        public static Order ToOrder(BindableOrder order) => order.Copy<Order>();
        #endregion
    }
}
