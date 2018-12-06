using PizzaStoreLibrary.library;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzaStoreLibrary.library
{
    public class Location
    {
        #region Fields
        // Name of ingredient to number in inventory
        private readonly Dictionary<string, int> _inventory = new Dictionary<string, int>();
        // Hold a stack of all placed orders to this Location. Top of the stack 
        //  was placed most recently
        private readonly Stack<Order> _orderHistory = new Stack<Order>();
        private string _name;
        #endregion

        #region Properties
        public string Name { get => _name; set => _name = value; }
        // Leaving out the setter to force the user
        //  to use the stock inventory method
        public Dictionary<string, int> Inventory { get => _inventory; }
        public Stack<Order> OrderHistory { get => _orderHistory; }
        #endregion

        #region Constructors
        public Location(string name)
        {
            if (name == null || name == "")
                _name = Utilities.InvalidLocation;
            else
                _name = name;
        }
        public Location(string name, KeyValuePair<string, int>[] ingredients)
        : this(name)
        {
            foreach (var ingredient in ingredients)
            {
                StockInventory(ingredient);
            }
        }
        #endregion

        #region Methods
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
        public bool PlaceOrder(Order order)
        {
            // Stop user from ordering from the same
            //  location before two hours has elapsed
            Order pastOrder = GetLastOrder(order.User);
            if(pastOrder != null)
            {
                TimeSpan currentTime = DateTime.Now.TimeOfDay;
                TimeSpan elapsedTime = currentTime - pastOrder.OrderTime;
                if (elapsedTime.Hours < 2)
                    return false;
            }


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

            if (!CheckInventoryForIngredient(ingredientList))
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
        public Order SuggestOrder(User user)
        {
            Order pastOrder = GetLastOrder(user);
            if(pastOrder != null)
                return new Order(GetLastOrder(user));

            return null;
        }
        public Order GetLastOrder(User user)
        {
            return OrderHistory.FirstOrDefault(o => o.User.FullName == user.FullName);
        }
        public Order GetLastOrder()
        {
            if (OrderHistory.Count > 0)
                return OrderHistory.Peek();
            return null;
        }
        public Stack<Order> GetFullHistory()
        {
            return OrderHistory;
        }
        public Stack<Order> GetFullHistory(User user)
        {
            // Use LINQ to grab only user past orders
            return new Stack<Order>(OrderHistory
                .Where(o => o.User.FullName == user.FullName)
                .Select(o => o));

        }
        #endregion

        #region Helper Functions
        private void DepleteInventory(KeyValuePair<string, int> ingredient)
        {
            string validatedIngredient = Pizza.ValidateIngredient(ingredient.Key);

            if (Inventory.ContainsKey(validatedIngredient))
                Inventory[validatedIngredient] -= ingredient.Value;
        }
        private bool CheckInventoryForIngredient(Dictionary<string, int> ingredientList)
        {
            if (ingredientList == null || ingredientList.Count == 0)
                return false;

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
        #endregion

    }
}