using Microsoft.EntityFrameworkCore;
using token.Context;
using token.DTOs;
using token.Models;
using token.RepositoryInterfaces;

namespace token.Repository;

public class PrescriptionMedicamentRepository : IPrescriptionMedicamentRepository
{
    public readonly AppDbContext AppDbConext;

    public PrescriptionMedicamentRepository(AppDbContext appDbContext)
    {
        AppDbConext = appDbContext;
    }


    public async Task<int> CompletePrescriptionInsert(MedicamentDto medicamentDto, int prescriptionId)
    {
        var medicament =
            await AppDbConext.Medicaments.FirstOrDefaultAsync(m =>
                m.IdMedicament == medicamentDto.IdMedicament);
        var prescription =
            await AppDbConext.Prescriptions
                .FirstOrDefaultAsync(p => p.Id == prescriptionId);
        await AppDbConext.PrescriptionMedicaments
            .AddAsync(new PrescriptionMedicament
            {
                Details = medicamentDto.Description,
                Dose = medicamentDto.Dose,
                MedicamentId = medicamentDto.IdMedicament,
                PrescriptionId = prescriptionId
            });

        var result = await AppDbConext.SaveChangesAsync();
        return result;
    }
}