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
using Maquina_OKv2.View;
using Maquina_OKv2.View.Animation_windows;

namespace Maquina_OKv2.View.ControUserView
{
    /// <summary>
    /// Lógica de interacción para Account2.xaml
    /// </summary>
    public partial class Account2 : UserControl
    {
        public Account2()
        {
            InitializeComponent();
        }

        private void account_exitsesion_Click(object sender, RoutedEventArgs e)
        {
            // Mostrar la ventana de inicio de sesión
            Login login = new Login();
            login.Show();

            // Obtener la ventana que contiene este UserControl
            Window parentWindow = Window.GetWindow(this);

            // Cerrar la ventana si se encontró
            if (parentWindow != null)
            {
                parentWindow.Close();
            }

        }
    }
}
