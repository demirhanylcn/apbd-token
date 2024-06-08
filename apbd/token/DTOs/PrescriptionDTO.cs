namespace token.DTOs;

public class PrescriptionDto
{
    public int PrescriptionId { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<MedicamentDto> MedicamentDtos { get; set; }
    public DoctorDto DoctorDtos { get; set; }

}