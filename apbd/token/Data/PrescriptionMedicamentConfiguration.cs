using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using solution.Models;

namespace solution.Data;

public class PrescriptionMedicamentConfiguration : IEntityTypeConfiguration<PrescriptionMedicament>
{
    public void Configure(EntityTypeBuilder<PrescriptionMedicament> builder)
    {
        builder.HasKey(e => new { e.PrescriptionId, e.MedicamentId});
        builder.Property(e => e.Details).HasMaxLength(100);
        
        builder.HasOne(e => e.Prescription)
            .WithMany(p => p.Prescription_Medicaments)
            .HasForeignKey(e => e.PrescriptionId);

        builder.HasOne(e => e.Medicament)
            .WithMany(m => m.PrescriptionMedicaments)
            .HasForeignKey(e => e.MedicamentId);
        

    }
}