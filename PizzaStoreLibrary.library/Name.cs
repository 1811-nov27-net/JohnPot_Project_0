using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaStoreLibrary.library
{
    public class Name
    {
        public const string InvalidName     = "Invalid Name";
        public const string InvalidLocation = "Invalid Location";

        private string[] FullName = new string[2] { null, null };

        public string FirstName { get => FullName[0]; set => FullName[0] = value; }
        public string LastName  { get => FullName[1];  set => FullName[1] = value; }

        // Locations will have an empty last name... wasting memory here.
        public string Location { get => FirstName; set => FirstName = value; }

        /***** Constructors *****/
        public Name(string name)
        {
            FirstName = name;
        }

        public Name(string firstName, string lastName)
        :this(firstName)
        {
            LastName = lastName;
        }

        public Name()
        {
        }

        public Name(string[] names)
        {
            if (names.Length > 0)
                FirstName = names[0];
            if (names.Length > 1)
                LastName = names[names.Length - 1];
            
        }

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
