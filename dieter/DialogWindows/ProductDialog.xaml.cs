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
            NameTB.Focus();
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
            if (!HasValidationErrors)
            {
                if (HasNull)
                {
                    BindingExpression bindingExpression = NameTB.GetBindingExpression(TextBox.TextProperty);
                    bindingExpression.UpdateSource();
                    bindingExpression = KcalTB.GetBindingExpression(TextBox.TextProperty);
                    bindingExpression.UpdateSource();
                    bindingExpression = CarboTB.GetBindingExpression(TextBox.TextProperty);
                    bindingExpression.UpdateSource();
                    bindingExpression = ProteinTB.GetBindingExpression(TextBox.TextProperty);
                    bindingExpression.UpdateSource();
                    bindingExpression = FatTB.GetBindingExpression(TextBox.TextProperty);
                    bindingExpression.UpdateSource();
                }
                else
                {
                    dieterDBM.Products.InsertOnSubmit(newProduct);
                    dieterDBM.SubmitChanges();
                    MessageBox.Show("Dodano produkt.");
                    DialogResult = true;
                }                
            }
            else
            {
                MessageBox.Show("Błędne dane.");
            }
                 
        }

        private bool HasValidationErrors
        {
            get { return Validation.GetHasError(NameTB) || Validation.GetHasError(KcalTB) || Validation.GetHasError(ProteinTB) || Validation.GetHasError(FatTB) || Validation.GetHasError(CarboTB); }
        }

        private bool HasNull
        {
            get { return ((Product)DataContext).Name == null || ((Product)DataContext).Kcal == null || ((Product)DataContext).Protein == null || ((Product)DataContext).Fat == null || ((Product)DataContext).Carbohydrate == null; }
        }
    }
}
