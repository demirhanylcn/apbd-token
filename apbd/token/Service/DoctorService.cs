using Microsoft.AspNetCore.Mvc;
using solution.Repository;
using solution.RepositoryInterfaces;
using token.DTOs;
using token.ServiceInterfaces;

namespace solution.Service;

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