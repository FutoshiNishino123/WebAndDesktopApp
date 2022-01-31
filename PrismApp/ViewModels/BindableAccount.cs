using Common.Extensions;
using Data.Models;
using Prism.Mvvm;
using Common.Utils;

namespace PrismApp.ViewModels
{
    public class BindableAccount : BindableBase
    {
        #region Id property
        private string? _id;
        public string? Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
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
                    Password = value is null ? null : PasswordUtils.GetHash(value);
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
