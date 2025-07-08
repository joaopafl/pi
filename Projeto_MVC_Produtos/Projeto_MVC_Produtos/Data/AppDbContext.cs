using Microsoft.EntityFrameworkCore;
using Projeto_MVC_Produtos.Models;

namespace Projeto_MVC_Produtos.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Produto> Produtos { get; set; }

        public DbSet<Fornecedor> Fornecedores { get; set;}

        public DbSet<Usuarios> Usuarios { get; set; }
    }
}
