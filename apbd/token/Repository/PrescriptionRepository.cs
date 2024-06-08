using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using token.Context;
using token.DTOs;
using token.Models;
using token.RepositoryInterfaces;

namespace token.Repository;

public class PrescriptionRepository : IPrescriptionRepository
{
    public readonly AppDbContext AppDbContext;

    public PrescriptionRepository(AppDbContext appDbContext)
    {
        AppDbContext = appDbContext;
    }

    public async Task<int> AddPrescription([FromBody] AddPrescriptionDto addPrescriptionDto)
    {
        var doctor =
            await AppDbContext.Doctors.FirstOrDefaultAsync(d => d.IdDoctor == addPrescriptionDto.DoctorId);
        var patient =
            await AppDbContext.Patients.FirstOrDefaultAsync(p => p.IdPatient == addPrescriptionDto.IdPatient);
        var prescription =
            new Prescription
            {
                Date = addPrescriptionDto.PrescriptionDate,
                DoctorId = addPrescriptionDto.DoctorId,
                Doctor = doctor,
                DueDate = addPrescriptionDto.PrescriptionDueDate,
                Patient = patient,
                PatientId = addPrescriptionDto.IdPatient,
                Prescription_Medicaments = new List<PrescriptionMedicament>()
            };
        await AppDbContext.Prescriptions.AddAsync(prescription);
        await AppDbContext.SaveChangesAsync();
        return prescription.Id;
    }
}