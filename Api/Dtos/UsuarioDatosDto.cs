namespace Api.Dtos
{
    public class UsuarioDatosDto

    {
        public string Mensaje { get; set; }
        public bool EstadoAtenticado { get; set; }
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Dni { get; set; }
        public string Token { get; set; }
        public List<RolConPermisosDto> Roles { get; set; }
    }

    public class RolConPermisosDto
    {
        public string Rol { get; set; }
        public List<ModuloConPermisosDto> Modulos { get; set; }
    }

    public class ModuloConPermisosDto
    {
        public string Modulo { get; set; }
        public List<string> Permisos { get; set; }
    }


}
