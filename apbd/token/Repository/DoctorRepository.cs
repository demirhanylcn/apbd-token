using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using solution.Exception;
using solution.RepositoryInterfaces;
using token.DTOs;

namespace solution.Repository;

public class DoctorRepository : IDoctorRepository
{
    public readonly AppDbContext AppDbContext;

    public DoctorRepository(AppDbContext appDbContext)
    {
        AppDbContext = appDbContext;
    }
    
    public async Task<bool> CheckDoctorExist([FromBody] AddPrescriptionDto addPrescriptionDto)
    {
        var doctor =
            await AppDbContext.Doctors.FirstOrDefaultAsync(d => d.IdDoctor == addPrescriptionDto.DoctorId);
        if (doctor == null) throw new DoctorDoesntExistsException(addPrescriptionDto.DoctorId);
        return true;
    }

}