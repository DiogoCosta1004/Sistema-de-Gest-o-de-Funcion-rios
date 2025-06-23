using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class DepartmentRepository(AppDbContext appDbContext) : IGenericRepositoryInterface<Departamento>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var department = await appDbContext.Departamentos.FindAsync(id);
            if (department == null)
                return NotFound();

            appDbContext.Departamentos.Remove(department);
            await Commit();
            return Success();
        }

        public async Task<List<Departamento>> GetAll() => await appDbContext
                                                            .Departamentos.AsNoTracking()
                                                            .Include(x => x.DepartamentoGeral)
                                                            .ToListAsync();
        public async Task<Departamento> GetById(int id) => await appDbContext.Departamentos.FindAsync(id);
        public async Task<GeneralResponse> Insert(Departamento item)
        {
            if (!await CheckName(item.Nome!))
                return new GeneralResponse(false, "Departmento já foi adicionado");

            appDbContext.Departamentos.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Departamento item)
        {
            var department = await appDbContext.Departamentos.FindAsync(item.Id);
            if (department == null)
                return NotFound();

            department.Nome = item.Nome;
            await Commit();
            return Success();
        }
        private async Task Commit() => await appDbContext.SaveChangesAsync();
        private GeneralResponse NotFound() => new GeneralResponse(false, "Departamento não encontrado");
        private GeneralResponse Success() => new GeneralResponse(true, "Operação realizada com sucesso");
        private async Task<bool> CheckName(string name)
        {
            var item = await appDbContext.Departamentos.FirstOrDefaultAsync(x => x.Nome!.ToLower().Equals(name.ToLower()));
            return item is null;
        }
    }
}
