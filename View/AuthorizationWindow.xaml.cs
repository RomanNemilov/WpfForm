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

namespace WpfForm
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
            this.Closing += AuthorizationWindow_Closing;
        }
        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void AuthorizationWindow_Closing(object sender, EventArgs e)
        {
            Owner.Show();
        }
        private void Login(object sender, RoutedEventArgs e)
        {
            if(LoginTextBox.Text == "romkapro" && PasswordBox.Password == "admin") 
            {
                CatalogEditWindow catalogEditWindow = new CatalogEditWindow();
                catalogEditWindow.Owner = this.Owner;
                catalogEditWindow.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Попробуйте ещё раз", "Введён неверный пароль!!!!!!!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
