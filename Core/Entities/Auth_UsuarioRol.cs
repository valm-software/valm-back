namespace Core.Entities
{
    public class Auth_UsuarioRol
    {
        public int UsuarioId { get; set; }
        public Auth_Usuario Auth_Usuario { get; set; }

        public int RolId { get; set; }
        public Auth_Rol Auth_Rol { get; set; }
    }
}
