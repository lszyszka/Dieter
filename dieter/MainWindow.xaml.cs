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
using dieter.UserControls;

namespace Dieter
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
           InitializeComponent();
            SetMainControl();
        }

        public void SetMainControl()
        {
            MainGrid.Children.Clear();
            MainGrid.Children.Add(new MainControl());
        }

        public void SetDayControl(int id)
        {
            MainGrid.Children.Clear();
            MainGrid.Children.Add(new DayControl(id));
        }

        public void SetMealControl(int id, int dayId)
        {
            MainGrid.Children.Clear();
            MainGrid.Children.Add(new MealControl(id, dayId));
        }
    }
}
