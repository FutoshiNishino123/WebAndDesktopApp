using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismApp.Models
{
    internal interface IAppAction
    {
        bool CanRefresh { get; }
        void Refresh();

        bool CanAddNewItem { get; }
        void AddNewItem();

        bool CanEditItem { get; }
        void EditItem();

        bool CanDeleteItem { get; }
        void DeleteItem();
    }
}
