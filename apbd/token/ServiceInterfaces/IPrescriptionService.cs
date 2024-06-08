using Microsoft.AspNetCore.Mvc;
using token.DTOs;

namespace token.ServiceInterfaces;

public interface IPrescriptionService
{
    public Task<int> AddPrescription([FromBody] AddPrescriptionDto addPrescriptionDto);
    public void CheckDueDate([FromBody] AddPrescriptionDto addPrescriptionDto);


}