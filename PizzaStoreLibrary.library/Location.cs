using PizzaStoreLibrary.library;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzaStoreLibrary.library
{
    public class Location
    {
        // Name of ingredient to number in inventory
        private Dictionary<string, int> _inventory = new Dictionary<string, int>();

        private Name _name;
        private Stack<Order> _orderHistory = new Stack<Order>();

        public Name Name { get => _name; set => _name = value; }

        // Leaving out the setter to force the user
        //  to use the stock inventory method
        public Dictionary<string, int> Inventory { get => _inventory; }
        public Stack<Order> OrderHistory { get => _orderHistory; }

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
            // Use validation to remove 's' and insure
            //  the ingredient is a valid choice, of course.
            string validatedIngredient = Pizza.ValidateIngredient(ingredient.Key);

            if (Inventory.ContainsKey(validatedIngredient))
            {
                Inventory[ingredient.Key] += ingredient.Value;
            }
            else
            {
                if(validatedIngredient != Pizza.InvalidIngredient)
                    Inventory.Add(validatedIngredient, ingredient.Value);
            }
        }
        public void StockInventory(Dictionary<string, int> ingredientList)
        {
            foreach (var ingredient in ingredientList)
            {
                StockInventory(ingredient);
            }
        }

        private void DepleteInventory(KeyValuePair<string, int> ingredient)
        {
            string validatedIngredient = Pizza.ValidateIngredient(ingredient.Key);

            if (Inventory.ContainsKey(validatedIngredient))
                Inventory[validatedIngredient] -= ingredient.Value;
        }

        // Returns whether or not the order was 
        //  successfully placed
        public bool PlaceOrder(Order order)
        {
            // Create a dictionary of all ingredients
            //  required to make this order
            Dictionary<string, int> ingredientList = new Dictionary<string, int>();
            foreach (Pizza pizza in order.PizzaList)
            {
                foreach (string ingredient in pizza.Ingredients)
                {
                    // The pizza class should already preform 
                    //  ingredient validation for me. I'm going 
                    //  to assume all ingredients are valid.
                    if (ingredientList.ContainsKey(ingredient))
                        ingredientList[ingredient]++;
                    else
                        ingredientList.Add(ingredient, 1);
                }
            }

            if (!CheckInventoryForIngredientList(ingredientList))
                return false;

            // Finialize the order by depleting ingredients
            //  from the inventory and setting the time
            //  the order was placed.
            foreach (KeyValuePair<string,int> ingredient in ingredientList)
            {
                DepleteInventory(ingredient);
            }

            order.OrderTime = DateTime.Now.TimeOfDay;
            OrderHistory.Push(order);

            return true;
        }
        

        private bool CheckInventoryForIngredientList(Dictionary<string, int> ingredientList)
        {
            foreach (var ingredient in ingredientList)
            {
                if (!CheckInventoryForIngredient(ingredient))
                    return false;
            }

            return true;
        }
        private bool CheckInventoryForIngredient(KeyValuePair<string, int> ingredient)
        {
            return CheckInventoryForIngredient(ingredient.Key, ingredient.Value);
        }
        private bool CheckInventoryForIngredient(string ingredient, int quantity)
        {
            string validatedIngredient = Pizza.ValidateIngredient(ingredient);

            if (validatedIngredient != Pizza.InvalidIngredient && 
                Inventory.ContainsKey(ingredient))
                if (Inventory[ingredient] >= quantity)
                    return true;

            return false;
        }

        public Order GetLastOrder(User user)
        {
            return OrderHistory.FirstOrDefault(o => o.User == user);
        }
        public Order GetLastOrder()
        {
            if (OrderHistory.Count > 0)
                return OrderHistory.Peek();
            return null;
        }
    }
}