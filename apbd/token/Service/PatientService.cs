using Microsoft.AspNetCore.Mvc;
using token.DTOs;
using token.RepositoryInterfaces;
using token.ServiceInterfaces;

namespace token.Service;

public class PatientService : IPatientService
{
    public readonly IPatientRepository _PatientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        _PatientRepository = patientRepository;
    }

    public async Task<int> InsertNewPatient([FromBody] AddPrescriptionDto addPrescriptionDto)
    {
        var result = await _PatientRepository.InsertNewPatient(addPrescriptionDto);
        return result;
    }

    public async Task<bool> CheckPatientExist(int patientId)
    {
        var result = await _PatientRepository.CheckPatientExist(patientId);
        return result;
    }

    public async Task<PatientDto> GetPatientInformation(int patientId)
    {
        var result = await _PatientRepository.GetPatientInformation(patientId);
        return result;
    }
}