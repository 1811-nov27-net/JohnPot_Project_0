using System;
using System.Collections.Generic;

namespace PizzaStoreData.DataAccess
{
    public partial class Ingredient
    {
        public Ingredient()
        {
            PizzaIngredientId1Navigation = new HashSet<Pizza>();
            PizzaIngredientId2Navigation = new HashSet<Pizza>();
            PizzaIngredientId3Navigation = new HashSet<Pizza>();
            PizzaIngredientId4Navigation = new HashSet<Pizza>();
            PizzaIngredientId5Navigation = new HashSet<Pizza>();
            PizzaIngredientId6Navigation = new HashSet<Pizza>();
            PizzaIngredientId7Navigation = new HashSet<Pizza>();
            PizzaIngredientId8Navigation = new HashSet<Pizza>();
        }

        public int IngredientId { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }

        public virtual ICollection<Pizza> PizzaIngredientId1Navigation { get; set; }
        public virtual ICollection<Pizza> PizzaIngredientId2Navigation { get; set; }
        public virtual ICollection<Pizza> PizzaIngredientId3Navigation { get; set; }
        public virtual ICollection<Pizza> PizzaIngredientId4Navigation { get; set; }
        public virtual ICollection<Pizza> PizzaIngredientId5Navigation { get; set; }
        public virtual ICollection<Pizza> PizzaIngredientId6Navigation { get; set; }
        public virtual ICollection<Pizza> PizzaIngredientId7Navigation { get; set; }
        public virtual ICollection<Pizza> PizzaIngredientId8Navigation { get; set; }
    }
}
