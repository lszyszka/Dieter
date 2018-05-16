using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace dieter.Models
{
    [Table(Name="Products")]
    public class Product
    {
        private int id;
        private int kcal;
        private double carbohydrate;
        private double fat;
        private double protein;
        private int isUnit = 0;
        private String name;
        private EntitySet<ProductMeal> _productMeals;

        public Product()
        {
            _productMeals = new EntitySet<ProductMeal>(OnAddedProductMeal, OnRemovedProductMeal);
        }

        private void OnRemovedProductMeal(ProductMeal removedProductMeal)
        {
            removedProductMeal.Product = null;
        }

        private void OnAddedProductMeal(ProductMeal addedProductMeal)
        {
            addedProductMeal.Product = this;
        }

        [Column(IsPrimaryKey =true, IsDbGenerated =true)]
        public int Id { get => id; set => id = value; }
        [Column]
        public int Kcal { get => kcal; set => kcal = value; }
        [Column]
        public double Carbohydrate { get => carbohydrate; set => carbohydrate = value; }
        [Column]
        public double Fat { get => fat; set => fat = value; }
        [Column]
        public double Protein { get => protein; set => protein = value; }
        [Column]
        public int IsUnit { get => isUnit; set => isUnit = value; }
        [Column]
        public string Name { get => name; set => name = value; }


        [Association(Name = "FK_ProductMeal_Products", Storage = "_productMeals",
            OtherKey ="productId", ThisKey ="Id")]
        public ICollection<ProductMeal> ProductMeals
        {
            get { return _productMeals; }
            set { _productMeals.Assign(value); }
        }

        override
        public string ToString()
        {
            return Name + " Kcal: " + Kcal + " Białko: " + Protein + " Tłuszcz: " + Fat + " Węglowodany: " + Carbohydrate ;
        }
    }
}
