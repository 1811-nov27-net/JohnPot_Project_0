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
        private DateTime _TimePlaced;
        // can have at most 12 pizzas
        private List<Pizza> _pizzas;

        private int id;

        // For generating pizza ids
        private Random rand;

        #endregion

        #region Properties

        public Location Location { get => _location; set => _location = value; }
        public User User { get => _user; set => _user = value; }
        public DateTime TimePlaced { get => _TimePlaced; set => _TimePlaced = value; }
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

        public int Id { get => id; set => id = value; }


        #endregion

        #region Constructors

        // Intentionally left out default constructor
        //  since I think a user should be required
        //  inorder to create an order.
        // Location / pizzas can always be changed

        // All Constructors will call this base
        //  constructor. Initialize stuff that needs to 
        //  happen in all Orders here

        // Copy constructor
        // This constructor gets called with 
        //  each variant of constructors
        public Order(User user)
        {
            rand = new Random(DateTime.Now.TimeOfDay.Milliseconds * DateTime.Now.TimeOfDay.Seconds);
            id = rand.Next();

            User = user;
            _TimePlaced = new DateTime();
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
            return Id == rhs.Id;
        }
        
        #endregion  


        #region Methods

        // Core Add pizza method that will be called
        //  on all variations of AddPizzaToOrder
        public int AddPizzaToOrder(Pizza pizza)
        {
            if (!Pizza.PizzaIsValid(pizza))
                return -10;
            if (PizzaList.Count == Utilities.MaxPizzasPerOrder)
                return -3;
            if (pizza.Cost + Cost > Utilities.MaxCostPerOrder)
                return -1;
            
            pizza.Id = rand.Next();
            PizzaList.Add(pizza);
            return 1;
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

        public int IsValid()
        {
            if (Location != null && PizzaList.Count > 0)
            {
                if (PizzaList.Count > 12)
                    return -3;
                return 1;
            }
            
            return -10;
        }

        public void DisplayMax()
        {
            Console.WriteLine($"OrderId: {Id}");
            Console.WriteLine($"Total Cost: {Cost}");
            Console.WriteLine($"Location: {Location?.Name}");
            Console.WriteLine($"User: {User.FirstName} {User.LastName}");
            Console.WriteLine($"Time Placed: {TimePlaced}");
            Console.WriteLine($"Pizzas: ({PizzaList.Count} total for this order)");
            for (int i = 0; i < PizzaList.Count; i++)
            {
                Console.Write($"   ({i + 1}) ");
                PizzaList[i].Display();
            }
        }
        public void Display()
        {
            //Console.WriteLine($"OrderId: {Id}");
            Console.WriteLine($"Total Cost: {Cost}");
            //Console.WriteLine($"Location: {Location?.Name}");
            //Console.WriteLine($"User: {User.FirstName} {User.LastName}");
            //Console.WriteLine($"Time Placed: {TimePlaced}");
            Console.WriteLine($"Pizzas: ({PizzaList.Count} total for this order)");
            for (int i = 0; i < PizzaList.Count; i++)
            {
                Console.Write($"   ({i+1}) ");
                PizzaList[i].Display();
            }
        }

        #endregion
    }
}
