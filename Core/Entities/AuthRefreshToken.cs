namespace Core.Entities
{
    public class AuthRefreshToken : BaseEntity
    {
        public int UsuarioId { get; set; }
        public AuthUsuario Usuario { get; set; }
        public string Token { get; set; }
        public DateTime Expirado { get; set; }
        public bool HaExpirado => DateTime.UtcNow > Expirado;
        public DateTime Creado { get; set; }
        public DateTime? Revocado { get; set; }
        public bool EstaActivo => Revocado == null && !HaExpirado;

    }
}
