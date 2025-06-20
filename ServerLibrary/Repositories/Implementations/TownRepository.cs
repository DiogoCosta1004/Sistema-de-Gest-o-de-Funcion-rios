using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class TownRepository(AppDbContext appDbContext) : IGenericRepositoryInterface<Town>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var town = await appDbContext.Towns.FindAsync(id);
            if (town == null)
                return NotFound();

            appDbContext.Towns.Remove(town);
            await Commit();
            return Success();
        }
        public async Task<List<Town>> GetAll() => await appDbContext.Towns.ToListAsync();
        public async Task<Town> GetById(int id) => await appDbContext.Towns.FindAsync(id);
        public async Task<GeneralResponse> Insert(Town item)
        {
            if (!await CheckName(item.Nome!))
                return new GeneralResponse(false, "Cidade já foi adicionada");

            appDbContext.Towns.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Town item)
        {
            var town = await appDbContext.Towns.FindAsync(item.Id);
            if (town == null)
                return NotFound();

            town.Nome = item.Nome;
            await Commit();
            return Success();
        }
        private async Task Commit() => await appDbContext.SaveChangesAsync();
        private GeneralResponse NotFound() => new GeneralResponse(false, "Cidade não encontrada");
        private GeneralResponse Success() => new GeneralResponse(true, "Operação realizada com sucesso");
        private async Task<bool> CheckName(string name)
        {
            var item = await appDbContext.Cidades.FirstOrDefaultAsync(x => x.Nome!.ToLower().Equals(name.ToLower()));
            return item is null;
        }
    }
}
