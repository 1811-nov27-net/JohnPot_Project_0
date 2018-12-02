using PizzaStoreLibrary.library;
using System.Collections.Generic;

namespace PizzaStoreLibrary.library
{
    public class Location
    {
        // Name of ingredient to number in inventory
        private Dictionary<string, int> _inventory = new Dictionary<string, int>();

        private Name _name;

        public Name Name { get => _name; set => _name = value; }

        // Leaving out the setter to force the user
        //  to use the stock inventory method
        public Dictionary<string, int> Inventory { get => _inventory; }

        public Location(string name)
        {
            if (name == null || name == "")
                _name = new Name(Name.InvalidLocation);
            else
                _name = new Name(name);
        }

        public Location(string name, KeyValuePair<string, int>[] ingredients)
        : this(name)
        {
            foreach (var ingredient in ingredients)
            {
                StockInventory(ingredient);
            }
        }

        public void StockInventory(KeyValuePair<string, int> ingredient)
        {
            if (_inventory.ContainsKey(ingredient.Key))
            {
                _inventory[ingredient.Key] += ingredient.Value;
            }
            else
            {
                _inventory.Add(ingredient.Key, ingredient.Value);
            }
        }

        // Overriding Equality operator to check if values
        //  are equal instead of checking same instance
        //public override bool Equals(object obj)
        //{
        //    if (obj == null || !this.GetType().Equals(obj.GetType()))
        //        return false;

        //    Location rhs = (Location)obj;

        //    return rhs.Name == Name;
        //}

    }
}