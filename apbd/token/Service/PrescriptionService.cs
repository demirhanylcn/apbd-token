using Microsoft.AspNetCore.Mvc;
using solution.Exception;
using solution.RepositoryInterfaces;
using token.DTOs;
using token.ServiceInterfaces;

namespace solution.Service;

public class PrescriptionService : IPrescriptionService
{
    public readonly IPrescriptionRepository _PrescriptionRepository;

    public PrescriptionService(IPrescriptionRepository prescriptionRepository)
    {
        _PrescriptionRepository = prescriptionRepository;
    }
    public async Task<int> AddPrescription([FromBody] AddPrescriptionDto addPrescriptionDto)
    {
        var result = await _PrescriptionRepository.AddPrescription(addPrescriptionDto);
        return result;
    }

    public void CheckDueDate([FromBody] AddPrescriptionDto addPrescriptionDto)
    {
        var dueDate = addPrescriptionDto.PrescriptionDueDate;
        var date = addPrescriptionDto.PrescriptionDate;
        var result = dueDate >= date;
        if (result!) throw new DueDateSmallerThanDateException(dueDate,date);
    } 
}