using dieter.DialogWindows;
using dieter.DieterUtils;
using dieter.Models;
using Dieter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dieter.UserControls
{
    /// <summary>
    /// Interaction logic for MealControl.xaml
    /// </summary>
    public partial class MealControl : UserControl
    {

        DieterDBM dieterDBM = new DieterDBM();
        IEnumerable<ProductMeal> productMeals;
        IEnumerable<Product> products;
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
            productMeals = from productMeal in dieterDBM.ProductMeal where productMeal.Meal.Id == mealId select productMeal;
            productListBox.ItemsSource = productMeals;
        }

        private void InitCombo()
        {
            products = from product in dieterDBM.Products select product;
            productsComboBox.ItemsSource = products;
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
            var currentMeal = (from meal in dieterDBM.Meals where meal.Id == mealId select meal).First();
            ProductMeal productMeal = MakeProductMeal(amount);
            currentMeal.ProductMeals.Add(productMeal);
            SumNutritionalContents(currentMeal);
            dieterDBM.SubmitChanges();
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
                productMeal.Protein = Convert.ToInt32(currentProduct.Protein * amount);
                productMeal.Fat = Convert.ToInt32(currentProduct.Fat * amount);
                productMeal.Carbohydrate = Convert.ToInt32(currentProduct.Carbohydrate * amount);
            }
            else
            {
                productMeal.Amount = amount;
                productMeal.Product = currentProduct;
                productMeal.Kcal = Convert.ToInt32(currentProduct.Kcal * (amount / 100));
                productMeal.Protein = Convert.ToInt32(currentProduct.Protein * (amount / 100));
                productMeal.Fat = Convert.ToInt32(currentProduct.Fat * (amount / 100));
                productMeal.Carbohydrate = Convert.ToInt32(currentProduct.Carbohydrate * (amount / 100));
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
            ProductMeal removedProductMeal = (from productMeal in dieterDBM.ProductMeal where productMeal.Id == id select productMeal).Single();
            dieterDBM.ProductMeal.DeleteOnSubmit(removedProductMeal);
            dieterDBM.SubmitChanges();

            var currentMeal = (from meal in dieterDBM.Meals where meal.Id == mealId select meal).First();
            SumNutritionalContents(currentMeal);
            dieterDBM.SubmitChanges();
            InitProductMealsList();

        }

        private void DeleteProductClick(object sender, RoutedEventArgs e)
        {
            Product product = (Product)productsComboBox.SelectedItem;
            try
            {
                dieterDBM.Products.DeleteOnSubmit(product);
                dieterDBM.SubmitChanges();
                InitCombo();
            }
            catch
            {
                MessageBox.Show("Produkt jest używany nie można usunąć");
            }
        }

        private void EndClick(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).SetDayControl(dayId);
        }
    }
}
