using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace Api.Dtos
{
    public class ProductoAddUpdateDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage= "El nombre del producto es Obligatorio" )]
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int MarcaId { get; set; }
        public int categoriaId { get; set; }

    }
}
