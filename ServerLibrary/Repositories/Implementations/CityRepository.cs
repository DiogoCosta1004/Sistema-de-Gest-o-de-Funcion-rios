using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class CityRepository(AppDbContext appDbContext) : IGenericRepositoryInterface<Cidade>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var city = await appDbContext.Cidades.FindAsync(id);
            if (city == null)
                return NotFound();

            appDbContext.Cidades.Remove(city);
            await Commit();
            return Success();
        }

        public async Task<List<Cidade>> GetAll() => await appDbContext.Cidades.ToListAsync();
        public async Task<Cidade> GetById(int id) => await appDbContext.Cidades.FindAsync(id);
        public async Task<GeneralResponse> Insert(Cidade item)
        {
            if (!await CheckName(item.Nome!))
                return new GeneralResponse(false, "Cidade já foi adicionada");

            appDbContext.Cidades.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Cidade item)
        {
            var city = await appDbContext.Cidades.FindAsync(item.Id);
            if (city == null)
                return NotFound();
            city.Nome = item.Nome;
            city.PaisId = item.PaisId;
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
