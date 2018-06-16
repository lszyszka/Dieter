using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.Data;
using Dieter.Models;

namespace dieter.Models
{
    class DieterDBM : DataContext
    {
        public DieterDBM() : base(Properties.Settings.Default.DieterDBConnectionPath)
        {
        }

        public Table<Day>Days;
        public Table<Meal> Meals;
        public Table<ProductMeal> ProductMeal;
        public Table<Product> Products;
    }
}
