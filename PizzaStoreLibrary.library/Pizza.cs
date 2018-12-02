using System.Collections.Generic;
using System.Linq;

namespace PizzaStoreLibrary.library
{
    // TODO: Maybe make this a struct?
    public class Pizza
    {
        public static string[] IngredientList = new string[]
        {
            "Peperoni",
            "Olive",
            "Cheese",
            "Pineapple"
        };

        private List<string> _ingredients = new List<string>();

        public List<string> Ingredients { get => _ingredients; set => _ingredients = value; }

        public Pizza(string[] ingredients)
        {
            if (ingredients.Length == 0)
                return;
            
            // Insure that all ingredients we are adding
            //  are valid. Using LINQ. (Because LINQ is awesome.)
            Ingredients.AddRange(ingredients.Where(i => i != null && ValidIngredient(i)));
        }
        public Pizza(List<string> ingredients)
        :this(ingredients.ToArray())
        {
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
        private bool ValidIngredient(string ingredient)
        {
            // Truncate s (plural form) from ingredient 
            //  incase user uses that form. Only storing
            //  singlual form of ingredients.
            if(ingredient.EndsWith("s"))
            {
                ingredient = ingredient.Remove(ingredient.Length - 1);
            }

            return IngredientList.Contains(ingredient);
        }
    }
}