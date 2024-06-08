using Microsoft.EntityFrameworkCore;
using solution;
using solution.Repository;
using solution.RepositoryInterfaces;
using solution.Service;
using token.ServiceInterfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IPatientService,PatientService >();
builder.Services.AddScoped<IPrescriptionService,PrescriptionService >();
builder.Services.AddScoped<IPrescriptionMedicamentService,PrescriptionMedicamentService >();
builder.Services.AddScoped<IMedicamentService,MedicamentService >();
builder.Services.AddScoped<IDoctorService,DoctorService >();

builder.Services.AddScoped<IPatientRepository,PatientRepository >();
builder.Services.AddScoped<IDoctorRepository,DoctorRepository >();
builder.Services.AddScoped<IMedicamentRepository,MedicamentRepository >();
builder.Services.AddScoped<IPrescriptionRepository,PrescriptionRepository >();
builder.Services.AddScoped<IPrescriptionMedicamentRepository,PrescriptionMedicamentRepository >();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
