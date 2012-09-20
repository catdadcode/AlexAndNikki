using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace AlexAndNikki.Models
{
    public partial class Guest : IDataErrorInfo
    {
        private Dictionary<string, string> _errors = new Dictionary<string, string>();

        public string FullName()
        {
            return String.Format("{0}, {1}", this.LastName, this.FirstName);
        }

        partial void OnStateChanging(string value)
        {
            if (value.ToLower() == "none")
                _errors.Add("State", "Please choose a state.");
        }

        partial void OnFirstNameChanging(string value)
        {
            if (String.IsNullOrEmpty(value))
                _errors.Add("FirstName", "First Name is required.");
        }

        partial void OnRelationshipChanging(string value)
        {
            if (value.ToLower() == "none")
                _errors.Add("Relationship", "Please choose.");
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