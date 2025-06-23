using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class GeneralDepartmentRepository(AppDbContext appDbContext) : IGenericRepositoryInterface<DepartamentoGeral>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var departament = await appDbContext.DepartamentosGerais.FindAsync(id);
            if (departament is null)
                return NotFound();

            appDbContext.DepartamentosGerais.Remove(departament);
            await Commit();
            return Success();
        }

        public async Task<List<DepartamentoGeral>> GetAll() => await appDbContext.DepartamentosGerais.ToListAsync();

        public async Task<DepartamentoGeral> GetById(int id) => await appDbContext.DepartamentosGerais.FindAsync(id);

        public async Task<GeneralResponse> Insert(DepartamentoGeral item)
        {
            var existingItem = await appDbContext.DepartamentosGerais.FirstOrDefaultAsync(x => x.Nome!.ToLower().Equals(item.Nome.ToLower()));
            if (existingItem != null) 
            {
                return new GeneralResponse(false, "Departamento geral já cadastrado");
            }

            appDbContext.DepartamentosGerais.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(DepartamentoGeral item)
        {
            var departament = await appDbContext.DepartamentosGerais.FindAsync(item.Id);
            if (departament == null)
                return NotFound();

            departament.Nome = item.Nome;
            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new(false, "Departamento geral não encontrado");
        private static GeneralResponse Success() => new(true, "Processo completado");
        private async Task Commit() => await appDbContext.SaveChangesAsync();
        //private async Task<bool> CheckName(string name)
        //{
        //    var item = await appDbContext.DepartamentosGerais.FirstOrDefaultAsync(x => x.Nome!.ToLower().Equals(name.ToLower()));
        //    return item is null;
        //}
    }
}
