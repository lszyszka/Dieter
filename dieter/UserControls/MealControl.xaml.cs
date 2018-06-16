using dieter.DialogWindows;
using dieter.DieterUtils;
using dieter.Models;
using Dieter;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace dieter.UserControls
{
    /// <summary>
    /// Interaction logic for MealControl.xaml
    /// </summary>
    public partial class MealControl : UserControl
    {

        DieterDBM dieterDBM;
        int mealId;
        int dayId;

        public MealControl(int id, int day)
        {
            InitializeComponent();
            mealId = id;
            dayId = day;
            Init();
        }

        private void Init()
        {
            InitProductMealsList();
            InitCombo();
        }

        private void InitProductMealsList()
        {
            dieterDBM = new DieterDBM();
            DataLoadOptions loadOptions = new DataLoadOptions();
            loadOptions.LoadWith<ProductMeal>(PM => PM.Product);
            dieterDBM.LoadOptions = loadOptions;
            var productMeals = from productMeal in dieterDBM.ProductMeal where productMeal.Meal.Id == mealId select productMeal;
            productListBox.ItemsSource = productMeals;
            dieterDBM.Dispose();
        }

        private void InitCombo()
        {
            dieterDBM = new DieterDBM();
            var  products = from product in dieterDBM.Products orderby product.Name select product;
            productsComboBox.ItemsSource = products;
            dieterDBM.Dispose();
        }

        private void AddProductButtonClick(object sender, RoutedEventArgs e)
        {
            if (productsComboBox.SelectedItem != null && Double.TryParse(amountTBox.Text, NumberStyles.Float, new CultureInfo("en-US"), out double amount))
            {
                AddProductToMeal(amount);
            }
            else
            {
                MessageBox.Show("Błędne dane");
            }
        }

        private void AddProductToMeal(double amount)
        {
            dieterDBM = new DieterDBM();
            ProductMeal productMeal = MakeProductMeal(amount);            
            var currentMeal = (from meal in dieterDBM.Meals where meal.Id == mealId select meal).First();
            currentMeal.ProductMeals.Add(productMeal);
            SumNutritionalContents(currentMeal);
            dieterDBM.SubmitChanges();
            dieterDBM.Dispose();
            Clear();
            InitProductMealsList();
        }

        private void Clear()
        {
            productsComboBox.SelectedItem = null;
            amountTBox.Text = null;
        }

        private void SumNutritionalContents(Meal meal)
        {
            int sumKcal = 0;
            double sumProtein = 0;
            double sumFat = 0;
            double sumCarbo = 0;
            IEnumerable<ProductMeal> productMeals = meal.ProductMeals;
            foreach (ProductMeal productMeal in productMeals)
            {
                sumKcal = sumKcal + productMeal.Kcal;
                sumProtein = sumProtein + productMeal.Protein;
                sumFat = sumFat + productMeal.Fat;
                sumCarbo = sumCarbo + productMeal.Carbohydrate;
            }
            meal.Kcal = sumKcal;
            meal.Fat = sumFat;
            meal.Protein = sumProtein;
            meal.Carbohydrate = sumCarbo;
        }


        private ProductMeal MakeProductMeal(Double amount)
        {
            Product product = (Product)productsComboBox.SelectedItem;           
            var currentProduct = (from p in dieterDBM.Products where p.Id == product.Id select p).First();
            ProductMeal productMeal = new ProductMeal();
            if (currentProduct.IsUnit == 1)
            {
                productMeal.Amount = amount;
                productMeal.Product = currentProduct;
                productMeal.Kcal = Convert.ToInt32(currentProduct.Kcal * amount);
                productMeal.Protein = Convert.ToDouble(currentProduct.Protein * amount);
                productMeal.Fat = Convert.ToDouble(currentProduct.Fat * amount);
                productMeal.Carbohydrate = Convert.ToDouble(currentProduct.Carbohydrate * amount);
            }
            else
            {
                productMeal.Amount = amount;
                productMeal.Product = currentProduct;
                productMeal.Kcal = Convert.ToInt32(currentProduct.Kcal * (amount / 100));
                productMeal.Protein = Convert.ToDouble(currentProduct.Protein * (amount / 100));
                productMeal.Fat = Convert.ToDouble(currentProduct.Fat * (amount / 100));
                productMeal.Carbohydrate = Convert.ToDouble(currentProduct.Carbohydrate * (amount / 100));
            }
            return productMeal;
        }

        private void AddNewProductClick(object sender, RoutedEventArgs e)
        {
            ProductDialog productDialog = new ProductDialog();
            if (true == productDialog.ShowDialog())
            {
                InitCombo();
            }
        }

        private void DeleteProductFromMealClick(object sender, RoutedEventArgs e)
        {
            int id = Utils.GetIdFromUGrid((UniformGrid)((Button)sender).Parent);
            dieterDBM = new DieterDBM();
            ProductMeal removedProductMeal = (from productMeal in dieterDBM.ProductMeal where productMeal.Id == id select productMeal).Single();
            dieterDBM.ProductMeal.DeleteOnSubmit(removedProductMeal);
            dieterDBM.SubmitChanges();

            var currentMeal = (from meal in dieterDBM.Meals where meal.Id == mealId select meal).First();
            SumNutritionalContents(currentMeal);
            dieterDBM.SubmitChanges();
            dieterDBM.Dispose();
            InitProductMealsList();

        }

        private void DeleteProductClick(object sender, RoutedEventArgs e)
        {
            dieterDBM = new DieterDBM();
            Product product = (Product)productsComboBox.SelectedItem;
            var deletedProduct = (from p in dieterDBM.Products where p.Id == product.Id select p).Single();
            try
            {
                dieterDBM.Products.DeleteOnSubmit(deletedProduct);
                dieterDBM.SubmitChanges();
                dieterDBM.Dispose();
                InitCombo();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Produkt jest używany nie można usunąć");
            }
        }

        private void EndClick(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).SetDayControl(dayId);
        }
    }
}
