using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzaStoreLibrary.library
{
    // TODO: Maybe make this a struct?
    public class Pizza
    {
        #region Static Fields
        public static readonly float BasePizzaCost = 10.0f;
        // TODO: Pizza is currently coupled with 
        //  ingredients, is that ok? + Pricing is
        //  stored here... 
        public static Dictionary<string, float> IngredientList = new Dictionary<string, float>
        {
            { "pepperoni",  1.0f },
            { "olive",     1.0f },
            { "cheese",    0.50f },
            { "pineapple", 1.0f}
        };
        // TODO: Fix this. Use meta data? Adding to staic fields 
        //  section for now. Should probs be static.
        public const string InvalidIngredient = "Invalid Ingredient";
        #endregion

        #region Fields
        // List of all ingredients required to make 
        //  the pizza.
        private readonly List<string> _ingredients = new List<string>();
        #endregion

        #region Properties
        public List<string> Ingredients { get => _ingredients; }
        public float Cost
        {
            get => CalculatePizzaCost();
        }
        #endregion

        #region Constructors
        public Pizza() { }
        public Pizza(params string[] ingredients)
        {
            if (ingredients.Length == 0 || ingredients == null)
                return;
            
            // Validation check each ingredient using LINQ
            //  before adding it to the list of ingredients
            foreach (string item in ingredients.Where(i => i != null && 
                     ValidateIngredient(i) != InvalidIngredient))
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
        #endregion

        #region Methods
        public void AddToppingsToPizza(params string[] toppings)
        {
            foreach (string topping in toppings)
            {
                AddToppingToPizza(topping);
            }
        }
        // Determine if the pizza is a proper pizza
        //  (has ingredients)
        public static bool PizzaIsValid(Pizza pizza)
        {
            if (pizza.Ingredients.Count == 0)
                return false;

            return true;
        }
        // TODO: Serialize list of ingredients that 
        //  can be considered valid.
        // Method checks to see if ingredient 
        //  is on the list of ingredients 
        public static string ValidateIngredient(string ingredient)
        {
            if (ingredient.EndsWith("s"))
                ingredient = ingredient.Remove(ingredient.Length - 1);

            if (IngredientList.ContainsKey(ingredient.ToLower()))
                return ingredient.ToLower();

            return InvalidIngredient;
        }
        public void Display()
        {
            Console.Write("Pizza: ");
            for (int i = 0; i < Ingredients.Count; i++)
            {
                Console.Write(Ingredients[i]);
                if(i != Ingredients.Count - 1)
                    Console.Write(", ");
            }
            Console.WriteLine();
        }
        #endregion  

        #region Pizza Helper Functions
        private float CalculatePizzaCost()
        {
            float totalIngredientCost = 0.0f;
            foreach (string ingredient in Ingredients)
            {
                totalIngredientCost += IngredientList[ingredient];
            }
            return totalIngredientCost + BasePizzaCost;
        }
        private void AddToppingToPizza(string ingredient)
        {
            if (ValidateIngredient(ingredient) != InvalidIngredient)
                Ingredients.Add(ingredient.ToLower());
        }
        #endregion

    }
}