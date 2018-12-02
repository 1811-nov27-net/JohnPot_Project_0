using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzaStoreLibrary.library
{
    // TODO: Maybe make this a struct?
    public class Pizza
    {
        // TODO: Pizza is currently coupled with 
        //  ingredients, is that ok?
        public static Dictionary<string, float> IngredientList = new Dictionary<string, float>
        {
            { "pepperoni",  1.0f },
            { "olive",     1.0f },
            { "cheese",    0.50f },
            { "pineapple", 1.0f}
        };

        public static readonly float BasePizzaCost = 10.0f;

        private List<string> _ingredients = new List<string>();

        public List<string> Ingredients { get => _ingredients; }
        public float Cost
        {
            get => CalculatePizzaCost();
        }

        public Pizza() { }

        public Pizza(string[] ingredients)
        {
            if (ingredients.Length == 0)
                return;
            
            // Validation check each ingredient using LINQ
            //  before adding it to the list of ingredients
            foreach (string item in ingredients.Where(i => i != null && ValidIngredient(i)))
            {
                string ingredient = item;

                if (ingredient.EndsWith("s"))
                     ingredient = ingredient.Remove(item.Length - 1);

                // Going with all lowercase ingredients
                //  standard for this project
                AddToppingToPizza(ingredient.ToLower());
            }
        }
        public Pizza(List<string> ingredients)
        : this(ingredients.ToArray())
        {
        }

        private void AddToppingToPizza(string ingredient)
        {
            if (ValidIngredient(ingredient))
                Ingredients.Add(ingredient.ToLower());
        }
        public void AddToppingsToPizza(params string[] toppings)
        {
            foreach (string topping in toppings)
            {
                AddToppingToPizza(topping);
            }
        }

        // Determine if the pizza is a proper pizza
        //  (has ingredients)
        public bool IsValid()
        {
            if (Ingredients.Count == 0)
                return false;

            return true;
        }

        // TODO: Serialize list of ingredients that 
        //  can be considered valid.
        // Method checks to see if ingredient 
        //  is on the list of ingredients 
        private bool ValidIngredient(string ingredient)
        {
            if (ingredient.EndsWith("s"))
                ingredient = ingredient.Remove(ingredient.Length - 1);

            return IngredientList.ContainsKey(ingredient.ToLower());
        }


        private float CalculatePizzaCost()
        {
            float totalIngredientCost = 0.0f;
            foreach (string ingredient in Ingredients)
            {
                totalIngredientCost += IngredientList[ingredient];
            }
            return totalIngredientCost + BasePizzaCost;
        }
    }
}