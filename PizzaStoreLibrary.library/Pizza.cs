using System.Collections.Generic;

namespace PizzaStoreLibrary.library
{
    public class Pizza
    {
        private List<string> _ingredients = new List<string>();

        public List<string> Ingredients { get => _ingredients; set => _ingredients = value; }

        public Pizza(string[] ingredients)
        {
            Ingredients.AddRange(ingredients);
        }
        public Pizza(List<string> ingredients)
        {
            _ingredients.AddRange(ingredients);
        }
    }
}