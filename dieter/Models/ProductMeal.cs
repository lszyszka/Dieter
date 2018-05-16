using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace dieter.Models
{
    [Table]
    public class ProductMeal
    {
        
        private int id;
        private double amount;
        private double protein;
        private double carbohydrate;
        private double fat;
        private int kcal;

        [Column(IsPrimaryKey =true, IsDbGenerated =true)]
        public int Id { get => id; set => id = value; }
        [Column]
        public double Amount { get => amount; set => amount = value; }
        [Column]
        public double Protein { get => protein; set => protein = value; }
        [Column]
        public double Carbohydrate { get => carbohydrate; set => carbohydrate = value; }
        [Column]
        public double Fat { get => fat; set => fat = value; }

        [Column]
        public int Kcal { get => kcal; set => kcal = value; }

        [Column(Name ="ProductId")]
        private int productId;

        private EntityRef<Product> _product = new EntityRef<Product>();

        [Association(Name = "FK_ProductMeal_Products", IsForeignKey =true, 
            Storage ="_product", ThisKey ="productId")]
        public Product Product
        {
            get { return _product.Entity; }
            set {
                Product previousProduct = _product.Entity;
                Product newProduct = value;
                if(previousProduct != newProduct)
                {
                    _product.Entity = null;
                    if(previousProduct != null)
                    {
                        previousProduct.ProductMeals.Remove(this);
                    }
                    _product.Entity = newProduct;
                    if(newProduct != null)
                    {
                        newProduct.ProductMeals.Add(this);
                    }
                }
            }
        }

        [Column(Name = "MealId")]
        private int mealId;

        private EntityRef<Meal> _meal = new EntityRef<Meal>();

        [Association(Name = "FK_ProductMeal_Meals", IsForeignKey =true,
            Storage ="_meal", ThisKey = "mealId")]
        public Meal Meal
        {
            get { return _meal.Entity; }
            set {
                Meal previousMeal = _meal.Entity;
                Meal newMeal = value;
                if(previousMeal != newMeal)
                {
                    _meal.Entity = null;
                    if (previousMeal != null)
                    {
                        previousMeal.ProductMeals.Remove(this);
                    }
                    _meal.Entity = newMeal;
                    if(newMeal != null)
                    {
                        newMeal.ProductMeals.Add(this);
                    }
                }
            }
        }

        
    }
}
