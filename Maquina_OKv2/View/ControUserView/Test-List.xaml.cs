using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Maquina_OKv2.View.ControUserView
{
    public partial class Test_List : UserControl
    {
        public Test_List()
        {
            InitializeComponent();
            CargarNormas();
        }

        // Clase para representar la norma
        public class Norma
        {
            public int IdNorma { get; set; }
            public string CodigoNorma { get; set; }
            public string Descripcion { get; set; }
        }

        // Método para cargar normas
        private void CargarNormas()
        {
            List<Norma> normas = new List<Norma>();
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Model\\maquina_bd.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT IdNorma, CodigoNorma, Descripcion FROM VistaNormas;";
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        // Crear objeto Norma y agregar a la lista
                        var norma = new Norma
                        {
                            IdNorma = reader.GetInt32(0),
                            CodigoNorma = reader.GetString(1),
                            Descripcion = reader.GetString(2)
                        };

                        normas.Add(norma);

                        // Imprimir en la consola
                        Console.WriteLine($"IdNorma: {norma.IdNorma}, CodigoNorma: {norma.CodigoNorma}, Descripcion: {norma.Descripcion}");
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar normas: {ex.Message}");
                }
            }

            // Asignar la lista de normas al ListBox
            ntcListBox.ItemsSource = normas;
        }

        // Método para obtener la imagen de la norma desde la base de datos
        private byte[] ObtenerImagenNorma(int idNorma)
        {
            byte[] imageBytes = null;
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Model\\maquina_bd.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Imagen FROM ImagenesParametros WHERE IdParametro = @IdNorma";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdNorma", idNorma);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        imageBytes = (byte[])result;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al obtener la imagen: {ex.Message}");
                }
            }

            return imageBytes;
        }

        // Método para manejar la selección del ListBox
        private void ntcListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ntcListBox.SelectedItem is Norma selectedNorma)
            {
                var viewImageTest = new View_Image_Test();

                // Obtener la imagen de la norma seleccionada
                byte[] imageBytes = ObtenerImagenNorma(selectedNorma.IdNorma);

                // Configurar la descripción, imagen, Id y título en el UserControl
                viewImageTest.SetNormText(selectedNorma.Descripcion, imageBytes, selectedNorma.IdNorma, selectedNorma.CodigoNorma);

                // Mostrar el UserControl en el ContentControl
                detailsContentControl.Content = viewImageTest;
            }
        }


        private void search_ntc_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Implementar la lógica de búsqueda si es necesario
        }
    }
}
