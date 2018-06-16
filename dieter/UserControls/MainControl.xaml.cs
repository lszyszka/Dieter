using dieter.DieterUtils;
using dieter.Models;
using Dieter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Dieter;

namespace dieter.UserControls
{
    /// <summary>
    /// Interaction logic for MainControl.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {

        DieterDBM dieterDBM;
        IEnumerable<Day> days;

        public MainControl()
        {
            try
            {
                InitializeComponent();
                InitDaysList();
            }
            catch (Exception e)
            {
                MessageBox.Show("Wyjateczek " + e.Message);
            }
        }
     

        private void EditDayClick(object sender, RoutedEventArgs e)
        {
            int id = Utils.GetIdFromUGrid((UniformGrid)((Button)sender).Parent);
            ((MainWindow)Application.Current.MainWindow).SetDayControl(id);
        }



        private void InitDaysList()
        {
            double carboSum=0;
            double protSum=0;
            double fatSum=0;
            int kcalSum=0;
            dieterDBM = new DieterDBM();
            days = from day in dieterDBM.Days select day;
            foreach(Day d in days)
            {
                carboSum = carboSum + d.Carbohydrate;
                protSum = protSum + d.Protein;
                fatSum = fatSum + d.Fat;
                kcalSum = kcalSum + d.Kcal;
            }
            dayListBox.ItemsSource = days;
            if (days.Count() > 0)
            {
                carboTB.Text = String.Format("{0:0.00}", carboSum / days.Count());
                protTB.Text = String.Format("{0:0.00}", protSum / days.Count());
                kcalTB.Text = kcalSum / days.Count() + "";
                fatTB.Text = String.Format("{0:0.00}", fatSum / days.Count());
            }
            else
            {
                carboTB.Text = null;
                protTB.Text = null;
                kcalTB.Text = null;
                fatTB.Text = null;
            }
            dieterDBM.Dispose();

        }

        private void AddDay(object sender, RoutedEventArgs e)
        {

            dieterDBM = new DieterDBM();
            dieterDBM.Days.InsertOnSubmit(new Day());
            dieterDBM.SubmitChanges();
            dieterDBM.Dispose();
            InitDaysList();
        }

        private void DeleteDayClick(object sender, RoutedEventArgs e)
        {
            int id = Utils.GetIdFromUGrid((UniformGrid)((Button)sender).Parent);
            dieterDBM = new DieterDBM();
            var day = (from d in dieterDBM.Days where d.Id == id select d).Single();
            dieterDBM.Days.DeleteOnSubmit(day);
            dieterDBM.SubmitChanges();
            dieterDBM.Dispose();
            InitDaysList();
        }
    }
}
