using Microsoft.AspNetCore.Mvc;
using token.DTOs;
using token.RepositoryInterfaces;
using token.ServiceInterfaces;

namespace token.Service;

public class DoctorService : IDoctorService
{
    public readonly IDoctorRepository _DoctorRepository;

    public DoctorService(IDoctorRepository doctorRepository)
    {
        _DoctorRepository = doctorRepository;
    }

    public void CheckDoctorExist([FromBody] AddPrescriptionDto addPrescriptionDto)
    {
        _DoctorRepository.CheckDoctorExist(addPrescriptionDto);
    }
}