using System.Windows.Controls;

namespace Maquina_OKv2.View.ControUserView
{
    public partial class View_Image_Test : UserControl
    {
        public View_Image_Test()
        {
            InitializeComponent();
        }

        // Método para configurar el texto de la norma seleccionada
        public void SetNormText(string normText)
        {
            normTextBlock.Text = normText;
        }
    }
}
