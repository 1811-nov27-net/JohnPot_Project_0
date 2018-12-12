using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using db = PizzaStoreData.DataAccess;
using lib = PizzaStoreLibrary.library;


namespace PizzaStoreInterface.UI
{
    public class UserMethod
    {
        private readonly DbContextOptions<db.PizzaStoreContext> _options;
        private Repo repo;
        private lib.StoreManager store;

        public Dictionary<Responses, string> MethodResponse;
        public lib.User currentUser;

        public enum Responses
        {
            // Valid responses
            LocationCreated,
            DefaultLocationSet,
            LocationStocked,
            OrderPlaced,
            ToppingAdded,

            // Invalid responses
            InvalidLocationName,
            LocationExists,
            InvalidUserName,
            InvalidPizzaTopping,
            TooManyPizzas,
            NoInventoryCreated,
            InvalidInput,
            ToppingExists,
            MustWaitLonger,
            InsufficientIngredients,
            CostTooMuch,
            SwitchedUsers,
            UserExists,
            OrderTooSoon
        }

        private void GenerateResponses()
        {
            // Valid responses
            MethodResponse[Responses.LocationCreated] = "New location was successfully created: ";
            MethodResponse[Responses.DefaultLocationSet] = "Default location successfully set to: ";
            MethodResponse[Responses.LocationStocked] = "Location was successfully stocked!";
            MethodResponse[Responses.ToppingAdded] = "Successfully added topping!";
            MethodResponse[Responses.OrderPlaced] = "Order was successfully placed!";
            MethodResponse[Responses.SwitchedUsers] = "You are now logged in as: ";

            // Invalid responses
            MethodResponse[Responses.MustWaitLonger] = "Order not placed. Same users are unable to place an order at the same location within 2 hours!";
            MethodResponse[Responses.InsufficientIngredients] = "Could not add pizza to order. Location does not have required toppings in stock!";
            MethodResponse[Responses.CostTooMuch] = "Could not add pizza. Order cost would exceed $500!";
            MethodResponse[Responses.InvalidInput] = "Invalid Input! Try again...";
            MethodResponse[Responses.LocationExists] = "Location already exists!: ";
            MethodResponse[Responses.InvalidLocationName] = "Invalid Location Name!";
            MethodResponse[Responses.InvalidUserName] = "Invalid User Name!";
            MethodResponse[Responses.InvalidPizzaTopping] = "Invalid Pizza Topping!";
            MethodResponse[Responses.TooManyPizzas] = "You can only have a maximum of 12 pizzas per order!";
            MethodResponse[Responses.NoInventoryCreated] = "No inventory for location/ingredient was found.";
            MethodResponse[Responses.ToppingExists] = "Topping already exists!";
            MethodResponse[Responses.UserExists] = "That user already exists!";
            MethodResponse[Responses.OrderTooSoon] = "Must wait at least 2 hours to order from same location. Last order placed: ";
        }

        private void SyncStoreToDB()
        {
            List<db.Location> dbLocations;
            List<db.User> dbUsers;
            using (var database = new db.PizzaStoreContext(_options))
            {
                dbLocations = database.Location.ToList();
                dbUsers = database.User.ToList();
            }
            foreach (db.Location dbLocation in dbLocations)
            {
                store.RegisterNewLocation(db.Mapper.Map(dbLocation, _options));
            }
            foreach (db.User dbUser in dbUsers)
            {
                store.RegisterNewUser(db.Mapper.Map(dbUser, _options));
            }
        }

        public string SanitizeInput(string input)
        {
            // Check to see if the input is a valid method name
            var methods = GetType().GetMethods();
            List<string> methodNames = new List<string>();
            foreach (MethodInfo m in methods)
            {
                methodNames.Add(m.Name.ToLower());
            }

            if (!methodNames.Contains(input.ToLower()))
                return MethodResponse[Responses.InvalidInput];

            return input.ToLower();
        }
        public string test(string input)
        {
            return "You entered: " + input;
        }

