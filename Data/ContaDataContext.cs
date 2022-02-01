using ContaCorrente.ApiConta.Models;
using Microsoft.EntityFrameworkCore;

namespace ContaCorrente.ApiConta.Data
{
    public class ContaDataContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ContaDataContext(IConfiguration configuration) => _configuration = configuration;
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(_configuration.GetSection("DataConnectionString").Value);
        }

        public DbSet<Contas> Conta { get; set; }
    }
}
