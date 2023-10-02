using System.ComponentModel.DataAnnotations;

namespace Api.Dtos
{
    public class NuevoUsuarioDto
    {
        [Required]
        public string Dni { get; set; }

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
