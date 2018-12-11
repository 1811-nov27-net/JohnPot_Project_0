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
                Console.WriteLine($"Welcome back {firstName}!");
                currentUser = db.Mapper.Map(dbUser, options);
            }
            else
            {
                currentUser = new lib.User(firstName, lastName);
                repo.userRepository.Create(currentUser);
                repo.SaveAll();
            }

            // While true... Lol. 
            while (true)
            {
                Console.WriteLine("Enter some input...");
                string input;
                input = Console.ReadLine();

                // Process input
                input = userInterface.SanitizeInput(input);
                if (input != null)
                {
                    object[] parameters = new object[] { input };
                    string output = (string)userInterface.GetType().GetMethod(input).Invoke(userInterface, parameters);
                    Console.WriteLine(output);
                }
            }
        }
    }
}