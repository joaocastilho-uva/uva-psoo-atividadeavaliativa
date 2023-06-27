using System.ComponentModel.DataAnnotations;

namespace ArteConexao.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        [MinLength(6)]
        public string Senha { get; set; }
    }
}
