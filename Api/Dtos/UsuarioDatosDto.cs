using System.Text.Json.Serialization;

namespace Api.Dtos
{
    public class UsuarioDatosDto

    {
        public string Mensaje { get; set; }
        public bool EstadoAutenticado { get; set; }
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Dni { get; set; }
        public string Token { get; set; }
        public List<RolConPoliticasDto> Roles { get; set; }

        //[JsonIgnore]
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpirado { get; set; }

        public UsuarioDatosDto()
        {

        }

    }

    public class RolConPoliticasDto
    {
        public string Rol { get; set; }
        public List<ModuloConPoliticasDto> Modulos { get; set; }


        public RolConPoliticasDto()
        {

        }

    }

    public class ModuloConPoliticasDto
    {
        public string Modulo { get; set; }
        public List<string> Politicas { get; set; }


        public ModuloConPoliticasDto()
        {

        }
    }


}
