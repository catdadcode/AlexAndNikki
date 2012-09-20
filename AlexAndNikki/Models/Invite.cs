using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Text;

namespace AlexAndNikki.Models
{
    public partial class Invite : IDataErrorInfo
    {
        private Dictionary<string, string> _errors = new Dictionary<string, string>();

        partial void OnRelatedFamilyChanging(string value)
        {
            if (value.ToLower() != "ford" && value.ToLower() != "sommer")
                _errors.Add("RelatedFamily", "You must choose a related family.");
        }

        public string PrimaryContactNames()
        {
            var primaries = this.Guests.Where(x => x.PrimaryGuest).OrderBy(x => x.FirstName);
            switch (primaries.Count())
            {
                case 0:
                    StringBuilder guestNames = new StringBuilder();
                    for (int count = 0; count < this.Guests.Count; count++)
                    {
                        Guest guest = this.Guests.ToList()[count];
                        if (count == this.Guests.Count - 1)
                            guestNames.Append(", and ");
                        else if (count > 0)
                            guestNames.Append(", ");
                        guestNames.Append(guest.FirstName);
                    }
                    return guestNames.ToString();
                case 1:
                    return String.Format("{0} {1}", primaries.First().FirstName, primaries.First().LastName);
                case 2:
                    return String.Format("{0} and {1}", primaries.First().FirstName, primaries.Last().FirstName);
                default:
                    StringBuilder primaryGuestNames = new StringBuilder();
                    for (int count = 0; count < primaries.Count(); count++)
                    {
                        Guest guest = primaries.ToList()[count];
                        if (count == this.Guests.Count - 1)
                            primaryGuestNames.Append(", and ");
                        else if (count > 0)
                            primaryGuestNames.Append(", ");
                        primaryGuestNames.Append(guest.FirstName);
                    }
                    return primaryGuestNames.ToString();
            }
        }

        public string Error
        {
            get { return string.Empty; }
        }

        public string this[string columnName]
        {
            get
            {
                if (_errors.ContainsKey(columnName))
                    return _errors[columnName];
                return string.Empty;
            }
        }
    }
}