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
        #region NameRegex property
        private static Regex? nameRegex;
        public static Regex NameRegex => nameRegex ??= new Regex(@"^(?<last_name>\w+)\s+(?<first_name>\w+)$");
        #endregion

        #region Id property
        private int id;
        public int Id { get => id; set => SetProperty(ref id, value); }
        #endregion

        #region FirstName property
        private string? firstName;
        public string? FirstName { get => firstName; set => SetProperty(ref firstName, value); }
        #endregion

        #region LastName property
        private string? lastName;
        public string? LastName { get => lastName; set => SetProperty(ref lastName, value); }
        #endregion

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

        #region FirstKana property
        private string? firstKana;
        public string? FirstKana { get => firstKana; set => SetProperty(ref firstKana, value); }
        #endregion

        #region LastKana property
        private string? lastKana;
        public string? LastKana { get => lastKana; set => SetProperty(ref lastKana, value); }
        #endregion

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

                var match = NameRegex.Match(value);
                if (match.Success)
                {
                    LastKana = match.Groups["last"].Value;
                    FirstKana = match.Groups["first"].Value;
                }
                else
                {
                    FirstKana = value;
                }
            }
        }

        #region Gender property
        private Gender gender;
        public Gender Gender { get => gender; set => SetProperty(ref gender, value); }
        #endregion

        #region ImageUrl property
        private string? imageUrl;
        public string? ImageUrl { get => imageUrl; set => SetProperty(ref imageUrl, value); }
        #endregion

        #region Conversion method
        public static BindablePerson FromPerson(Person person) => person.Copy<BindablePerson>();

        public static Person ToPerson(BindablePerson person) => person.Copy<Person>();
        #endregion
    }
}
