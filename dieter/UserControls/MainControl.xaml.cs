using dieter.DialogWindows;
using dieter.DieterUtils;
using dieter.Models;
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
                dieterDBM = new DieterDBM();
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
