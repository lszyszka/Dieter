using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using dieter.Models;

namespace Dieter.Models
{
    [Table(Name = "Days")]
    public class Day 
    {
        private int id;
        private String date;
        int kcal;
        double protein;
        double fat;
        double carbohydrate;

        EntitySet<Meal> _meals;

        public Day()
        {
            Date = DateTime.Today.ToShortDateString();
            _meals = new EntitySet<Meal>(OnMealAdded, OnMealRemoved);
        }
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get => id; set => id = value; }
        [Column]
        public String Date { get => date; set => date = value; }
        

        [Association(Name = "FK_Meals_Days",
        Storage = "_meals", OtherKey = "dayId", ThisKey = "Id")]
        public ICollection<Meal> Meals
        {
            get { return _meals; }
            set { _meals.Assign(value); }
        }

        [Column]
        public int Kcal { get => kcal; set => kcal = value; }
        [Column]
        public double Protein { get => protein; set => protein = value; }
        [Column]
        public double Fat { get => fat; set => fat = value; }
        [Column]
        public double Carbohydrate { get => carbohydrate; set => carbohydrate = value; }
        

        private void OnMealAdded(Meal addedMeal)
        {
            addedMeal.Day = this;
        }

        private void OnMealRemoved(Meal removedMeal)
        {
            removedMeal.Day = null;
        }
    }
    
}
