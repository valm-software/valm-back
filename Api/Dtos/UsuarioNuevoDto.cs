using System.ComponentModel.DataAnnotations;

namespace Api.Dtos
{
    public class UsuarioNuevoDto
    {
        [Required]
        public string DNI { get; set; }

        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Correo { get; set; }

        [Required]
        public string Usuario { get; set; }
        [Required]
        public string Password { get; set; }


    }
}
