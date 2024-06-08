using token.DTOs;

namespace token.ServiceInterfaces;

public interface IMedicamentService
{
    public void CheckMedicamentExists(AddPrescriptionDto addPrescriptionDto);

    public void CheckMedicamentLowerThan10(AddPrescriptionDto addPrescriptionDto);
}