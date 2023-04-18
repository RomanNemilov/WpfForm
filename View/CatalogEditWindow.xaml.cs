﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
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
            //MessageBox.Show(listBoxCategory.SelectedIndex.ToString());
            if(listBoxCategory.SelectedIndex != -1)
            {
                App.UpdateProducts((string)listBoxCategory.SelectedValue);
                ListBoxProducts.ItemsSource = null;
                ListBoxProducts.ItemsSource = App.Products;
                TextBoxCategory.Text = (string)listBoxCategory.SelectedValue;
            }
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
            if (result == MessageBoxResult.No) return;
            Excel.Worksheet sheet = App.Workbook.Worksheets[listBoxCategory.SelectedValue];
            App.ExcelApp.DisplayAlerts = false;
            sheet.Delete();
            App.ExcelApp.DisplayAlerts = true;
            App.Workbook.Save();
            UpdateListCategory();
        }

        private void ButtonAddCategory_Click(object sender, RoutedEventArgs e)
        {
            //App.Workbook.Sheets[App.Workbook.Sheets.Count].Select();
            Excel.Worksheet sheet;
            if (listBoxCategory.SelectedIndex != -1)
            {
                Excel.Worksheet selectedSheet = App.Workbook.Sheets[listBoxCategory.SelectedValue];
                sheet = App.Workbook.Worksheets.Add(After: selectedSheet);
            }
            else
            {
                Excel.Worksheet lastSheet = App.Workbook.Sheets[App.Workbook.Sheets.Count];
                sheet = App.Workbook.Worksheets.Add(After: lastSheet);
            }
            sheet.Name = TextBoxCategory.Text;
            UpdateListCategory();
            App.Workbook.Save();
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
                int rowCount = range.Rows.Count;
                for (int i = 1; i <= rowCount; i++)
                {
                    //MessageBox.Show(((Product)ListBoxProducts.SelectedItem).Name, sheet.Cells[i, 1].Value);
                    if (sheet.Cells[i, 1].Value == ((Product)ListBoxProducts.SelectedItem).Name &&
                        sheet.Cells[i, 2].Value == ((Product)ListBoxProducts.SelectedItem).Cost)
                    {
                        Excel.Range rng = (Excel.Range)sheet.Rows[i];
                        rng.Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
                        i--;                                                  //После удаления строки со смещением нужно проверить строку ещё раз
                        rowCount--;                                           //И уменьшить количество строк
                        //MessageBox.Show("удалили 100%");
                    }
                }
                // range.Rows[ListBoxProducts.SelectedValue].Cells.Clear(); //Не работает(
                App.UpdateProducts(listBoxCategory.SelectedItem.ToString());
                ListBoxProducts.ItemsSource = null;
                ListBoxProducts.ItemsSource = App.Products;
                App.Workbook.Save();
            }
        }

        private void ButtonAddProduct_Click(object sender, RoutedEventArgs e)
        {
            Excel.Worksheet sheet = App.Workbook.Worksheets[listBoxCategory.SelectedValue];
            Excel.Range range = sheet.UsedRange;
            int rowCount = range.Rows.Count;
            if (sheet.Cells[1, 1].Value == null || sheet.Cells[1, 2].Value == null) rowCount = 0; //Без этого не работает
            sheet.Cells[rowCount + 1, 1].Value = TextBoxProductName.Text;
            sheet.Cells[rowCount + 1, 2].Value = Convert.ToDouble(TextBoxProductCost.Text);
            App.UpdateProducts((string)listBoxCategory.SelectedValue);
            ListBoxProducts.ItemsSource = null;
            ListBoxProducts.ItemsSource = App.Products;
            App.Workbook.Save();
        }

        private void ButtonEditCategory_Click(object sender, RoutedEventArgs e)
        {
            Excel.Worksheet sheet = App.Workbook.Worksheets[listBoxCategory.SelectedValue];
            sheet.Name = TextBoxCategory.Text;
            UpdateListCategory();
            //App.Workbook.Save();
        }

        private void ButtonEditProduct_Click(object sender, RoutedEventArgs e)
        {
            Excel.Worksheet sheet = App.Workbook.Worksheets[listBoxCategory.SelectedValue];
            Excel.Range range = sheet.UsedRange;
            int rowCount = range.Rows.Count;
            for (int i = 1; i <= rowCount; i++)
            {
                MessageBox.Show(((Product)ListBoxProducts.SelectedItem).Name, sheet.Cells[i, 1].Value);
                if (sheet.Cells[i, 1].Value == ((Product)ListBoxProducts.SelectedItem).Name &&
                    sheet.Cells[i, 2].Value == ((Product)ListBoxProducts.SelectedItem).Cost)
                {
                    sheet.Cells[i, 1].Value = TextBoxProductName.Text;
                    sheet.Cells[i, 2].Value = Convert.ToDouble(TextBoxProductCost.Text);
                }
            }
            // range.Rows[ListBoxProducts.SelectedValue].Cells.Clear(); //Не работает(
            App.UpdateProducts(listBoxCategory.SelectedItem.ToString());
            ListBoxProducts.ItemsSource = null;
            ListBoxProducts.ItemsSource = App.Products;
            App.Workbook.Save();
        }
    }
}
