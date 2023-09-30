namespace Api.Dtos
{
    public class DtoProducto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal precio { get; set;}
        public DateTime FechaCreacion { get; set; }
        public DtoMarca Marca { get; set; }
        public DtoCategoria Categoria { get; set; }
    }
}
