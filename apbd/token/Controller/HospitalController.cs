using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using token.DTOs;
using token.Exception;
using token.ServiceInterfaces;

namespace token.Controller;

[ApiController]
[Route("api/")]
public class HospitalController : ControllerBase
{
    private readonly IDoctorService _DoctorService;
    private readonly IMedicamentService _MedicamentService;
    private readonly IPatientService _PatientService;
    private readonly IPrescriptionMedicamentService _PrescriptionMedicamentService;

    private readonly IPrescriptionService _PrescriptionService;


    public HospitalController(IPrescriptionService prescriptionService,
        IPatientService patientService,
        IMedicamentService medicamentService,
        IDoctorService doctorService,
        IPrescriptionMedicamentService prescriptionMedicamentService)
    {
        _PrescriptionService = prescriptionService;
        _MedicamentService = medicamentService;
        _DoctorService = doctorService;
        _PrescriptionMedicamentService = prescriptionMedicamentService;
        _PatientService = patientService;
    }


    [Authorize]
    [HttpPost("AddPrescription")]
    public async Task<IActionResult> AddPrescription([FromBody] AddPrescriptionDto addPrescriptionDto)
    {
        try
        {
            var patientExists = await _PatientService.CheckPatientExist(addPrescriptionDto.IdPatient);
            if (!patientExists) await _PatientService.InsertNewPatient(addPrescriptionDto);
            _MedicamentService.CheckMedicamentExists(addPrescriptionDto);
            _MedicamentService.CheckMedicamentLowerThan10(addPrescriptionDto);
            _DoctorService.CheckDoctorExist(addPrescriptionDto);
            _PrescriptionService.CheckDueDate(addPrescriptionDto);
            var prescriptionId = await _PrescriptionService.AddPrescription(addPrescriptionDto);
            var result =
                await _PrescriptionMedicamentService.CompletePrescriptionInsert(addPrescriptionDto, prescriptionId);
            return Ok(result);
        }
        catch (DoctorDoesntExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (MedicamentDoesntExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (MedicamentGreaterThan10Exception ex)
        {
            return BadRequest(ex.Message);
        }
        catch (DueDateSmallerThanDateException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("GetPatientInformation/{patientId:int}")]
    public async Task<IActionResult> GetPatientInformation(int patientId)
    {
        try
        {
            var patientExists = await _PatientService.CheckPatientExist(patientId);
            if (!patientExists) throw new PatientDoesntExistsException(patientId);
            var patient = await _PatientService.GetPatientInformation(patientId);
            return Ok(patient);
        }
        catch (PatientDoesntExistsException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}