using Microsoft.AspNetCore.Mvc;
using token.DTOs;

namespace solution.RepositoryInterfaces;

public interface IDoctorRepository
{
    public Task<bool> CheckDoctorExist([FromBody] AddPrescriptionDto addPrescriptionDto);

}