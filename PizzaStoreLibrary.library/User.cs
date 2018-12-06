using PizzaStoreLibrary.library;
using System;
using System.Collections.Generic;

namespace PizzaStoreLibrary.library
{
    public class User
    {
        private readonly string[] _name = new string[2];
        private string _defaultLocation;

        public string FirstName { get => _name[0]; set => _name[0] = value; }
        public string LastName{ get => _name[1]; set => _name[1] = value; }
        public string[] FullName { get => _name; }
        public string DefaultLocation { get => _defaultLocation; }

        #region Constructors
        public User(params string[] names)
        {
            // If no name or only a first name was given
            //  reject the entry
            if (names == null || names.Length == 0 || names.Length == 1)
            {
                FirstName = Utilities.InvalidName;
                LastName  = Utilities.InvalidName;
                return;
            }

            FirstName = names[0];
            LastName = names[names.Length - 1];

            // No location was provided
            _defaultLocation = Utilities.InvalidLocation;
        }
        public User(string[] name, string location)
        : this(name)
        {
            // Bad location checks
            if (location == null || location == "")
                _defaultLocation = Utilities.InvalidLocation;
            else
                _defaultLocation = location;
        }
        #endregion

        public void SetDefaultLocation(string location)
        {
            // Validate location before setting it
            if (IsValidLocation(location))
                _defaultLocation = location;

        }

        // TODO: Location validation ?
        public bool IsValidLocation(string location)
        {
            if (location == "" || location == null || location.Length == 0)
                return false;

            return true;
        }
    }
}