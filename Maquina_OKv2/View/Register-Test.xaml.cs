using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

                        CreateParameterControl(parametroId, nombreParametro, limiteInferior, limiteSuperior, unidad, imageBytes);
                    }
                }
            }
        }

        private void CreateParameterControl(int parametroId, string nombreParametro, decimal limiteInferior, decimal limiteSuperior, string unidad, byte[] imageBytes)
        {
            // Crear un Grid para el diseño
            Grid parameterGrid = new Grid
            {
                Margin = new Thickness(0, 10, 0, 10)
            };
            parameterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            parameterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });

            // Mostrar la imagen si existe
            Image parameterImage = new Image
            {
                Margin = new Thickness(10),
                Stretch = Stretch.Uniform, // Asegura que la imagen no se recorte y mantenga proporciones
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
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

            // Crear un StackPanel para centrar el texto y el TextBox horizontalmente
            StackPanel inputStack = new StackPanel
            {
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Center // Centra el contenido del StackPanel
            };

            // Etiqueta del parámetro
            TextBlock parameterLabel = new TextBlock
            {
                //({limiteInferior} - {limiteSuperior} {unidad})
                Text = $"{nombreParametro} ({unidad})", 
                FontSize = 14,
                Foreground = Brushes.White,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center, // Centrar horizontalmente
                Margin = new Thickness(0, 0, 0, 5) // Espacio inferior
            };
            inputStack.Children.Add(parameterLabel);

            // Campo de entrada
            TextBox parameterInput = new TextBox
            {
                Width = 300, // Ancho del TextBox
                Height = 40, // Altura del TextBox
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3E3E52")), // Fondo
                Foreground = Brushes.White, // Texto
                BorderBrush = Brushes.White, // Borde
                BorderThickness = new Thickness(0, 0, 0, 1), // Solo borde inferior
                FontFamily = new FontFamily("Montserrat Medium"), // Fuente personalizada
                FontSize = 14, // Tamaño de la fuente
                VerticalContentAlignment = VerticalAlignment.Center, // Centrar texto verticalmente
                HorizontalContentAlignment = HorizontalAlignment.Left, // Alinear texto horizontalmente
                Padding = new Thickness(5, 0, 5, 0), // Espaciado interno
                IsEnabled = true // Habilitado para entrada
            };

            parameterInput.PreviewTextInput += ParameterInput_PreviewTextInput;
            inputStack.Children.Add(parameterInput);

            parameterGrid.Children.Add(inputStack);
            Grid.SetColumn(inputStack, 1);

            // Guardar la información del parámetro
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
            // Solo permite números y puntos
            Regex regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            return regex.IsMatch(text);
        }

        private void ParameterInput_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void ParameterInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                if (!decimal.TryParse(textBox.Text, out _)) // Valida que sea un número decimal válido
                {
                    textBox.Text = string.Empty; // Borra el contenido si no es válido
                }
            }
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

            // Lógica para guardar los datos en la base de datos
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Model\\maquina_bd.mdf;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var parameter in _parameterInfos.Values)
                {
                    decimal valorMedido = Convert.ToDecimal(parameter.InputControl.Text);

                    string query = "INSERT INTO ResultadosPrueba (IdPrueba, IdParametro, ValorMedido, EstadoParametro) VALUES (@IdPrueba, @IdParametro, @ValorMedido, @EstadoParametro)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdPrueba", /* IdPrueba */ 1);
                        command.Parameters.AddWithValue("@IdParametro", parameter.IdParametro);
                        command.Parameters.AddWithValue("@ValorMedido", valorMedido);
                        command.Parameters.AddWithValue("@EstadoParametro", (valorMedido >= parameter.LimiteInferior && valorMedido <= parameter.LimiteSuperior) ? "Cumple" : "No Cumple");

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Resultados guardados correctamente.");
            }
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
    }
}