        public UserMethod(DbContextOptions<db.PizzaStoreContext> options)
        {
            _options = options;
            repo = new Repo(options);
            MethodResponse = new Dictionary<Responses, string>();
            GenerateResponses();

            store = new lib.StoreManager();
            SyncStoreToDB();
        }

        public string SanitizeToppingInput(string input)
        {
            // TODO: Better input checks here...
            return input.ToLower();
        }


        public string startorder2(string LocationName)
        {

            db.Location dbLocation = new db.Location() { Name = LocationName };
            // This is just silly... but time is a thing.
            dbLocation = db.Mapper.GetLocationById(db.Mapper.GetLocationIdByName(LocationName, _options), _options);
            // Make sure the location is valid
            if (dbLocation.LocationId == -1)
                return MethodResponse[Responses.InvalidLocationName] + LocationName;
            // 2 hour rule
            {
                List<db.Order> dbOrders = repo.orderRepository.GetAllOrders();
                List<lib.Order> libOrders = ConcatDBOrders(dbOrders);
                lib.Order mostRecent = libOrders.Where(o => o.User.Id == currentUser.Id)
                        .Where(o => o.Location.Id == dbLocation.LocationId).OrderBy(o => o.TimePlaced).First();
                if(DateTime.Now.TimeOfDay - mostRecent.TimePlaced.TimeOfDay < new TimeSpan(2, 0, 0))
                {
                    return MethodResponse[Responses.OrderTooSoon] + mostRecent.TimePlaced.ToShortTimeString();
                }
            }

            lib.Order libOrder = new lib.Order(currentUser);
            libOrder.Location = db.Mapper.Map(dbLocation, _options);
            string orderInput = "";
            while (orderInput != "No" && orderInput != "no")
            {
                string input = "";
                lib.Pizza libPizza = new lib.Pizza();
                while (input != "end")
                {
                    Console.WriteLine("Enter topping to add to pizza: (Enter 'end' to finish pizza)");
                    input = Console.ReadLine();
                    input = SanitizeToppingInput(input);
                    if (!lib.IngredientValidator.IsValidIngredient(input) && input != "end")
                    {
                        Console.WriteLine(MethodResponse[Responses.InvalidPizzaTopping]);
                        continue;
                    }

                    // Attempt to add the topping to a pizza
                    libPizza.AddIngredientsToPizza(input);
                    libPizza.Display();
                }
                int added = libOrder.AddPizzaToOrder(libPizza);
                if (added == -1)
                {
                    Console.WriteLine(MethodResponse[Responses.CostTooMuch]);
                }
                else if (added == -3)
                {
                    Console.WriteLine(MethodResponse[Responses.TooManyPizzas]);
                }
                libOrder.Display();
                Console.WriteLine("Add another pizza to the order? (Yes / No)");
                orderInput = Console.ReadLine();
                while (orderInput != "Yes" && orderInput != "yes" &&
                       orderInput != "No" && orderInput != "no")
                {
                    Console.WriteLine(MethodResponse[Responses.InvalidInput]);
                    Console.WriteLine("Add another pizza to the order? (Yes / No)");
                    orderInput = Console.ReadLine();
                }
            }

            libOrder.Location = db.Mapper.Map(dbLocation, _options);
            libOrder.User = currentUser;
            int orderPlaced = libOrder.Location.PlaceOrder(libOrder);
            if (orderPlaced < 0)
            {
                switch (orderPlaced)
                {
                    case -1:
                        {
                            return MethodResponse[Responses.MustWaitLonger];
                            break;
                        }
                    case -2:
                        {
                            return MethodResponse[Responses.InsufficientIngredients];
                            break;
                        }
                    case -3:
                        {
                            return MethodResponse[Responses.TooManyPizzas];
                            break;
                        }
                    default:
                        return "Something went wrong while placing the order.";

                }
            }

            foreach (lib.Pizza p in libOrder.PizzaList)
            {
                repo.pizzaRepository.Create(p);
            }
            repo.orderRepository.Create(libOrder);

            foreach (var i in libOrder.Location.Inventory)
            {
                db.Inventory inventory = repo.inventoryRepository.GetById(libOrder.Location.Id, db.Mapper.GetIngredientByName(i.Key, _options).IngredientId);
                inventory.Count = i.Value;
                repo.inventoryRepository.Update(inventory);
            }
            repo.locationRepository.Update(libOrder.Location);

            return MethodResponse[Responses.OrderPlaced];
        }

