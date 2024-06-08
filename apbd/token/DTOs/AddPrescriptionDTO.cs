namespace token.DTOs;

public class AddPrescriptionDto
{
    
    
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; } 
    public List<MedicamentDto> Medicaments { get; set; } = [];
    public DateTime PrescriptionDate { get; set; }
    public DateTime PrescriptionDueDate { get; set; }
    
    public int DoctorId { get; set; }
    
}