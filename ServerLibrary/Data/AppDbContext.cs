using BaseLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace ServerLibrary.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Funcionario> Funcionarios { get; set; }

        //Todos os departamentos
        public DbSet<DepartamentoGeral> DepartamentosGerais { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Filial> Filiais { get; set; }

        public DbSet<Town> Towns { get; set; }
        //public DbSet<Cidade> Cidades { get; set; }

        //Parte de autenticação
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<SystemRole> SystemRoles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RefreshTokenInfo> RefreshTokenInfos { get; set; }

        //Outros, férias
        public DbSet<Vacation> Vacations { get; set; }
        public DbSet<VacationType> VacationTypes { get; set; }

        public DbSet<Overtime> Overtimes { get; set; }
        public DbSet<OvertimeType> OvertimeTypes { get; set; }

        public DbSet<Sanction> Sanctions { get; set; }
        public DbSet<SanctionType> SanctionTypes { get; set; }  
        public DbSet<Doctor> Doctors { get; set; }



    }
}

