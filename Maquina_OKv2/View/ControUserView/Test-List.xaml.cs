using System.Windows;
using System.Windows.Controls;

namespace Maquina_OKv2.View.ControUserView
{
    public partial class Test_List : UserControl
    {
        public Test_List()
        {
            InitializeComponent();
        }

        private void search_ntc_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Lógica para filtrar las normas en el ListBox
        }

        private void ntcListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Verificar si hay un elemento seleccionado
            if (ntcListBox.SelectedItem != null)
            {
                // Obtener el texto de la norma seleccionada
                var selectedNorm = (ntcListBox.SelectedItem as ListBoxItem).Content.ToString();

                // Crear una instancia del UserControl de detalles
                var viewImageTest = new View_Image_Test();

                // Configurar el texto de la norma seleccionada en el nuevo UserControl
                viewImageTest.SetNormText(selectedNorm);

                // Mostrar el UserControl en el ContentControl
                detailsContentControl.Content = viewImageTest;
            }
        }
    }
}
