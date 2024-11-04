using System;
using System.Threading.Tasks;
using System.Windows;
using Maquina_OKv2.View;
using Maquina_OKv2.View.Animation_windows;

namespace Maquina_OKv2.View
{
    public partial class Loading_start : Window
    {
        public Loading_start()
        {
            InitializeComponent();
            ShowLoadingScreen();
        }

        private async void ShowLoadingScreen()
        {
            Helper.FadeIn(this);
            // Espera 3 segundos antes de pasar a la siguiente ventana
            //await Task.Delay(3000);

            // Crear y mostrar la nueva ventana
            Login login = new Login();
            login.Show();

            // Cierra la ventana de carga
            Helper.FadeOutAndClose(this);
            //this.Close();
        }
    }
}