        public string startorder(string LocationName)
        {
            db.Location dbLocation = new db.Location() { Name = LocationName };
            // This is just silly... but time is a thing.
            dbLocation = db.Mapper.GetLocationById(db.Mapper.GetLocationIdByName(LocationName, _options), _options);
            // Make sure the location is valid
            if (dbLocation.LocationId == -1)
                return MethodResponse[Responses.InvalidLocationName] + LocationName;

            lib.Order libOrder = new lib.Order(currentUser);
            libOrder.Location = db.Mapper.Map(dbLocation, _options);
            string orderInput = "";
            while (orderInput != "No" && orderInput != "no")
            {
                string input = "";
                lib.Pizza libPizza = new lib.Pizza();
                while (input != "end")
                {
                    Console.WriteLine("Enter topping to add to pizza: (Enter 'end' to finish pizza)");
                    input = Console.ReadLine();
                    input = SanitizeToppingInput(input);
                    if (!lib.IngredientValidator.IsValidIngredient(input) && input != "end")
                    {
                        Console.WriteLine(MethodResponse[Responses.InvalidPizzaTopping]);
                        continue;
                    }

                    // Attempt to add the topping to a pizza
                    libPizza.AddIngredientsToPizza(input);
                    libPizza.Display();
                }
                int added = libOrder.AddPizzaToOrder(libPizza);
                if (added == -1)
                {
                    Console.WriteLine(MethodResponse[Responses.CostTooMuch]);
                }
                else if(added == -3)
                {
                    Console.WriteLine(MethodResponse[Responses.TooManyPizzas]);
                }
                libOrder.Display();
                Console.WriteLine("Add another pizza to the order? (Yes / No)");
                orderInput = Console.ReadLine();
                while (orderInput != "Yes" && orderInput != "yes" &&
                       orderInput != "No" && orderInput != "no")
                {
                    Console.WriteLine(MethodResponse[Responses.InvalidInput]);
                    Console.WriteLine("Add another pizza to the order? (Yes / No)");
                    orderInput = Console.ReadLine();
                }
            }

            libOrder.Location = db.Mapper.Map(dbLocation, _options);
            libOrder.User = currentUser;
            int orderPlaced = libOrder.Location.PlaceOrder(libOrder);
            if (orderPlaced < 0)
            {
                switch (orderPlaced)
                {
                    case -1:
                        {
                            return MethodResponse[Responses.MustWaitLonger];
                            break;
                        }
                    case -2:
                        {
                            return MethodResponse[Responses.InsufficientIngredients];
                            break;
                        }
                    case -3:
                        {
                            return MethodResponse[Responses.TooManyPizzas];
                            break;
                        }
                    default:
                        return "Something went wrong while placing the order.";

                }
            }

            foreach (lib.Pizza p in libOrder.PizzaList)
            {
                repo.pizzaRepository.Create(p);
            }
            repo.orderRepository.Create(libOrder);

            foreach (var i in libOrder.Location.Inventory)
            {
                db.Inventory inventory = repo.inventoryRepository.GetById(libOrder.Location.Id, db.Mapper.GetIngredientByName(i.Key, _options).IngredientId);
                inventory.Count = i.Value;
                repo.inventoryRepository.Update(inventory);
            }
            repo.locationRepository.Update(libOrder.Location);

            return MethodResponse[Responses.OrderPlaced];
        }

