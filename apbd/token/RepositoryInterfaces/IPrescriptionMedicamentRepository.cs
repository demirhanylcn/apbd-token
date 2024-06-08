using token.DTOs;

namespace token.RepositoryInterfaces;

public interface IPrescriptionMedicamentRepository
{
    public Task<int> CompletePrescriptionInsert(MedicamentDto medicamentDto, int prescriptionId);
}