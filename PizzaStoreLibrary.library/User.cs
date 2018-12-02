using PizzaStoreLibrary.library;
using System;
using System.Collections.Generic;

namespace PizzaStoreLibrary.library
{
    public class User
    {
        private Name _name;
        private Name _defaultLocation;

        public User(string[] names)
        {
            // If no name or only a first name was given
            //  reject the entry
            if (names == null || names.Length == 0 || names.Length == 1)
            {
                _name = new Name(Name.InvalidName, Name.InvalidName);
                return;
            }

            _name = new Name(names);

            // No location was provided
            _defaultLocation = new Name(Name.InvalidLocation);
        }

        public User(string[] name, string location)
        : this(name)
        {
            // Bad location checks
            if (location == null || location == "")
                _defaultLocation = new Name(Name.InvalidLocation);
            else
                _defaultLocation = new Name(location);
        }

        public Name FullName { get => _name; }
        public Name DefaultLocation { get => _defaultLocation; }

        public void SetDefaultLocation(string location)
        {
            // Validate location before setting it
            if (IsValidLocation(location))
                _defaultLocation.Location = location;

        }

        // TODO: Location validation.
        public bool IsValidLocation(string location)
        {
            if (location == "" || location == null || location.Length == 0)
                return false;

            return true;
        }
    }
}