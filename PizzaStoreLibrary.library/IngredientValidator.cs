using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaStoreLibrary.library
{
    public static class IngredientValidator
    {
        // Name to cost pair
        public static Dictionary<string, float> Ingredients;

        static IngredientValidator()
        {
            Ingredients = new Dictionary<string, float>();
            // REMOVE: Use for testing purposes only
            PopulateWithTestIngredients();
        }

        // For testing purposes, don't require user to
        //  access the db for initial ingredients
        public static void PopulateWithTestIngredients()
        {
            Ingredients.Add("cheese", 0.5f);
            Ingredients.Add("pepperoni", 1.0f);
            Ingredients.Add("olive", 1.0f);
            Ingredients.Add("pineapple", 1.0f);
        }

        public static void AddIngredient(KeyValuePair<string, float> ingredient)
        {
            string ingredientName = ingredient.Key;

            if (ingredientName.EndsWith("s"))
                ingredientName = ingredientName.Remove(ingredientName.Length - 1);

            ingredientName = ingredientName.ToLower();

            if (Ingredients.ContainsKey(ingredientName))
            {
                Ingredients[ingredientName] += ingredient.Value;
            }
            else
            {
                Ingredients[ingredientName] = ingredient.Value;
            }
        }

        public static void AddIngredient(List<KeyValuePair<string, float>> ingredients)
        {
            foreach (var ingredient in ingredients)
            {
                AddIngredient(ingredient);
            }
        }

        public static bool IsValidIngredient(string ingredient)
        {
            string ingredientName = ingredient;

            if (ingredientName.EndsWith("s"))
                ingredientName = ingredientName.Remove(ingredientName.Length - 1);

            ingredientName = ingredientName.ToLower();

            return Ingredients.ContainsKey(ingredientName);
        }

        public static float GetIngredientCost(string ingredient)
        {
            if (IsValidIngredient(ingredient))
                return Ingredients[ingredient.ToLower()];

            return -1.0f;
        }
    }
}
