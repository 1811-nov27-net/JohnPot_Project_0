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
        public int? PizzaId1 { get; set; }
        public int? PizzaId2 { get; set; }
        public int? PizzaId3 { get; set; }
        public int? PizzaId4 { get; set; }
        public int? PizzaId5 { get; set; }
        public int? PizzaId6 { get; set; }
        public int? PizzaId7 { get; set; }
        public int? PizzaId8 { get; set; }
        public int? PizzaId9 { get; set; }
        public int? PizzaId10 { get; set; }
        public int? PizzaId11 { get; set; }
        public int? PizzaId12 { get; set; }

        public virtual Location Location { get; set; }
        public virtual Pizza PizzaId10Navigation { get; set; }
        public virtual Pizza PizzaId11Navigation { get; set; }
        public virtual Pizza PizzaId12Navigation { get; set; }
        public virtual Pizza PizzaId1Navigation { get; set; }
        public virtual Pizza PizzaId2Navigation { get; set; }
        public virtual Pizza PizzaId3Navigation { get; set; }
        public virtual Pizza PizzaId4Navigation { get; set; }
        public virtual Pizza PizzaId5Navigation { get; set; }
        public virtual Pizza PizzaId6Navigation { get; set; }
        public virtual Pizza PizzaId7Navigation { get; set; }
        public virtual Pizza PizzaId8Navigation { get; set; }
        public virtual Pizza PizzaId9Navigation { get; set; }
        public virtual User User { get; set; }
    }
}
