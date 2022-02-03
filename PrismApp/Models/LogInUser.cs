using Data.Models;
using Prism.Events;
using Prism.Mvvm;
using PrismApp.Events;

namespace PrismApp.Models
{
    public class LogInUser : BindableBase
    {
        #region User property
        private User? _user;
        public User? User
        {
            get => _user;
            private set => SetProperty(ref _user, value);
        }
        #endregion

        public LogInUser(IEventAggregator ea)
        {
            ea.GetEvent<LogInEvent>().Subscribe(LogIn);
            ea.GetEvent<LogOutEvent>().Subscribe(LogOut);

#if DEBUG
            // デバッグ時はログインしなくても管理者権限を付与
            User = new User 
            {
                Name = "ゲスト",
                Account = new Account 
                { 
                    IsAdmin = true,
                } 
            };
#endif
        }

        public void LogIn(User user)
        {
            User = user;
        }

        public void LogOut()
        {
            User = null;
        }
    }
}
