using Data.Models;
using Prism.Events;
using Prism.Mvvm;
using PrismApp.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismApp.Models
{
    public class AppData : BindableBase
    {
        #region LogInUser property
        private User? _logInUser;
        public User? LogInUser
        {
            get => _logInUser;
            set => SetProperty(ref _logInUser, value);
        }
        #endregion

        public AppData(IEventAggregator ea)
        {
            ea.GetEvent<LogInEvent>().Subscribe(user => LogInUser = user);
            ea.GetEvent<LogOutEvent>().Subscribe(() => LogInUser = null);

#if DEBUG
            // 管理者として起動
            LogInUser = new User 
            {
                Name = "ゲスト",
                Account = new Account 
                { 
                    IsAdmin = true,
                } 
            };
#endif
        }
    }
}
