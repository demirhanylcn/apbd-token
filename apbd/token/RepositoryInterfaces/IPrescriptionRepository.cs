using Microsoft.AspNetCore.Mvc;
using token.DTOs;

namespace token.RepositoryInterfaces;

public interface IPrescriptionRepository
{
    public Task<int> AddPrescription([FromBody] AddPrescriptionDto addPrescriptionDto);
}