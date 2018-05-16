using dieter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace dieter.DialogWindows
{
    /// <summary>
    /// Logika interakcji dla klasy ProductDialog.xaml
    /// </summary>
    public partial class ProductDialog : Window
    {
        DieterDBM dieterDBM = new DieterDBM();
        Product newProduct = new Product();

        public ProductDialog()
        {
            InitializeComponent();
            DataContext = newProduct;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            newProduct.IsUnit = 1;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            newProduct.IsUnit = 0;
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            dieterDBM.Products.InsertOnSubmit(newProduct);
            dieterDBM.SubmitChanges();
            MessageBox.Show("Dodano product.");
            DialogResult = true;           
        }
    }
}
