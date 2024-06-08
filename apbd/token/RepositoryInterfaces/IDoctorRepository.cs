using Microsoft.AspNetCore.Mvc;
using token.DTOs;

namespace token.RepositoryInterfaces;

public interface IDoctorRepository
{
    public Task<bool> CheckDoctorExist([FromBody] AddPrescriptionDto addPrescriptionDto);
}