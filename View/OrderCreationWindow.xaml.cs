using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using WpfForm.View;
using Excel = Microsoft.Office.Interop.Excel;

namespace WpfForm
{
    /// <summary>
    /// Логика взаимодействия для OrderCreationWindow.xaml
    /// </summary>
    public partial class OrderCreationWindow : Window
    {

        public OrderCreationWindow()
        {
            InitializeComponent();
            this.Closing += OrderCreationWindow_Closing;
            listBoxCategory.Items.Clear();
            foreach (Excel.Worksheet worksheet in App.Workbook.Worksheets)
            {
                listBoxCategory.Items.Add(worksheet.Name);
            }
            listBoxProducts.Items.Clear();
            UpdateCostBlock();
        }
        
        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void OrderCreationWindow_Closing(object sender, EventArgs e)
        {
            Owner.Show();
        }
        private void ListBoxCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.UpdateProducts((string)listBoxCategory.SelectedValue);
            listBoxProducts.ItemsSource = null;
            listBoxProducts.ItemsSource = App.Products;
        }
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            Product product = (sender as Button).DataContext as Product;
            App.Cart.AddProduct(product);
            UpdateCostBlock();
        }
        private void UpdateCostBlock()
        {
            TextBlockCost.Text = String.Format("Стоимость услуг в корзине: ${0}", App.Cart.TotalCost);
        }
        private void ButtonCart_Click(object sender, EventArgs e)
        {
            CartWindow cart = new CartWindow();
            cart.Owner = this;
            cart.Show();
            this.Hide();
        }
    }
}
