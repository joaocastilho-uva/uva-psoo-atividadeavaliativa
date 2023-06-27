using System.ComponentModel.DataAnnotations;

namespace ArteConexao.ViewModels
{
    public class UsuarioViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Senha { get; set; }

        public bool Admin { get; set; }
    }
}
