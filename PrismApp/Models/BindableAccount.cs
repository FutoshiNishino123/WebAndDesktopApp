using Common.Utils;
using Data.Extensions;
using Data.Models;
using Prism.Mvvm;

namespace PrismApp.Models
{
    public class BindableAccount : BindableBase
    {
        #region Id property
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        #endregion

        #region Name property
        private string? _name;
        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        #endregion

        #region Password property
        private string? _password;
        public string? Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        #endregion

        #region RawPassword property
        private string? _rawPassword;
        public string? RawPassword
        {
            get => _rawPassword;
            set
            {
                if (SetProperty(ref _rawPassword, value))
                {
                    Password = string.IsNullOrEmpty(value) ? null : PasswordUtils.GetHash(value);
                }
            }
        }
        #endregion

        #region IsAdmin property
        private bool _isAdmin;
        public bool IsAdmin
        {
            get => _isAdmin;
            set => SetProperty(ref _isAdmin, value);
        }
        #endregion

        #region Mapper method
        public static BindableAccount FromAccount(Account account) => account.Map<BindableAccount>();

        public static Account ToAccount(BindableAccount account) => account.Map<Account>();
        #endregion
    }
}
