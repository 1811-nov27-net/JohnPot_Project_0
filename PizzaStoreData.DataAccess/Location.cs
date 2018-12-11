using System;
using System.Collections.Generic;

namespace PizzaStoreData.DataAccess
{
    public partial class Location
    {
        public Location()
        {
            Inventory = new HashSet<Inventory>();
            Order = new HashSet<Order>();
            User = new HashSet<User>();
        }

        public int LocationId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Inventory> Inventory { get; set; }
        public virtual ICollection<Order> Order { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
