using dieter.Models;
using Dieter.Models;
using dieter.DieterUtils;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;

namespace dieter.DialogWindows
{
    /// <summary>
    /// Logika interakcji dla klasy DayDialog.xaml
    /// </summary>
    public partial class DayDialog : Window
    {
        DieterDBM dieterDBM = new DieterDBM();
        IEnumerable<Meal> meals;
        int dayId;

        public DayDialog(int id)
        {
            InitializeComponent();
            dayId = id;
            InitMealsList();
            var currentDay = (from dbDay in dieterDBM.Days where dbDay.Id == dayId select dbDay).First();
            titleLabel.DataContext = currentDay;
        }

        private void InitMealsList()
        {
            dieterDBM = new DieterDBM();
            meals = from meal in dieterDBM.Meals where meal.Day.Id == dayId select meal;
            mealsListBox.ItemsSource = meals;            
        }

        private void AddMeal(object sender, RoutedEventArgs e)
        {
            var currentDay = (from dbDay in dieterDBM.Days where dbDay.Id == dayId select dbDay).First();
            currentDay.Meals.Add(new Meal());            
            dieterDBM.SubmitChanges();
            InitMealsList();
        }

        private void SumNutritionalContents(Day day)
        {
            int sumKcal = 0;
            double sumProtein = 0;
            double sumFat = 0;
            double sumCarbo = 0;
            IEnumerable<Meal> meals = day.Meals;
            foreach (Meal meal in meals)
            {
                sumKcal = sumKcal + meal.Kcal;
                sumProtein = sumProtein + meal.Protein;
                sumFat = sumFat + meal.Fat;
                sumCarbo = sumCarbo + meal.Carbohydrate;
            }
            day.Kcal = sumKcal;
            day.Fat = sumFat;
            day.Protein = sumProtein;
            day.Carbohydrate = sumCarbo;
        }

        private void EditMealClick(object sender, RoutedEventArgs e)
        {
            int id = Utils.GetIdFromUGrid((UniformGrid)((Button)sender).Parent);   
            MealDialog mealDialog = new MealDialog(id, dayId);
            mealDialog.ShowDialog();

            dieterDBM = new DieterDBM();
            var currentDay = (from dbDay in dieterDBM.Days where dbDay.Id == dayId select dbDay).First();
            SumNutritionalContents(currentDay);
            dieterDBM.SubmitChanges();
            InitMealsList();
        }

        private void DeleteMealClick(object sender, RoutedEventArgs e)
        {
            dieterDBM = new DieterDBM();
            int id = Utils.GetIdFromUGrid((UniformGrid)((Button)sender).Parent);
            var removedMeal = dieterDBM.Meals.Single(m => m.Id == id);
            dieterDBM.Meals.DeleteOnSubmit(removedMeal);
            dieterDBM.SubmitChanges();

            var currentDay = dieterDBM.Days.Single(d => d.Id == dayId);
            SumNutritionalContents(currentDay);
            dieterDBM.SubmitChanges();
            InitMealsList();
        }
    }
}
