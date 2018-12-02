using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaStoreLibrary.library
{
    // TODO: Maybe make this an interface?
    public class Name
    {
        // Consts for invalid user input
        public const string InvalidName = "Invalid Name";
        public const string InvalidLocation = "Invalid Location";

        // Array to hold up to two user provided names, first / last
        private string[] FullName = new string[2] { null, null };

        // Properties to access FullName
        public string FirstName { get => FullName[0]; set => FullName[0] = value; }
        public string LastName  { get => FullName[1];  set => FullName[1] = value; }

        // Locations will have an empty last name... wasting memory here.

        // I want to be able to access the name element through
        //  a .Location prop instead of .FirstName for Store
        //  locations
        public string Location { get => FirstName; set => FirstName = value; }

        /***** Constructors *****/
        public Name(){}
        public Name(string name)
        {
            FirstName = name;
        }
        public Name(string firstName, string lastName)
        :this(firstName)
        {
            LastName = lastName;
        }
        public Name(string[] names)
        {
            if (names.Length > 0)
                FirstName = names[0];
            // Only store first and last names
            if (names.Length > 1)
                LastName = names[names.Length - 1];
            
        }

        /***** Conversions *****/ 
        // Conversion to string array
        public static implicit operator string[](Name name)
        {
           return name.FullName;
        }

        // Conversion from string array
        public static implicit operator Name(string[] names)
        {
            return new Name(names);
        }
    }
}
