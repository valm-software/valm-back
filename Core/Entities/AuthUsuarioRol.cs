namespace Core.Entities
{
    public class AuthUsuarioRol
    {
        public int UsuarioId { get; set; }
        public AuthUsuario AuthUsuario { get; set; }

        public int RolId { get; set; }
        public AuthRol AuthRol { get; set; }
    }
}
