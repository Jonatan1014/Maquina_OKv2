using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Media;

namespace Maquina_OKv2.View.Animation_windows
{
    public static class Helper
    {
        // Método para desvanecer una ventana al aparecer
        public static void FadeIn(Window window)
        {
            DoubleAnimation fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1.5));
            window.BeginAnimation(Window.OpacityProperty, fadeIn);
            window.Show();
        }

        // Método para desvanecer una ventana al aparecer
        public static void FadeIn2(Window window)
        {
            DoubleAnimation fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
            window.BeginAnimation(Window.OpacityProperty, fadeIn);
            window.Show();
        }

        // Método para desvanecer una ventana al cerrar
        public static void FadeOutAndClose(Window window)
        {
            DoubleAnimation fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.3));
            fadeOut.Completed += (s, e) => window.Close();
            window.BeginAnimation(Window.OpacityProperty, fadeOut);
        }

        
    }
}
