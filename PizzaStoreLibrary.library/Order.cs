using PizzaStoreLibrary.library;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaStoreLibrary.library
{
    public class Order
    {
        #region Fields

        // has a location
        private Location _location;
        // has a user
        private User _user;
        // has an order time (when the order was placed)
        private TimeSpan _orderTime;
        // can have at most 12 pizzas
        private List<Pizza> _pizzas;

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

        // Copy constructor
        public Order(Order o)
        : this(o?.User)
        {
            // Preform a deep copy of the provided order
            if (o.Location != null)
                Location = new Location(o.Location.Name);
            if (OrderTime != null)
            {
                OrderTime = new TimeSpan(o.OrderTime.Hours, o.OrderTime.Minutes, o.OrderTime.Seconds);
            }
            foreach (Pizza pizza in o.PizzaList)
            {
                PizzaList.Add(new Pizza(pizza.Ingredients));
            }
        }
        public Order(User user)
        {
            User = user;
            _orderTime = new TimeSpan();
            _pizzas = new List<Pizza>();
        }
        public Order(params string[] user)
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

        #region Operators

        
        public bool Equals(Order rhs)
        {
            if ((rhs.Location == null && Location == null) ||
                (Location.Name.Equals(rhs.Location.Name) &&
                User.FullName.Equals(rhs.User.FullName)))
            {
                // TODO:
                // I understand there could be different pizzas 
                //  in the order which would make this a different
                //  order. Fix later.
                if (PizzaList.Count == rhs.PizzaList.Count)
                {
                    // 1 Second difference buffer for 
                    //  checking time equality
                    if(Math.Abs((OrderTime - rhs.OrderTime).Seconds) < 1)
                        return true;
                }
            }
                
            return false;
        }
        
        #endregion  

        #region Properties

        public Location Location { get => _location; set => _location = value; }
        public User User { get => _user; set => _user = value; }
        public TimeSpan OrderTime { get => _orderTime; set => _orderTime = value; }
        public List<Pizza> PizzaList { get => _pizzas; }
        // total value cannot exceed $500
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
            if (PizzaList.Count == Utilities.MaxPizzasPerOrder || !Pizza.PizzaIsValid(pizza) || 
                pizza.Cost + Cost > Utilities.MaxCostPerOrder)
                return;

            PizzaList.Add(pizza);
        }
        // Adding new pizza using strings
        public void AddPizzaToOrder(params string[] ingredients)
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
