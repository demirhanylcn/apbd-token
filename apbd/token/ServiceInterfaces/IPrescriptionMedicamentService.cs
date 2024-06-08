using token.DTOs;

namespace token.ServiceInterfaces;

public interface IPrescriptionMedicamentService
{
    public Task<int> CompletePrescriptionInsert(AddPrescriptionDto addPrescriptionDto, int prescriptionId);

}