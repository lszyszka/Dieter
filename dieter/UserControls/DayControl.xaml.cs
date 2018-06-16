using dieter.DialogWindows;
using dieter.DieterUtils;
using dieter.Models;
using Dieter;
using Dieter.Models;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for DayControl.xaml
    /// </summary>
    public partial class DayControl : UserControl
    {


        DieterDBM dieterDBM;
        IEnumerable<Meal> meals;
        int dayId;

        public DayControl(int id)
        {
            InitializeComponent();
            dayId = id;
            RefreshDayNutritionalContents();
            InitTitleLabel();
        }

        private void InitTitleLabel()
        {
            dieterDBM = new DieterDBM();
            var currentDay = (from dbDay in dieterDBM.Days where dbDay.Id == dayId select dbDay).First();
            titleLabel.DataContext = currentDay;
            dieterDBM.Dispose();
        }

        private void InitMealsList()
        {
            dieterDBM = new DieterDBM();
            meals = from meal in dieterDBM.Meals where meal.Day.Id == dayId select meal;
            mealsListBox.ItemsSource = meals;
            dieterDBM.Dispose();
        }

        private void AddMeal(object sender, RoutedEventArgs e)
        {
            dieterDBM = new DieterDBM();
            var currentDay = (from dbDay in dieterDBM.Days where dbDay.Id == dayId select dbDay).First();
            currentDay.Meals.Add(new Meal());
            dieterDBM.SubmitChanges();
            dieterDBM.Dispose();
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
            ((MainWindow)Application.Current.MainWindow).SetMealControl(id, dayId);
        }

        public void RefreshDayNutritionalContents()
        {
            dieterDBM = new DieterDBM();
            var currentDay = (from dbDay in dieterDBM.Days where dbDay.Id == dayId select dbDay).First();
            SumNutritionalContents(currentDay);
            dieterDBM.SubmitChanges();
            dieterDBM.Dispose();
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
            dieterDBM.Dispose();
            InitMealsList();
        }

        private void EndClick(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).SetMainControl();            
        }
    }
}
