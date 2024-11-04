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
using Maquina_OKv2.View;
using Maquina_OKv2.View.Animation_windows;

namespace Maquina_OKv2.View
{
    /// <summary>
    /// Lógica de interacción para Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
            Helper.FadeIn2(this);
        }



        // Evento para mover la ventana
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        // Evento para cerrar la ventana
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Helper.ZoomOutAndClose(this);
            this.Close();
        }

        private void btn_register_Click(object sender, RoutedEventArgs e)
        {


        }

        private void btn_Return_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            Helper.FadeOutAndClose(this);
            //this.Close();
        }
    }
}
