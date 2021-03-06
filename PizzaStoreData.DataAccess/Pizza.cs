﻿using System;
using System.Collections.Generic;

namespace PizzaStoreData.DataAccess
{
    public partial class Pizza
    {
        public int PizzaId { get; set; }
        public int IngredientId { get; set; }
        public int? Count { get; set; }

        public virtual Ingredient Ingredient { get; set; }
    }
}
