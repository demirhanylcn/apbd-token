using Microsoft.EntityFrameworkCore;
using solution.Exception;
using solution.RepositoryInterfaces;

namespace solution.Repository;

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