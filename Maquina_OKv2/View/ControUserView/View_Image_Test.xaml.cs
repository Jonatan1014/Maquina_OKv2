using System.Windows;
using System.Windows.Controls;

namespace Maquina_OKv2.View.ControUserView
{
    public partial class View_Image_Test : UserControl
    {
        private int _normaId;       // Campo para almacenar el Id de la norma
        private string _normaTitle; // Campo para almacenar el título de la norma

        public View_Image_Test()
        {
            InitializeComponent();
        }

        // Método para configurar el texto, la imagen y los datos de la norma seleccionada
        public void SetNormText(string normText, byte[] imageBytes, int normaId, string normaTitle)
        {
            normTextBlock.Text = normText;
            _normaId = normaId;       // Guardar el Id de la norma
            _normaTitle = normaTitle; // Guardar el título de la norma

            // Configurar la imagen si se proporciona
            if (imageBytes != null && imageBytes.Length > 0)
            {
                using (var ms = new System.IO.MemoryStream(imageBytes))
                {
                    var image = new System.Windows.Media.Imaging.BitmapImage();
                    image.BeginInit();
                    image.StreamSource = ms;
                    image.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                    image.EndInit();
                    normImage.Source = image;
                }
            }
            else
            {
                normImage.Source = null;
            }
        }

        // Evento que se ejecuta al hacer clic en el StackPanel
        private void StackPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Crear una instancia de la ventana Register_Test y pasar el Id y el título de la norma
            var registerTestWindow = new Register_Test(_normaId, _normaTitle);
            registerTestWindow.Show(); // Mostrar la ventana
        }
    }
}
