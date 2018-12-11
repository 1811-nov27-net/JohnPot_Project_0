using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PizzaStoreLibrary.library
{
    // Store is going to encapsulate the functionality
    //  requried by the project
    public class StoreManager
    {
        private readonly List<User> _userList = new List<User>();
        private List<Location> _locationList = new List<Location>();

        public List<User> UserList { get => _userList; }
        public List<Location> LocationList { get => _locationList; }

        public Location GetLocationByName(string name)
        {
            return LocationList.Find(l => l.Name == name);
        }

        public User RegisterNewUser(User user)
        {
            if (user == null)
                return null;
            // Check to see if the user already exists
            if (!UserList.Contains(user))
            {
                UserList.Add(user);
                return user;
            }
            return null;
        }
        public User RegisterNewUser(params string[] name)
        {
            if (name.Length == 0 || name.Length == 1)
                return null;

            return RegisterNewUser(new User(name));
        }

        public Location RegisterNewLocation(Location location)
        {
            if (location == null)
                return null;

            // Make sure the location does not already exist
            if (!LocationList.Contains(location))
            {
                LocationList.Add(location);
                return location;
            }

            return null;
        }
        public Location RegisterNewLocation(string name)
        {
            if (name.Length == 0)
                return null;

            return RegisterNewLocation(new Location(name));
        }


        //    //place pizza order to a location for a user
        //    public bool PlaceOrder();

        //get a suggested order for a user based on his order history
        public Order SuggestOrder(User user)
        {
            List<Order> userOrderHistory = GetUserHistory(user);

            return userOrderHistory.OrderBy(o => o.TimePlaced).FirstOrDefault();

        }

        public bool PlaceOrder(Order o)
        {
            if (o.IsValid())
            {
                return o.Location.PlaceOrder(o);
            }
            return false;
        }


        //    //search users by name
        public User FindUser(string userFirstName, string userLastName)
        {
            foreach (User user in UserList)
            {
                if (user.FirstName == userFirstName &&
                   user.LastName == userLastName)
                    return user;
            }

            return null;
        }

        //    //display details of an order
        //    public void DisplayOrder(Order order);

        //    //display all order history of a location
        public void DisplayHistory(Location location)
        {
            location?.DisplayOrderHistory();
        }

        //display all order history of a user
        public void DisplayUserHistory(User user)
        {
            List<Order> userOrderList = GetUserHistory(user);

            Console.WriteLine($"Order history for: {user.FirstName} {user.LastName}");
            for (int i = 0; i < userOrderList.Count; i++)
            {
                Console.WriteLine($"Order ({i}):");
                userOrderList[i].Display();
            }
        }
        public List<Order> GetUserHistory(User user)
        {
            List<Order> userOrderList = new List<Order>();
            foreach (Location l in LocationList)
            {
                userOrderList.AddRange(l.GetFullHistory(user));
            }

            return userOrderList;
        }

        //    //display order history sorted by earliest, latest, cheapest, most expensive

        public void DisplayOrderHistoryBy(List<Order> orders, Comparison<Order> sortMethod)
        {
            // Sort the list 
            orders.Sort(sortMethod);
            foreach (Order o in orders)
            {
                o.Display();
                Console.WriteLine();
            }
        }

    }
}
