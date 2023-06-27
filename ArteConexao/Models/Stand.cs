using Microsoft.AspNetCore.Identity;

namespace ArteConexao.Models
{
    public class Stand
    {
        public Stand()
        {
            this.Produtos = new List<Produto>();
            this.Usuarios = new List<IdentityUser>();
        }

        public Guid Id { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public string Localizacao { get; set; }
        public Guid? UsuarioInclusao { get; set; }
        public DateTime? DataInclusao { get; set; }
        public Guid? UsuarioAlteracao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public ICollection<Produto> Produtos { get; set; }
        public ICollection<IdentityUser> Usuarios { get; set; }
        public ICollection<ItemReserva> ItensReserva { get; set; }
    }
}
