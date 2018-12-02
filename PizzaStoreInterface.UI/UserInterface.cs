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
            // Arrange
            Order order1 = new Order(new string[] { "John", "Pot" });

            Pizza reallyExpensivePizza = new Pizza();
            // Create a $100 pizza ($10 base + 90-$1 toppings)
            for (int i = 0; i < 90; ++i)
            {
                reallyExpensivePizza.AddToppingsToPizza("Pepperoni");
            }
            
            // Add six $100 pizzas = $600 total
            for (int i = 0; i < 6; ++i)
            {
                order1.AddPizzaToOrder(reallyExpensivePizza);
            }

            Console.WriteLine(order1.PizzaList.Count);

        }
    }
}
