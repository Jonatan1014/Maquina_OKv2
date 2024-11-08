public class TestHistory
{
    public int Id { get; set; }
    public string ImagePath { get; set; } // Ruta de la imagen (si tienes una en la base de datos, será un byte array convertido)
    public string Title { get; set; }
    public string Description { get; set; }
}
