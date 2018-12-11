using System;
using System.Collections.Generic;

namespace PizzaStoreData.DataAccess
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public int? LocationId { get; set; }
        public int? UserId { get; set; }
        public DateTime TimePlaced { get; set; }
        public int PizzaId { get; set; }

        public virtual Location Location { get; set; }
        public virtual User User { get; set; }
    }
}
