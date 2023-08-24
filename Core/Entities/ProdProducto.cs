using Core.Interfaces;

namespace Core.Entities
{
    public class ProdProducto : BaseEntity
    {
        public required string Nombre { get; set; }
        public string Descripcion { get; set; }
        public required decimal ValorCosto { get; set; }
        public required decimal ValorVenta { get; set; }
        public required decimal ValorInicial { get; set; }
        public required decimal ValorCuota { get; set; }
        public required int NumCuotas { get; set; }
        public bool ConfActivo { get; set; }
        public decimal ConfPorUtilidad { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int IdMarca { get; set; }


    }
}
