using System;
using System.Collections.Generic;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace WpfForm
{
    /// <summary>
    /// Логика взаимодействия для CatalogEdit.xaml
    /// </summary>
    public partial class CatalogEditWindow : Window
    {
        public CatalogEditWindow()
        {
            InitializeComponent();
            this.Closing += CatalogEditWindow_Closing;
            UpdateListCategory();
        }
        private void ListBoxCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.UpdateProducts((string)listBoxCategory.SelectedValue);
            ListBoxProducts.ItemsSource = null;
            ListBoxProducts.ItemsSource = App.Products;
            TextBoxCategory.Text = (string)listBoxCategory.SelectedValue;
        }
        private void ListBoxProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Product product = (Product)ListBoxProducts.SelectedValue;
            if(product != null)
            {
                TextBoxProductName.Text = product.Name;
                TextBoxProductCost.Text = product.Cost.ToString();
                Uri uri = new Uri(product.Photo, UriKind.Absolute);
                ImageProduct.Source = new BitmapImage(uri);
            }
        }
        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void CatalogEditWindow_Closing(object sender, EventArgs e)
        {
            Owner.Show();
        }

        private void ButtonDeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы удаляете категорию без возможности восстановления. \nВы уверены что хотите это сделать?", "Внмиание !!!", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                App.Workbook.Worksheets[listBoxCategory.SelectedValue].Delete();
                App.Workbook.Save();
                UpdateListCategory();
            }
        }

        private void ButtonAddCategory_Click(object sender, RoutedEventArgs e)
        {
            Excel.Worksheet sheet = App.Workbook.Worksheets.Add();
            sheet.Name = TextBoxCategory.Text;
            App.Workbook.Save();
            UpdateListCategory();
        }
        private void UpdateListCategory()
        {
            listBoxCategory.Items.Clear();
            foreach (Excel.Worksheet worksheet in App.Workbook.Worksheets)
            {
                listBoxCategory.Items.Add(worksheet.Name);
            }
        }

        private void ButtonDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы удаляете услугу без возможности восстановления. \nВы уверены что хотите это сделать?", "Внмиание !!!", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Excel.Worksheet sheet = App.Workbook.Worksheets[listBoxCategory.SelectedValue];
                Excel.Range range = sheet.UsedRange;
                //int rowCount = range.Rows.Count;
                //for (int i = 1; i < rowCount; i++)
                //{

                //}
                range.Rows[ListBoxProducts.SelectedValue].Cells.Clear(); //Не работает(
                //App.Workbook.Save();
                UpdateListCategory();
            }
        }

        private void ButtonAddProduct_Click(object sender, RoutedEventArgs e)
        {
            Excel.Worksheet sheet = App.Workbook.Worksheets[listBoxCategory.SelectedValue];
            Excel.Range range = sheet.UsedRange;
            sheet.Cells[range.Rows.Count + 1, 1].Value = TextBoxProductName.Text;
            sheet.Cells[range.Rows.Count + 1, 2].Value = Convert.ToDouble(TextBoxProductCost.Text);
            App.UpdateProducts((string)listBoxCategory.SelectedValue);
            ListBoxProducts.ItemsSource = null;
            ListBoxProducts.ItemsSource = App.Products;
            App.Workbook.Save();
        }
    }
}
