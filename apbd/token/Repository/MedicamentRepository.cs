using Microsoft.EntityFrameworkCore;
using token.Context;
using token.Exception;
using token.RepositoryInterfaces;

namespace token.Repository;

public class MedicamentRepository : IMedicamentRepository
{
    public readonly AppDbContext AppDbContext;

    public MedicamentRepository(AppDbContext appDbContext)
    {
        AppDbContext = appDbContext;
    }

    public async Task<bool> CheckMedicamentExists(int medicamentId)
    {
        var medicament = await AppDbContext.Medicaments.FirstOrDefaultAsync(m => m.IdMedicament == medicamentId);
        if (medicament == null) throw new MedicamentDoesntExistsException(medicamentId);
        return true;
    }
}