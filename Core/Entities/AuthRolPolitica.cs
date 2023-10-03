namespace Core.Entities
{
    public class AuthRolPolitica
    {
        public int RolId { get; set; }
        public AuthRol AuthRol { get; set; }

        public int PoliticaId { get; set; }
        public AuthPolitica AuthPolitica { get; set; }
    }
}
