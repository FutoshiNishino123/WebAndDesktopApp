using Data.Models;
using Prism.Events;
using Prism.Mvvm;
using PrismApp.Events;
namespace PrismApp.Models
{
    public class AppFunction : BindableBase
    {
        #region User property
        private User? _user;
        public User? User
        {
            get => _user;
            private set => SetProperty(ref _user, value);
        }
        #endregion

        public AppFunction(IEventAggregator ea)
        {
            ea.GetEvent<LogInEvent>().Subscribe(LogIn);
            ea.GetEvent<LogOutEvent>().Subscribe(LogOut);

#if DEBUG
            // デバッグ時は管理者権限
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
