using BaseLibrary.Entities;
using Microsoft.EntityFrameworkCore;

namespace ServerLibrary.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<DepartamentoGeral> DepartamentosGerais { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Filial> Filiais { get; set; }
        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<SystemRole> SystemRoles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RefreshTokenInfo> RefreshTokenInfos { get; set; }
    }
}

