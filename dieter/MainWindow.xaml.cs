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
using dieter.DialogWindows;
using dieter.Models;
using dieter.DieterUtils;
using Dieter.Models;

namespace Dieter
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DieterDBM dieterDBM;
        IEnumerable<Day> days;
        
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                InitDaysList();
                dieterDBM = new DieterDBM();
            }catch(Exception e)
            {
                MessageBox.Show("Wyjateczek " + e.Message);
            }            
        }

        private void EditDayClick(object sender, RoutedEventArgs e)
        {
            int id = Utils.GetIdFromUGrid((UniformGrid)((Button)sender).Parent);            
            DayDialog dayDialog = new DayDialog(id);
            dayDialog.ShowDialog();
            InitDaysList();
        }      

        private void InitDaysList()
        {
            dieterDBM = new DieterDBM();
            days = from day in dieterDBM.Days select day;
            dayListBox.ItemsSource = days;
        }       

        private void AddDay(object sender, RoutedEventArgs e)
        {            
            dieterDBM.Days.InsertOnSubmit(new Day());
            dieterDBM.SubmitChanges();
            InitDaysList();            
        }

        private void DeleteDayClick(object sender, RoutedEventArgs e)
        {
            int id = Utils.GetIdFromUGrid((UniformGrid)((Button)sender).Parent);
            var day = (from d in dieterDBM.Days where d.Id == id select d).Single();
            dieterDBM.Days.DeleteOnSubmit(day);
            dieterDBM.SubmitChanges();
            InitDaysList();
        }
    }
}
