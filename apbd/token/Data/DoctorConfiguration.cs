
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using solution.Models;

namespace solution.Data;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> d)
    {
        d.HasKey(e => e.IdDoctor);
        d.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
        d.Property(e => e.LastName).HasMaxLength(100).IsRequired();
        d.Property(e => e.Email).HasMaxLength(100).IsRequired();

        d.HasMany(e => e.Prescriptions).WithOne(e => e.Doctor).HasForeignKey(e => e.DoctorId);
    }
}