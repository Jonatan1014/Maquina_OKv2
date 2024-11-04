using Maquina_OKv2.View.Animation_windows;
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
using Maquina_OKv2.View.ControUserView;
namespace Maquina_OKv2.View
{
    /// <summary>
    /// Lógica de interacción para Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        Welcome_udi welcome_Udi = new Welcome_udi();
        public Main()
        {
            InitializeComponent();
            
            Contenido.NavigationService.Navigate(welcome_Udi);
            
        }

        private void main_exit_Click(object sender, RoutedEventArgs e)
        {
            Helper.ZoomOutAndClose(this);
        }
        // Evento para mover la ventana
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void main_home_Click(object sender, RoutedEventArgs e)
        {
            Contenido.NavigationService.Navigate(welcome_Udi);

        }

        private void main_test_Click(object sender, RoutedEventArgs e)
        {
            Test_List test_List = new Test_List();
            Contenido.NavigationService.Navigate(test_List);
        }

        private void main_historytest_Click(object sender, RoutedEventArgs e)
        {
            History_test historytest = new History_test();
            Contenido.NavigationService.Navigate(historytest);
        }

        private void main_account_Click(object sender, RoutedEventArgs e)
        {
            Account2 account = new Account2();
            Contenido.NavigationService.Navigate(account);
        }
    }
}
