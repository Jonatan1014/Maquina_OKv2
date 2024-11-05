using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Maquina_OKv2.View.ControUserView
{
    public partial class History_test : UserControl
    {
        public ICommand NavigateCommand { get; private set; }

        public History_test()
        {
            InitializeComponent();

            // Inicializar el comando de navegación
            NavigateCommand = new RelayCommand<int>(NavigateToDetails);

            // Cargar datos de ejemplo
            LoadData();
        }

        private void LoadData()
        {
            // Ejemplo de datos. Aquí puedes cargar desde la base de datos
            List<CardItem> items = new List<CardItem>
            {
                new CardItem { Id = 1, ImagePath = "/Images/sample1.png", Title = "Item 1", Description = "Descripción del Item 1" },
                new CardItem { Id = 2, ImagePath = "/Images/sample2.png", Title = "Item 2", Description = "Descripción del Item 2" },
                new CardItem { Id = 3, ImagePath = "/Images/sample3.png", Title = "Item 3", Description = "Descripción del Item 3" }
            };

            // Asignar los datos al ItemsControl
            cardItemsControl.ItemsSource = items;
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
