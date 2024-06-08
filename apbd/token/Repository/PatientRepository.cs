using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using solution.Models;
using solution.RepositoryInterfaces;
using token.DTOs;

namespace solution.Repository;

public class PatientRepository : IPatientRepository
{
    public readonly AppDbContext AppDbContext;

    public PatientRepository(AppDbContext appDbContext)
    {
        AppDbContext = appDbContext;
    }
    public async Task<bool> CheckPatientExist(int patientId)
    {
        var patient =
            await AppDbContext.Patients.FirstOrDefaultAsync(p => p.IdPatient == patientId);
        if (patient == null) return false;
        return true;
    }

    public async Task<int> InsertNewPatient([FromBody] AddPrescriptionDto addPrescriptionDto)
    {
        var patient =
            new Patient
            {
                BirthDate = addPrescriptionDto.BirthDate,
                FirstName = addPrescriptionDto.FirstName,
                LastName = addPrescriptionDto.LastName,
                Prescriptions = new List<Prescription>()
            };
        await AppDbContext.Patients.AddAsync(patient);
        var result = await AppDbContext.SaveChangesAsync();
        return result;
    }

    public async Task<PatientDto> GetPatientInformation(int patientId)
    {
        var patient = await AppDbContext.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Prescription_Medicaments)
            .ThenInclude(pm => pm.Medicament).ThenInclude(medicament => medicament.PrescriptionMedicaments)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Doctor)
            .FirstOrDefaultAsync(p => p.IdPatient == patientId);

        var result = new PatientDto
        {
            IdPatient = patientId,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            Prescriptions = patient.Prescriptions.Select(pr => new PrescriptionDto
            {
                PrescriptionId = pr.Id,
                Date = pr.Date,
                DueDate = pr.DueDate,
                MedicamentDtos = pr.Prescription_Medicaments.Select(pm => pm.Medicament)
                    .Select(m => new MedicamentDto
                    {
                        Description = m.PrescriptionMedicaments.First(ms => ms.MedicamentId == m.IdMedicament).Details,
                        Dose = m.PrescriptionMedicaments.First(ms => ms.MedicamentId == m.IdMedicament).Dose
                    }).ToList(),
                DoctorDtos = new DoctorDto
                {
                    FirstName = pr.Doctor.FirstName,
                    LastName = pr.Doctor.LastName,
                    IdDoctor = pr.Doctor.IdDoctor
                }
            }).ToList()
        };

        return result;
    }

    
}