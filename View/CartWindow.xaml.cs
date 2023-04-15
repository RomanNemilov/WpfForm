using System;
using System.Collections.Generic;
using System.Data;
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
using Word = Microsoft.Office.Interop.Word;

namespace WpfForm.View
{
    /// <summary>
    /// Логика взаимодействия для CartWindow.xaml
    /// </summary>
    public partial class CartWindow : Window
    {
        DataTable tableCart = new DataTable("Cart");

        public CartWindow()
        {
            InitializeComponent();
            //this.Activated += Window_Activated; 
            this.Closing += CartWindow_Closing;
            tableCart.Columns.Add("Name", typeof(string));
            tableCart.Columns.Add("Cost", typeof(string));
            tableCart.Columns.Add("Quantity", typeof(int));
            tableCart.Columns.Add("Sum", typeof(string));
            UpdateGrid();
        }
        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateGrid()
        {
            tableCart.Clear();
            foreach(ProductInOrder product in App.Cart.Products)
            {
                object[] item = new object[4];
                item[0] = product.Name;
                item[1] = "$" + product.Cost;
                item[2] = product.Quantity;
                item[3] = "$" + product.Cost * product.Quantity;
                tableCart.Rows.Add(item);
            }
            //DataGridCart.ItemsSource = null;
            DataGridCart.DataContext = tableCart;
        }

        //private void Window_Activated(object sender, EventArgs e)
        //{

        //}
        private void CartWindow_Closing(object sender, EventArgs e)
        {
            Owner.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Word.Application wordApp = null!;
            try
            {
                wordApp = new Word.Application();
                wordApp.Visible = false;
            }
            catch
            {
                MessageBox.Show("Не");
            }
            Word.Document wordDoc = wordApp.Documents.Add();
            Word.Paragraph wordPar = wordDoc.Paragraphs.Add();
            //wordPar.set_Style("Заголовок 1");
            wordDoc.Content.Font.Size = 16;
            //wordPar.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            Word.Range wordRange = wordPar.Range;
            wordRange.Text = "Спасибо за заказ у нас, анонимусов!";
            Word.InlineShape wordShape = wordDoc.InlineShapes.AddPicture(Environment.CurrentDirectory + "/anonymous_logo.png", Type.Missing, Type.Missing, Type.Missing);
            wordShape.Width = 100;
            wordShape.Height = 100;

            wordRange.InsertParagraphAfter();
            wordPar = wordDoc.Paragraphs.Add();
            wordRange = wordPar.Range;
            wordRange.Text = "Заказ был оформлен: " + DateTime.Now.ToLongDateString();

            wordRange.InsertParagraphAfter();
            wordPar = wordDoc.Paragraphs.Add();
            wordRange = wordPar.Range;
            wordRange.Text = "Список заказанных услуг";

            wordRange.InsertParagraphAfter();
            wordPar = wordDoc.Paragraphs.Add();
            wordRange = wordPar.Range;
            Word.Table wordTable = wordDoc.Tables.Add(wordRange, tableCart.Rows.Count + 1, tableCart.Columns.Count);
            wordTable.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            wordTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            for (int col = 0;  col < tableCart.Columns.Count; col++)
            {
                //wordTable.Cell(1, col + 1).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalTop;
                Word.Range range = wordTable.Cell(1, col + 1).Range;
                range.Text = DataGridCart.Columns[col].Header.ToString();
            }
            for (int row = 1; row < tableCart.Rows.Count + 1; row++)
            {
                for(int col = 0; col < tableCart.Columns.Count; col++)
                {
                    Word.Range tableRange = wordTable.Cell(row + 1, col + 1).Range;
                    tableRange.Text = tableCart.Rows[row - 1].ItemArray[col].ToString();
                }
            }

            wordRange.InsertParagraphAfter();
            wordPar = wordDoc.Paragraphs.Add();
            wordRange = wordPar.Range;
            wordRange.Text = "Общая стоимость заказа: $" + App.Cart.TotalCost;

            wordDoc.SaveAs(Environment.CurrentDirectory + "/чек.pdf", Word.WdExportFormat.wdExportFormatPDF);

            //wordDoc.Close();
            wordApp.Quit(Word.WdSaveOptions.wdDoNotSaveChanges);
        }
        private void ButtonPlus_Click(object sender, RoutedEventArgs e)
        {
            App.Cart.AddProduct(tableCart.Rows[DataGridCart.SelectedIndex].ItemArray[0] as string);
            UpdateGrid();
        }
        private void ButtonMinus_Click(object sender, RoutedEventArgs e)
        {
            App.Cart.RemoveProduct(tableCart.Rows[DataGridCart.SelectedIndex].ItemArray[0] as string);
            UpdateGrid();
        }

    }
}
