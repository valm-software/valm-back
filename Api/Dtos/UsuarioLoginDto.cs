using System.ComponentModel.DataAnnotations;

namespace Api.Dtos
{
    public class UsuarioLoginDto
    {
        [Required]
        public string Usuario { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
