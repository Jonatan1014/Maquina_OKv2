using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Maquina_OKv2.View
{
    public partial class Register_Test : Window
    {
        private int _selectedNormaId;
        private Dictionary<int, ParametroInfo> _parameterInfos;

        public Register_Test(int normaId, string normaTitle)
        {
            InitializeComponent();
            _selectedNormaId = normaId;
            _parameterInfos = new Dictionary<int, ParametroInfo>();
            txtNormaTitle.Text = $"Evaluación para la norma {normaTitle}";
            LoadParameters();
        }

        private void LoadParameters()
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Model\\maquina_bd.mdf;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
            SELECT p.IdParametro, p.NombreParametro, p.LimiteInferior, p.LimiteSuperior, p.Unidad, i.Imagen
            FROM Parametros p
            LEFT JOIN ImagenesParametros i ON p.IdParametro = i.IdParametro
            WHERE p.IdNorma = @NormaId";

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
                        byte[] imageBytes = reader["Imagen"] as byte[];

                        // Crear controles para cada parámetro, pasando la imagen y demás datos
                        CreateParameterControl(parametroId, nombreParametro, limiteInferior, limiteSuperior, unidad, imageBytes);
                    }
                }
            }
        }

        private void CreateParameterControl(int parametroId, string nombreParametro, decimal limiteInferior, decimal limiteSuperior, string unidad, byte[] imageBytes)
        {
            Grid parameterGrid = new Grid { Margin = new Thickness(0, 10, 0, 10) };
            parameterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            parameterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });

            // Mostrar la imagen
            Image parameterImage = new Image { Width = 100, Height = 100, Margin = new Thickness(5) };
            if (imageBytes != null)
            {
                using (var ms = new System.IO.MemoryStream(imageBytes))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = ms;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                    parameterImage.Source = image;
                }
            }
            parameterGrid.Children.Add(parameterImage);
            Grid.SetColumn(parameterImage, 0);

            // Contenedor para la etiqueta y campo de entrada
            StackPanel inputStack = new StackPanel();
            TextBlock parameterLabel = new TextBlock
            {
                Text = $"{nombreParametro} ({limiteInferior} - {limiteSuperior} {unidad})",
                FontSize = 14,
                Foreground = System.Windows.Media.Brushes.White
            };
            inputStack.Children.Add(parameterLabel);

            TextBox parameterInput = new TextBox
            {
                Width = 200,
                Height = 30,
                Margin = new Thickness(0, 5, 0, 5),
                Background = System.Windows.Media.Brushes.LightGray
            };
            parameterInput.PreviewTextInput += (s, e) => e.Handled = !IsTextAllowed(e.Text);
            inputStack.Children.Add(parameterInput);
            parameterGrid.Children.Add(inputStack);
            Grid.SetColumn(inputStack, 1);

            _parameterInfos[parametroId] = new ParametroInfo
            {
                IdParametro = parametroId,
                LimiteInferior = limiteInferior,
                LimiteSuperior = limiteSuperior,
                InputControl = parameterInput
            };

            parametersPanel.Children.Add(parameterGrid);
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); // Solo permite números y puntos
            return !regex.IsMatch(text);
        }

        private void SaveResults_Click(object sender, RoutedEventArgs e)
        {
            foreach (var parameter in _parameterInfos.Values)
            {
                if (string.IsNullOrWhiteSpace(parameter.InputControl.Text))
                {
                    MessageBox.Show("Por favor, completa todos los campos.");
                    return;
                }
            }
            // Código para guardar los datos en la base de datos
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private class ParametroInfo
        {
            public int IdParametro { get; set; }
            public decimal LimiteInferior { get; set; }
            public decimal LimiteSuperior { get; set; }
            public TextBox InputControl { get; set; }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