        public string createnewlocation(string LocationName)
        {
            db.Location dbLocation = new db.Location() { Name = LocationName };
            int idCheck = db.Mapper.GetLocationIdByName(LocationName, _options);
            // Make sure the location we are creating does not already exist
            if (idCheck != -1)
                return MethodResponse[Responses.LocationExists] + LocationName;

            // The location does not exist. Create and add it to the database
            repo.locationRepository.Create(dbLocation);
            repo.SaveAll();

            // Add a lib.Location to the store manager for lib use
            store.RegisterNewLocation(db.Mapper.Map(dbLocation, _options));

            return MethodResponse[Responses.LocationCreated] + LocationName;
        }

        public string addpossibleingredient(string IngredientName, string Price)
        {
            float price;
            float.TryParse(Price, out price);
            db.Ingredient dbIngredient;
            dbIngredient = db.Mapper.GetIngredientByName(IngredientName, _options);
            if (dbIngredient != null)
                return MethodResponse[Responses.ToppingExists];

            dbIngredient = new db.Ingredient();
            dbIngredient.Name = IngredientName;
            dbIngredient.Cost = price;
            repo.ingredientRepository.Create(dbIngredient);
            return MethodResponse[Responses.ToppingAdded];
        }

        public string stocklocation(string LocationName, string IngredientName, string IngredientCount)
        {
            SyncStoreToDB();

            int ingredientCount = Int32.Parse(IngredientCount);
            // Make sure the location exists within the database
            db.Location dbLocation = new db.Location() { Name = LocationName };
            dbLocation.LocationId = db.Mapper.GetLocationIdByName(LocationName, _options);
            if (dbLocation.LocationId == -1)
                return MethodResponse[Responses.InvalidLocationName] + LocationName;
            // Make sure the ingredient exists within the database
            db.Ingredient dbIngredient = new db.Ingredient() { Name = IngredientName };
            dbIngredient = repo.ingredientRepository.GetByName(IngredientName);
            if (dbIngredient == null)
                return MethodResponse[Responses.InvalidPizzaTopping] + IngredientName;

            lib.Location libLocation = store.GetLocationByName(LocationName);
            if (libLocation == null)
                return MethodResponse[Responses.InvalidLocationName] + LocationName;

            // Stock the libLocation inventory
            libLocation.StockInventory(new KeyValuePair<string, int>(IngredientName, ingredientCount));

            // Stock the database inventory
            db.Inventory dbInventory;
            dbInventory = repo.inventoryRepository.GetById(dbLocation.LocationId, dbIngredient.IngredientId);
            if (dbInventory == null)
            {
                // Location does not have ingredient currently... Add it
                //  the the inventory.
                dbInventory = new db.Inventory();
                dbInventory.LocationId = dbLocation.LocationId;
                dbInventory.IngredientId = dbIngredient.IngredientId;
                dbInventory.Count = ingredientCount;
                repo.inventoryRepository.Create(dbInventory);
                repo.SaveAll();
                return MethodResponse[Responses.LocationStocked];
            }

            // Everything exists on the database already. Just update
            //  the inventory count
            dbInventory.Count += ingredientCount;
            repo.inventoryRepository.Update(dbInventory);
            repo.SaveAll();
            return MethodResponse[Responses.LocationStocked];
        }

        private void SyncLocationHistory(lib.Location location)
        {
            db.Location dbLocation = db.Mapper.GetLocationByName(location.Name, _options);
            List<db.Order> dbOrders = repo.orderRepository.GetByLocationId(location.Id);

        }

        public List<lib.Order> ConcatDBOrders(List<db.Order> dbOrders)
        {
            List<lib.Order> result = new List<lib.Order>();
            List<int> orderIds = dbOrders.Select(o => o.OrderId).Distinct().ToList();
            foreach (int id in orderIds)
                result.Add(db.Mapper.Map(dbOrders.Where(o => o.OrderId == id).ToList(), _options));

            return result;
        }

