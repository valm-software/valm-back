namespace Api.Dtos
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal precio { get; set;}
        public DateTime FechaCreacion { get; set; }
        public MarcaDto Marca { get; set; }
        public CategoriaDto Categoria { get; set; }
    }
}
