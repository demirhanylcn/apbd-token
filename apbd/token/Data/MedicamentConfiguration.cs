using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using solution.Models;

namespace solution.Data;

public class MedicamentConfiguration : IEntityTypeConfiguration<Medicament>
{
    public void Configure(EntityTypeBuilder<Medicament> builder)
    {
        builder.HasKey(e => e.IdMedicament);
        builder.Property(e => e.Name).HasMaxLength(100);
        builder.Property(e => e.Description).HasMaxLength(100);
        builder.Property(e => e.Type).HasMaxLength(100);
        builder.HasMany(e => e.PrescriptionMedicaments).WithOne(e => e.Medicament).HasForeignKey(e => e.MedicamentId);
    }
}