        public string createnewuser(string FirstName, string LastName)
        {
            db.User dbUser = repo.userRepository.GetByName(FirstName, LastName);

            if (dbUser != null)
            {
                return MethodResponse[Responses.UserExists];
            }
            else
            {
                currentUser = new lib.User(FirstName, LastName);
                repo.userRepository.Create(currentUser);
                repo.SaveAll();
            }

            return MethodResponse[Responses.SwitchedUsers] + FirstName + " " + LastName;
        }
        public string switchusers(string FirstName, string LastName)
        {
            Console.WriteLine($"Attempting to login as {FirstName} {LastName}");

            db.User dbUser = repo.userRepository.GetByName(FirstName, LastName);

            if (dbUser != null)
            {
                Console.WriteLine($"Welcome back, {FirstName}!");
                currentUser = db.Mapper.Map(dbUser, _options);
            }

            return MethodResponse[Responses.SwitchedUsers] + FirstName + " " + LastName;

        }

        public string displayordersbymostexpensive()
        {
            List<db.Order> dbOrders = repo.orderRepository.GetAllOrders();
            List<lib.Order> libOrders = ConcatDBOrders(dbOrders);

            libOrders = libOrders.OrderByDescending(o => o.Cost).ToList();
            Console.WriteLine();
            Console.WriteLine($"Displaying orders sorted by most expensive first. Total of {libOrders.Count} orders:");
            Console.WriteLine();
            for (int i = 0; i < libOrders.Count; ++i)
            {
                Console.WriteLine($"Order {i + 1}: ");
                libOrders[i].DisplayMax();
                Console.WriteLine();
            }
            return null;
        }
        public string displayordersbycheapest()
        {
            List<db.Order> dbOrders = repo.orderRepository.GetAllOrders();
            List<lib.Order> libOrders = ConcatDBOrders(dbOrders);

            libOrders = libOrders.OrderBy(o => o.Cost).ToList();
            Console.WriteLine();
            Console.WriteLine($"Displaying orders sorted by cheapest first. Total of {libOrders.Count} orders:");
            Console.WriteLine();
            for (int i = 0; i < libOrders.Count; ++i)
            {
                Console.WriteLine($"Order {i + 1}: ");
                libOrders[i].DisplayMax();
                Console.WriteLine();
            }
            return null;
        }
        public string displayordersbylatest()
        {
            List<db.Order> dbOrders = repo.orderRepository.GetAllOrders();
            List<lib.Order> libOrders = ConcatDBOrders(dbOrders);

            libOrders = libOrders.OrderByDescending(o => o.TimePlaced).ToList();
            Console.WriteLine();
            Console.WriteLine($"Displaying orders sorted by latest first. Total of {libOrders.Count} orders:");
            Console.WriteLine();
            for (int i = 0; i < libOrders.Count; ++i)
            {
                Console.WriteLine($"Order {i + 1}: ");
                libOrders[i].DisplayMax();
                Console.WriteLine();
            }
            return null;
        }
        public string displayordersbyearliest()
        {
            List<db.Order> dbOrders = repo.orderRepository.GetAllOrders();
            List<lib.Order> libOrders = ConcatDBOrders(dbOrders);

            libOrders = libOrders.OrderBy(o => o.TimePlaced).ToList();
            Console.WriteLine();
            Console.WriteLine($"Displaying orders sorted by earliest first. Total of {libOrders.Count} orders:");
            Console.WriteLine();
            for (int i = 0; i < libOrders.Count; ++i)
            {
                Console.WriteLine($"Order {i + 1}: ");
                libOrders[i].DisplayMax();
                Console.WriteLine();
            }
            return null;
        }

        public string displaylocationhistory(string LocationName)
        {
            List<db.Order> dbOrders = repo.orderRepository.GetAllOrders();
            List<lib.Order> libOrders = ConcatDBOrders(dbOrders);
            db.Location dbLocation = db.Mapper.GetLocationByName(LocationName, _options);
            if (dbLocation == null)
                return MethodResponse[Responses.InvalidLocationName];

            libOrders = libOrders.Where(o => o.Location.Id == dbLocation.LocationId).ToList();
            Console.WriteLine();
            Console.WriteLine($"Displaying orders for {LocationName}. Total of {libOrders.Count} orders:");
            Console.WriteLine();
            for (int i = 0; i < libOrders.Count; ++i)
            {
                Console.WriteLine($"Order {i + 1}: ");
                libOrders[i].DisplayMax();
                Console.WriteLine();
            }

            return null;

        }

