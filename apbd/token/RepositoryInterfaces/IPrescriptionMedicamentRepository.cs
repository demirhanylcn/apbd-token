using token.DTOs;

namespace solution.RepositoryInterfaces;

public interface IPrescriptionMedicamentRepository
{
    public Task<int> CompletePrescriptionInsert(MedicamentDto medicamentDto, int prescriptionId);

}