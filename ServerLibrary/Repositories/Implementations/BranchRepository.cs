using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class BranchRepository(AppDbContext appDbContext) : IGenericRepositoryInterface<Filial>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var branch = await appDbContext.Filiais.FindAsync(id);
            if (branch == null)
                return NotFound();

            appDbContext.Filiais.Remove(branch);
            await Commit();
            return Success();
        }
        public async Task<List<Filial>> GetAll() => await appDbContext.Filiais.ToListAsync();
        public async Task<Filial> GetById(int id) => await appDbContext.Filiais.FindAsync(id);
        public async Task<GeneralResponse> Insert(Filial item)
        {
            if (!await CheckName(item.Nome!))
                return new GeneralResponse(false, "Departmento já foi adicionado");

            appDbContext.Filiais.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Filial item)
        {
            var branch = await appDbContext.Filiais.FindAsync(item.Id);
            if (branch == null)
                return NotFound();

            branch.Nome = item.Nome;
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
