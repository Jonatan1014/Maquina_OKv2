using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Maquina_OKv2.View.ControUserView
{
    public partial class History_test : UserControl
    {
        public ObservableCollection<TestHistory> TestHistories { get; set; }
        public ICommand NavigateCommand { get; private set; }

        public History_test()
        {
            InitializeComponent();
            TestHistories = new ObservableCollection<TestHistory>();
            DataContext = this;

            // Inicializar el comando de navegación
            NavigateCommand = new RelayCommand<int>(NavigateToDetails);

            // Cargar el historial de pruebas desde la base de datos
            LoadTestHistoryFromDatabase();
        }

        private void LoadTestHistoryFromDatabase()
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Model\\maquina_bd.mdf;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT IdPrueba, Estado, FechaPrueba, ImagenPrueba FROM Pruebas";
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string state = reader.GetString(1);
                        string date = reader.GetDateTime(2).ToShortDateString();
                        byte[] imageBytes = reader["ImagenPrueba"] as byte[];

                        // Convertir la imagen (byte[]) en una ruta de imagen o un BitmapImage para mostrar
                        string imagePath = ConvertToImageSource(imageBytes);

                        TestHistories.Add(new TestHistory
                        {
                            Id = id,
                            Title = $"Prueba ID: {id}",
                            Description = $"Estado: {state} - Fecha: {date}",
                            ImagePath = imagePath
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar el historial de pruebas: {ex.Message}");
                }
            }

            // Asignar los datos al ItemsControl
            cardItemsControl.ItemsSource = TestHistories;
        }

        // Convertir byte array a ruta de imagen
        private string ConvertToImageSource(byte[] imageBytes)
        {
            if (imageBytes == null)
                return null;

            // Convertir a ruta de imagen temporal
            var imagePath = System.IO.Path.GetTempFileName() + ".png";
            System.IO.File.WriteAllBytes(imagePath, imageBytes);
            return imagePath;
        }

        private void NavigateToDetails(int id)
        {
            // Navegar a la página Test_ready y pasar el ID
            var testReadyPage = new Test_ready();
            testReadyPage.LoadData(id); // Método para cargar datos basado en el ID
            NavigationService.GetNavigationService(this)?.Navigate(testReadyPage);
        }
    }
}
