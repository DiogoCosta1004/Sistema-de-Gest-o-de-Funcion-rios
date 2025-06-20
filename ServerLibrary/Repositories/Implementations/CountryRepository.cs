using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class CountryRepository(AppDbContext appDbContext) : IGenericRepositoryInterface<Pais>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var country = await appDbContext.Paises.FindAsync(id);
            if (country == null)
                return NotFound();

            appDbContext.Paises.Remove(country);
            await Commit();
            return Success();
        }

        public async Task<List<Pais>> GetAll() => await appDbContext.Paises.ToListAsync();
        public async Task<Pais> GetById(int id) => await appDbContext.Paises.FindAsync(id);

        public async Task<GeneralResponse> Insert(Pais item)
        {
            if (!await CheckName(item.Nome!))
                return new GeneralResponse(false, "País já foi adicionado");

            appDbContext.Paises.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Pais item)
        {
            var country = await appDbContext.Paises.FindAsync(item.Id);
            if (country == null)
                return NotFound();

            country.Nome = item.Nome;
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
