using Core.Interfaces;

namespace Core.Entities;
public class Marca : BaseEntity
{
    public string Nombre { get; set; }
    public ICollection<Producto> Productos { get; set; }

}
