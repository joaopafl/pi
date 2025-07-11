using Microsoft.EntityFrameworkCore;
using Pi_Odonto.Models;
using System.Collections.Generic;

namespace Pi_Odonto.Data
    {
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {

        // DbSet para representar a tabela Responsaveis no banco
        public DbSet<Responsavel> responsavel { get; set; }
        // Adicione aqui outros DbSets se tiver outras entidades no Pi_Odonto
        // Exemplo:
        // public DbSet<OutraEntidade> OutrasEntidades { get; set; }
    }
}
