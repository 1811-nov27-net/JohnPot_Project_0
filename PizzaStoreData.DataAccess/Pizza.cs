using System;
using System.Collections.Generic;

namespace PizzaStoreData.DataAccess
{
    public partial class Pizza
    {
        public Pizza()
        {
            OrderPizzaId10Navigation = new HashSet<Order>();
            OrderPizzaId11Navigation = new HashSet<Order>();
            OrderPizzaId12Navigation = new HashSet<Order>();
            OrderPizzaId1Navigation = new HashSet<Order>();
            OrderPizzaId2Navigation = new HashSet<Order>();
            OrderPizzaId3Navigation = new HashSet<Order>();
            OrderPizzaId4Navigation = new HashSet<Order>();
            OrderPizzaId5Navigation = new HashSet<Order>();
            OrderPizzaId6Navigation = new HashSet<Order>();
            OrderPizzaId7Navigation = new HashSet<Order>();
            OrderPizzaId8Navigation = new HashSet<Order>();
            OrderPizzaId9Navigation = new HashSet<Order>();
        }

        public int PizzaId { get; set; }
        public int? IngredientId1 { get; set; }
        public int? IngredientId2 { get; set; }
        public int? IngredientId3 { get; set; }
        public int? IngredientId4 { get; set; }
        public int? IngredientId5 { get; set; }
        public int? IngredientId6 { get; set; }
        public int? IngredientId7 { get; set; }
        public int? IngredientId8 { get; set; }

        public virtual Ingredient IngredientId1Navigation { get; set; }
        public virtual Ingredient IngredientId2Navigation { get; set; }
        public virtual Ingredient IngredientId3Navigation { get; set; }
        public virtual Ingredient IngredientId4Navigation { get; set; }
        public virtual Ingredient IngredientId5Navigation { get; set; }
        public virtual Ingredient IngredientId6Navigation { get; set; }
        public virtual Ingredient IngredientId7Navigation { get; set; }
        public virtual Ingredient IngredientId8Navigation { get; set; }
        public virtual ICollection<Order> OrderPizzaId10Navigation { get; set; }
        public virtual ICollection<Order> OrderPizzaId11Navigation { get; set; }
        public virtual ICollection<Order> OrderPizzaId12Navigation { get; set; }
        public virtual ICollection<Order> OrderPizzaId1Navigation { get; set; }
        public virtual ICollection<Order> OrderPizzaId2Navigation { get; set; }
        public virtual ICollection<Order> OrderPizzaId3Navigation { get; set; }
        public virtual ICollection<Order> OrderPizzaId4Navigation { get; set; }
        public virtual ICollection<Order> OrderPizzaId5Navigation { get; set; }
        public virtual ICollection<Order> OrderPizzaId6Navigation { get; set; }
        public virtual ICollection<Order> OrderPizzaId7Navigation { get; set; }
        public virtual ICollection<Order> OrderPizzaId8Navigation { get; set; }
        public virtual ICollection<Order> OrderPizzaId9Navigation { get; set; }
    }
}
