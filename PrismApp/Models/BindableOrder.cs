using Data.Extensions;
using Data.Models;
using Prism.Mvvm;
using System;

namespace PrismApp.Models
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
        private DateTime _created;
        public DateTime Created
        {
            get => _created;
            set => SetProperty(ref _created, value);
        }
        #endregion

        #region Updated property
        private DateTime _updated;
        public DateTime Updated
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

        #region User property
        private User? _user;
        public User? User
        {
            get => _user;
            set => SetProperty(ref _user, value);
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

        #region IsActive property
        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }
        #endregion

        #region Mapper method
        public static BindableOrder FromOrder(Order order) => order.Map<BindableOrder>();

        public static Order ToOrder(BindableOrder order) => order.Map<Order>();
        #endregion
    }
}
