namespace token.DTOs;

public class MedicamentDto
{
    public int IdMedicament { get; set; }
    public int Dose { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public List<PrescriptionMedicamentDto> PrescriptionMedicaments { get; set; }
}