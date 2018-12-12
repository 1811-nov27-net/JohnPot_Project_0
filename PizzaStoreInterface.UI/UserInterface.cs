using System;
using System.Collections.Generic;
using System.Linq;
using lib = PizzaStoreLibrary.library;
using db = PizzaStoreData.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace PizzaStoreInterface.UI
{
    // interactive console application
    // only display-related code can be here
    // low-priority component, will be replaced when we move to project 1
    class UserInterface
    {
        static void Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<db.PizzaStoreContext>();
            optionsBuilder.UseSqlServer(db.SecretConfiguration.ConnectionString);
            var options = optionsBuilder.Options;

            Repo repo = new Repo(options);
            UserMethod userInterface = new UserMethod(options);

            // Populate the ingredient validator
            using (var database = new db.PizzaStoreContext(options))
            {
                List<db.Ingredient> ingredients = database.Ingredient.ToList();
                foreach (db.Ingredient ingredient in ingredients)
                {
                    lib.IngredientValidator.AddIngredient(new KeyValuePair<string, float>(ingredient.Name, (float)ingredient.Cost));
                }
            }

            Console.WriteLine("Welcome to John's Pizza Management!");

            lib.Location currentLocation = null;
            lib.User currentUser = null;
            lib.StoreManager store = new lib.StoreManager();

            Console.WriteLine("User login... ");
            Console.WriteLine("What's your first name?");
            string firstName, lastName;
            firstName = Console.ReadLine();
            Console.WriteLine("Last name?");
            lastName = Console.ReadLine();
            Console.WriteLine($"Thank you, {firstName} {lastName}");

            db.User dbUser = repo.userRepository.GetByName(firstName, lastName);

            if (dbUser != null)
            {
                Console.WriteLine($"Welcome back, {firstName}!");
                currentUser = db.Mapper.Map(dbUser, options);
            }
            else
            {
                currentUser = new lib.User(firstName, lastName);
                repo.userRepository.Create(currentUser);
                repo.SaveAll();
            }

            userInterface.currentUser = currentUser;

            // While true... Lol. 
            while (true)
            {
                Console.WriteLine("Enter some input...");
                string input;
                input = Console.ReadLine();

                if(input == "Exit" || input == "exit")
                {
                    Console.WriteLine($"Goodbye, {currentUser.FirstName}");
                    return;
                }
                // Process input
                input = userInterface.SanitizeInput(input);
                if (input != userInterface.MethodResponse[UserMethod.Responses.InvalidInput])
                {
                    List<string> parameters = new List<string>();
                    // Retrieve the parameters
                    var methodParameters = userInterface.GetType().GetMethod(input).GetParameters();
                    foreach (var param in methodParameters)
                    {
                        Console.WriteLine(param.Name + "?");
                        parameters.Add(Console.ReadLine());
                    }

                    string output = (string)userInterface.GetType().GetMethod(input).Invoke(userInterface, parameters.ToArray());
                    repo.SaveAll();
                    Console.WriteLine(output);
                }
                else
                {
                    Console.WriteLine(input);
                }
            }
        }
    }
}