using Microsoft.AspNetCore.Mvc;
using token.DTOs;

namespace token.ServiceInterfaces;

public interface IDoctorService
{
    public void CheckDoctorExist([FromBody] AddPrescriptionDto addPrescriptionDto);
}