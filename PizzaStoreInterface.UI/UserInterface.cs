using PizzaStoreLibrary.library;
using System;
using System.Collections.Generic;

namespace PizzaStoreInterface.UI
{
    // interactive console application
    // only display-related code can be here
    // low-priority component, will be replaced when we move to project 1
    class UserInterface
    {
        static void Main(string[] args)
        {
            Pizza p = new Pizza(new string[] { null });

            Console.WriteLine(p.IsValid());

        }
    }
}
