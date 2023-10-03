using Core.Interfaces;

namespace Core.Entities
{
    public class AuthPolitica : BaseEntity
    {
        public string NombrePolitica { get; set; }
        public string Modulo { get; set; }
    }
}
