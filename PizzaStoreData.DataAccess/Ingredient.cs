using System;
using System.Collections.Generic;

namespace PizzaStoreData.DataAccess
{
    public partial class Ingredient
    {
        public Ingredient()
        {
            Inventory = new HashSet<Inventory>();
            Pizza = new HashSet<Pizza>();
        }

        public int IngredientId { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }

        public virtual ICollection<Inventory> Inventory { get; set; }
        public virtual ICollection<Pizza> Pizza { get; set; }
    }
}
