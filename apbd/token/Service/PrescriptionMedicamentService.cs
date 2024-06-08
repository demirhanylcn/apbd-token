using solution.Repository;
using solution.RepositoryInterfaces;
using token.DTOs;
using token.ServiceInterfaces;

namespace solution.Service;

public class PrescriptionMedicamentService : IPrescriptionMedicamentService
{
    public readonly IPrescriptionMedicamentRepository _PrescriptionMedicamentRepository;

    public PrescriptionMedicamentService(IPrescriptionMedicamentRepository prescriptionMedicamentRepository)
    {
        _PrescriptionMedicamentRepository = prescriptionMedicamentRepository;
    }

    public async Task<int> CompletePrescriptionInsert(AddPrescriptionDto addPrescriptionDto, int prescriptionId)
    {
        var result = 0;
        foreach (var each in addPrescriptionDto.Medicaments)
        {
            result = await _PrescriptionMedicamentRepository.CompletePrescriptionInsert(each, prescriptionId);
        }

        return result;
    }
}