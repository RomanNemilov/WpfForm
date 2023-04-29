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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace WpfForm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rnd = new Random();
        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainWindow_Closing;
            try
            {
                App.ExcelApp = new Excel.Application();
                //App.ExcelApp.Visible = true;
                //if (File.Exists(App.file))
            }
            catch
            {
                MessageBox.Show("Для работы приложения требуется Microsoft Excel", "Ошибка");
                this.Close();
            }
            try
            {
                App.Workbook = App.ExcelApp.Workbooks.Open(App.FilePath);
                App.Workbook.BeforeClose += Worksheet_Closing;
            }
            catch
            {
                MessageBox.Show("Не удалось отрыть файл с ценами");
                this.Close();
            }

        }
        private void PriceListClick(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Прайс-лист ещё не доступен", "Данный функционал находится в разработке");
            try
            {
                App.ExcelApp.Visible = true;
            }
            catch
            {
                MessageBox.Show("Не удалось отрыть файл с ценами");
                this.Close();
            }
            App.ExcelApp.Visible = true;
        }
        private void MakeOrderClick(object sender, RoutedEventArgs e)
        {
            //double balance = rnd.NextDouble() * 1000;
            //string message =  "На ней " + string.Format("{0:C2}", balance);
            //MessageBox.Show(message, "Мы заглянули на вашу карту...");
            OrderCreationWindow orderCreationWindow = new OrderCreationWindow();
            orderCreationWindow.Owner = this;
            orderCreationWindow.Show();
            this.Hide();
        }
        private void UpdateCatalog(object sender, RoutedEventArgs e)
        {
            //string password;
            //using (StreamReader reader = new StreamReader("..\\..\\..\\password.txt"))
            //{
            //    password = reader.ReadToEnd();
            //}
            //MessageBox.Show($"Для редактирования каталога товаров требуется ввод пароля({password})", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            AuthorizationWindow authorisationWindow = new AuthorizationWindow();
            authorisationWindow.Owner = this;
            authorisationWindow.Show();
            this.Hide();
        }
        private void ExitClick(object sender, EventArgs e)
        {
            this.Close();
        }
        private void MainWindow_Closing(Object sender, EventArgs e)
        {
            App.Closing = true;
            App.ExcelApp.Quit();
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(App.ExcelApp);
            GC.Collect();
        }
        private void Worksheet_Closing(ref bool Cancel)
        {
            if (!App.Closing)
            {
                Cancel = true;
                App.ExcelApp.Visible = false;
            }
        }
    }
}
