using Microsoft.AspNetCore.Mvc;
using token.DTOs;

namespace token.RepositoryInterfaces;

public interface IPatientRepository
{
    public Task<bool> CheckPatientExist(int patientId);
    public Task<int> InsertNewPatient([FromBody] AddPrescriptionDto addPrescriptionDto);
    public Task<PatientDto> GetPatientInformation(int patientId);
}