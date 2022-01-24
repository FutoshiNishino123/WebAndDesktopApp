using Data;
using Common.Extensions;
using Data.Models;
using Prism.Mvvm;
using System.Linq;
using System.Text.RegularExpressions;

namespace PrismApp.ViewModels
{
    public class BindablePerson : BindableBase
    {
        #region Id property
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        #endregion

        #region FirstName property
        private string? _firstName;
        public string? FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }
        #endregion

        #region LastName property
        private string? _lastName;
        public string? LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }
        #endregion

        #region NameRegex property
        private static Regex? _nameRegex;
        public static Regex NameRegex => _nameRegex ??= new Regex(@"^(?<last_name>\w+)\s+(?<first_name>\w+)$");
        #endregion

        #region Name property
        public string? Name
        {
            get
            {
                return $"{LastName} {FirstName}".Trim();
            }
            set
            {
                if (value is null)
                {
                    LastName = FirstName = null;
                    return;
                }

                var match = NameRegex.Match(value);
                if (match.Success)
                {
                    LastName = match.Groups["last_name"].Value;
                    FirstName = match.Groups["first_name"].Value;
                }
                else
                {
                    FirstName = value;
                }
            }
        }
        #endregion

        #region FirstKana property
        private string? _firstKana;
        public string? FirstKana
        {
            get => _firstKana;
            set => SetProperty(ref _firstKana, value);
        }
        #endregion

        #region LastKana property
        private string? _lastKana;
        public string? LastKana
        {
            get => _lastKana;
            set => SetProperty(ref _lastKana, value);
        }
        #endregion

        #region KanaRegex property
        private static Regex? _kanaRegex;
        public static Regex KanaRegex => _kanaRegex ??= new Regex(@"^(?<last_kana>\w+)\s+(?<first_kana>\w+)$");
        #endregion

        #region Kana property
        public string? Kana
        {
            get
            {
                return $"{LastKana} {FirstKana}".Trim();
            }
            set
            {
                if (value is null)
                {
                    LastKana = FirstKana = null;
                    return;
                }

                var match = KanaRegex.Match(value);
                if (match.Success)
                {
                    LastKana = match.Groups["last_kana"].Value;
                    FirstKana = match.Groups["first_kana"].Value;
                }
                else
                {
                    FirstKana = value;
                }
            }
        }
        #endregion

        #region Gender property
        private Gender _gender;
        public Gender Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }
        #endregion

        #region ImageUrl property
        private string? _imageUrl;
        public string? ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }
        #endregion

        #region Conversion method
        public static BindablePerson FromPerson(Person person) => person.Copy<BindablePerson>();

        public static Person ToPerson(BindablePerson person) => person.Copy<Person>();
        #endregion
    }
}
