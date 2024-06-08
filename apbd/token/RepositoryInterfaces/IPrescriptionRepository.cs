using Microsoft.AspNetCore.Mvc;
using token.DTOs;

namespace solution.RepositoryInterfaces;

public interface IPrescriptionRepository
{
    public Task<int> AddPrescription([FromBody] AddPrescriptionDto addPrescriptionDto);
}