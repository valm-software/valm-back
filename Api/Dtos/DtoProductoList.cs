namespace Api.Dtos
{
    public class DtoProductoList
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal precio { get; set; }
        public int MarcaId { get; set; }
        public string Marca { get; set; }
        public int CategoriaId { get; set; }
        public string Categoria { get; set; }
    }
}
