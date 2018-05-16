using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using Dieter.Models;

namespace dieter.Models
{
    [Table(Name="Meals")]
    public class Meal
    {
        private int id;
        private String time;
        int kcal;
        double protein;
        double fat;
        double carbohydrate;
        private EntitySet<ProductMeal> _productMeals = new EntitySet<ProductMeal>();
        private EntityRef<Day> _day;

        public Meal()
        {
            Time = DateTime.Now.ToShortTimeString();
            _productMeals = new EntitySet<ProductMeal>(OnProductMealAdded, OnProductMealRemoved);
        }

        private void OnProductMealRemoved(ProductMeal removedProductMeal)
        {
            removedProductMeal.Meal = null;
        }

        private void OnProductMealAdded(ProductMeal addedProductMeal)
        {
            addedProductMeal.Meal = this;
        }

        [Column(IsPrimaryKey = true, IsDbGenerated =true)]
        public int Id { get => id; set => id = value; }
        [Column]
        public String Time { get => time; set => time = value; }
        [Column]
        public int Kcal { get => kcal; set => kcal = value; }
        [Column]
        public double Protein { get => protein; set => protein = value; }
        [Column]
        public double Fat { get => fat; set => fat = value; }
        [Column]
        public double Carbohydrate { get => carbohydrate; set => carbohydrate = value; }

        [Column(Name = "DayId")] public int dayId;       

        [Association(Name = "FK_Meals_Days",
        IsForeignKey = true, Storage = "_day", ThisKey = "dayId")]
        public Day Day
        {
            get { return _day.Entity; }
            set {
                Day previousDay = _day.Entity;
                Day newDay = value;
                if(newDay != previousDay)
                {
                    _day.Entity = null;
                    if(previousDay != null)
                    {
                        previousDay.Meals.Remove(this);
                    }
                    _day.Entity = newDay;
                    if(newDay != null)
                    {
                        newDay.Meals.Add(this);
                    }
                }                
            }
        }

        [Association(Name ="FK_ProductMeal_Meals", Storage ="_productMeals",
            OtherKey ="mealId", ThisKey ="Id")]
        public ICollection<ProductMeal> ProductMeals
        {
            get { return _productMeals; }
            set { _productMeals.Assign(value); }
        }
    }
}
