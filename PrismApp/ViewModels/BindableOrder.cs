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
        private int id;
        public int Id { get => id; set => SetProperty(ref id, value); }
        #endregion

        #region CreatedDate property
        private DateTime? createdDate;
        public DateTime? CreatedDate { get => createdDate; set => SetProperty(ref createdDate, value); }
        #endregion

        #region UpdatedDate property
        private DateTime? updatedDate;
        public DateTime? UpdatedDate { get => updatedDate; set => SetProperty(ref updatedDate, value); }
        #endregion

        #region Number property
        private string? number;
        public string? Number { get => number; set => SetProperty(ref number, value); }
        #endregion

        #region Person property
        private Person? person;
        public Person? Person { get => person; set => SetProperty(ref person, value); }
        #endregion

        #region Status property
        private Status? status;
        public Status? Status { get => status; set => SetProperty(ref status, value); }
        #endregion

        #region Remarks property
        private string? remarks;
        public string? Remarks { get => remarks; set => SetProperty(ref remarks, value); }
        #endregion

        #region ExpirationDate property
        private DateTime? expirationDate;
        public DateTime? ExpirationDate { get => expirationDate; set => SetProperty(ref expirationDate, value); }
        #endregion

        #region IsClosed property
        private bool isClosed;
        public bool IsClosed { get => isClosed; set => SetProperty(ref isClosed, value); }
        #endregion

        #region Conversion method
        public static BindableOrder FromOrder(Order order) => order.Copy<BindableOrder>();

        public static Order ToOrder(BindableOrder order) => order.Copy<Order>();
        #endregion
    }
}
