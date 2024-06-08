using Microsoft.AspNetCore.Mvc;
using token.DTOs;

namespace token.ServiceInterfaces;

public interface IPatientService
{
    public Task<int> InsertNewPatient([FromBody] AddPrescriptionDto addPrescriptionDto);

    public Task<bool> CheckPatientExist(int patientId);
    public Task<PatientDto> GetPatientInformation(int patientId);
}