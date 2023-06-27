using ArteConexao.Models;
using Microsoft.EntityFrameworkCore;

namespace ArteConexao.Data
{
    public class ArteConexaoDbContext : DbContext
    {
        public ArteConexaoDbContext(DbContextOptions<ArteConexaoDbContext> options) : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Carrinho> Carrinhos { get; set; }
        public DbSet<ItemCarrinho> ItensCarrinho { get; set; }
        public DbSet<Stand> Stands { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<ItemReserva> ItensReserva { get; set; }
    }
}
