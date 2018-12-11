using System;
using System.Collections.Generic;

namespace PizzaStoreData.DataAccess
{
    public partial class Inventory
    {
        public int LocationId { get; set; }
        public int IngredientId { get; set; }
        public int? Count { get; set; }

        public virtual Ingredient Ingredient { get; set; }
        public virtual Location Location { get; set; }
    }
}
