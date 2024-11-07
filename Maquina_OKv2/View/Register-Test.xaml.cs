using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Maquina_OKv2.View
{
    public partial class Register_Test : Window
    {
        private int _selectedNormaId;
        private Dictionary<int, TextBox> _parameterInputs;

        public Register_Test(int normaId, string normaTitle)
        {
            InitializeComponent();
            _selectedNormaId = normaId;
            _parameterInputs = new Dictionary<int, TextBox>();

            // Set the title of the norma
            txtNormaTitle.Text = $"Evaluación para la norma {normaTitle}";

            // Load parameters for the selected norma
            LoadParameters();
        }

        private void LoadParameters()
        {
            // Conexión a la base de datos
            string connectionString = "your_connection_string_here";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT IdParametro, NombreParametro, LimiteInferior, LimiteSuperior, Unidad FROM Parametros WHERE IdNorma = @NormaId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NormaId", _selectedNormaId);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int parametroId = reader.GetInt32(0);
                        string nombreParametro = reader.GetString(1);
                        decimal limiteInferior = reader.GetDecimal(2);
                        decimal limiteSuperior = reader.GetDecimal(3);
                        string unidad = reader.GetString(4);

                        // Crear controles para cada parámetro
                        CreateParameterControl(parametroId, nombreParametro, limiteInferior, limiteSuperior, unidad);
                    }
                }
            }
        }

        private void CreateParameterControl(int parametroId, string nombreParametro, decimal limiteInferior, decimal limiteSuperior, string unidad)
        {
            // Crear contenedor para cada parámetro
            StackPanel parameterStack = new StackPanel { Margin = new Thickness(0, 10, 0, 10) };

            // Etiqueta del parámetro
            TextBlock parameterLabel = new TextBlock
            {
                Text = $"{nombreParametro} ({limiteInferior} - {limiteSuperior} {unidad})",
                FontSize = 14,
                Foreground = System.Windows.Media.Brushes.White
            };
            parameterStack.Children.Add(parameterLabel);

            // Campo de entrada para el valor medido
            TextBox parameterInput = new TextBox
            {
                Width = 200,
                Height = 30,
                Margin = new Thickness(0, 5, 0, 5),
                Background = System.Windows.Media.Brushes.LightGray
            };
            parameterStack.Children.Add(parameterInput);

            // Guardar el TextBox en el diccionario para acceder más tarde
            _parameterInputs[parametroId] = parameterInput;

            // Agregar el stack del parámetro al panel principal
            parametersPanel.Children.Add(parameterStack);
        }

        private void SaveResults_Click(object sender, RoutedEventArgs e)
        {
            // Aquí guardamos los resultados en la base de datos
            string connectionString = "your_connection_string_here";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var parameter in _parameterInputs)
                {
                    int parametroId = parameter.Key;
                    decimal valorMedido = Convert.ToDecimal(parameter.Value.Text);

                    string query = "INSERT INTO ResultadosPrueba (IdPrueba, IdParametro, ValorMedido, EstadoParametro) VALUES (@IdPrueba, @IdParametro, @ValorMedido, @EstadoParametro)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        //command.Parameters.AddWithValue("@IdPrueba", /* IdPrueba generado */);
                        command.Parameters.AddWithValue("@IdParametro", parametroId);
                        command.Parameters.AddWithValue("@ValorMedido", valorMedido);
                        //command.Parameters.AddWithValue("@EstadoParametro", valorMedido >= limiteInferior && valorMedido <= limiteSuperior ? "Cumple" : "No Cumple");

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Resultados guardados correctamente.");
            }
        }
    }
}
