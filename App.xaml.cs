using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
//using Microsoft.Office.Interop.Word;

namespace WpfForm
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Excel.Application ExcelApp { get; set; }
        public static Excel.Workbook Workbook { get; set; }
        public static bool Closing { get; set; } = false;
        public static string FilePath { get; set; } = Environment.CurrentDirectory + @"\Prices.xlsx";
        internal static ShoppingCart Cart { get; set; } = new ShoppingCart();
        internal static List<Product> Products { get; set; } = new List<Product>();
        internal static void UpdateProducts(string categoryName)
        {
            Products.Clear();
            Excel.Worksheet worksheet = Workbook.Worksheets[categoryName];
            Excel.Range range = worksheet.UsedRange;
            int rowCount = range.Rows.Count;
            for (int i = 1; i <= rowCount; i++)
            {
                Product product = new Product();
                product.Name = worksheet.Cells[i, 1].Text;
                product.Cost = (int)worksheet.Cells[i, 2].Value;
                string fileName = Environment.CurrentDirectory + "\\photos\\" + product.Name + ".png";
                if (File.Exists(fileName))
                {
                    product.Photo = fileName;
                }
                else
                {
                    product.Photo = Environment.CurrentDirectory + @"\default.png";
                }
                Products.Add(product);
            }
        }
        //public static string FilePath { get; set; } = @"..\..\..\Resources\Prices.xlsx";
        //public static 
    }
}