        public void displayuserhistory()
        {
            List<db.Order> dbOrders = repo.orderRepository.GetAllOrders();
            List<lib.Order> libOrders = ConcatDBOrders(dbOrders);
            libOrders = libOrders.Where(o => o.User.Id == currentUser.Id).ToList();
            Console.WriteLine();
            Console.WriteLine($"Displaying orders for {currentUser.FirstName} {currentUser.LastName}. Total of {libOrders.Count} orders:");
            Console.WriteLine();
            for (int i = 0; i < libOrders.Count; ++i)
            {
                Console.WriteLine($"Order {i + 1}: ");
                libOrders[i].DisplayMax();
                Console.WriteLine();
            }

        }

        
        public string suggestorder()
        {
            List<db.Order> dbOrders = repo.orderRepository.GetAllOrders();
            List<lib.Order> libOrders = ConcatDBOrders(dbOrders);
            libOrders = libOrders.Where(o => o.User.Id == currentUser.Id).ToList();
            lib.Order libOrder = libOrders.OrderByDescending(o => o.Cost).First();

            Console.WriteLine("Here's an order you might like:");
            Console.WriteLine();
            libOrder.DisplayMax();

            Console.WriteLine("Re-order? (Yes / No)");
            string input = Console.ReadLine();
            while (input != "Yes" && input != "yes"
                && input != "No" && input != "no")
            {
                Console.WriteLine(MethodResponse[Responses.InvalidInput]);
                Console.WriteLine("Re-order? (Yes / No)");
                input = Console.ReadLine();
            }
            if(input == "Yes" || input == "yes")
            {

                db.Location dbLocation = db.Mapper.GetLocationByName(libOrder.Location.Name, _options);
                lib.Order newOrder = new lib.Order(currentUser);
                newOrder.Location = db.Mapper.Map(dbLocation, _options);
                newOrder.User = currentUser;
                foreach (lib.Pizza p in libOrder.PizzaList)
                {
                    newOrder.AddPizzaToOrder(p);
                }

                int orderPlaced = newOrder.Location.PlaceOrder(newOrder);
                if (orderPlaced < 0)
                {
                    switch (orderPlaced)
                    {
                        case -1:
                            {
                                return MethodResponse[Responses.MustWaitLonger];
                                break;
                            }
                        case -2:
                            {
                                return MethodResponse[Responses.InsufficientIngredients];
                                break;
                            }
                        case -3:
                            {
                                return MethodResponse[Responses.TooManyPizzas];
                                break;
                            }
                        default:
                            return "Something went wrong while placing the order.";

                    }
                }

                foreach (lib.Pizza p in newOrder.PizzaList)
                {
                    repo.pizzaRepository.Create(p);
                }
                repo.orderRepository.Create(newOrder);

                foreach (var i in newOrder.Location.Inventory)
                {
                    db.Inventory inventory = repo.inventoryRepository.GetById(newOrder.Location.Id, db.Mapper.GetIngredientByName(i.Key, _options).IngredientId);
                    inventory.Count = i.Value;
                    repo.inventoryRepository.Update(inventory);
                }

                repo.locationRepository.Update(newOrder.Location);

                return MethodResponse[Responses.OrderPlaced];
            }
            else
            {
                return null;
            }
        }
        

        public string setdefaultlocation(string name)
        {
            // Make sure the location exists
            db.Location dbLocation = new db.Location() { Name = name };
            dbLocation.LocationId = db.Mapper.GetLocationIdByName(name, _options);
            if (dbLocation.LocationId == -1)
                return MethodResponse[Responses.InvalidLocationName] + name;

            // Assuming the user exists already. It should be a valid
            //  user passed in through code, not user input
            //currentUser.DefaultLocationId = dbLocation.LocationId;
            //repo.userRepository.Update(currentUser);
            //repo.SaveAll();

            return MethodResponse[Responses.DefaultLocationSet] + name;
        }


    }
}
