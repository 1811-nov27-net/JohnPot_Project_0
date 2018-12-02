using PizzaStoreLibrary.library;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaStoreLibrary.library
{
    public class Order
    {
        // Pizza limit denoted by project instructions
        private static readonly int MaxPizzasPerOrder = 12;
        private static readonly int MaxCostPerOrder = 500;

        #region Fields

        // has a location
        private Location _location;
        // has a user
        private User _user;
        // has an order time(when the order was placed)
        private DateTime _orderTime;
        // can have at most 12 pizzas
        private List<Pizza> _pizzas;
        // total value cannot exceed $500
        private float _cost;

        #endregion

        #region Constructors

        /***** Constructors *****/
        // Intentionally left out default constructor
        //  since I think a user should be required
        //  inorder to create an order.
        // Location / pizzas can always be changed

        // All Constructors will call this base
        //  constructor. Initialize stuff that needs to 
        //  happen in all Orders here
        public Order(User user)
        {
            User = user;
            _orderTime = new DateTime();
            _pizzas = new List<Pizza>();
        }
        public Order(string[] user)
        :this(new User(user))
        {
        }
        public Order(string[] user, Location location)
        :this(user)
        {
            SetOrderLocation(location);
        }
        public Order(string[] user, string location)
        : this(user, new Location(location))
        {
        }
        public Order(User user, Location location)
        :this(user)
        {
            SetOrderLocation(location);
        }
        public Order(User user, string location)
        :this(user, new Location(location))   
        {
        }
        #endregion

        #region Properties

        public Location Location { get => _location; set => _location = value; }
        public User User { get => _user; set => _user = value; }
        public DateTime OrderTime { get => _orderTime; set => _orderTime = value; }
        public List<Pizza> PizzaList { get => _pizzas; }
        public float Cost
        {
            get
            {
                float totalCost = 0.0f;
                foreach (Pizza pizza in PizzaList)
                {
                    totalCost += pizza.Cost;
                }

                return totalCost;
            }
        }

        #endregion

        #region Methods

        // Core Add pizza method that will be called
        //  on all variations of AddPizzaToOrder
        public void AddPizzaToOrder(Pizza pizza)
        {
            // TODO: Maybe let the user know they are
            //  adding too many pizzas? Set up event/delegate
            //  here to fire when too many pizzas are added!
            if (PizzaList.Count == MaxPizzasPerOrder || !pizza.IsValid() || 
                pizza.Cost + Cost > MaxCostPerOrder)
                return;

            PizzaList.Add(pizza);
        }
        // Adding new pizza using strings
        public void AddPizzaToOrder(string[] ingredients)
        {
            AddPizzaToOrder(new Pizza(ingredients));
        }
        // Core Set order method that will be called
        //  on all variations of SetOrderLocation
        public void SetOrderLocation(Location location)
        {
            Location = location;
        }
        // Setting location using string
        public void SetOrderLocation(string location)
        {
            SetOrderLocation(new Location(location));
        }

        #endregion
    }
}
