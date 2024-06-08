using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using solution.Models;

namespace solution.Data;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasKey(e => e.IdPatient);
        builder.Property(e => e.LastName).HasMaxLength(100);
        builder.Property(e => e.FirstName).HasMaxLength(100);
        builder.HasMany(e => e.Prescriptions).WithOne(e => e.Patient).HasForeignKey(e => e.PatientId);
    }
}