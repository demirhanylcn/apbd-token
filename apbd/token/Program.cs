using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using solution;
using solution.Service;
using token.Context;
using token.Repository;
using token.RepositoryInterfaces;
using token.Service;
using token.ServiceInterfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<IPrescriptionMedicamentService, PrescriptionMedicamentService>();
builder.Services.AddScoped<IMedicamentService, MedicamentService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IMedicamentRepository, MedicamentRepository>();
builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
builder.Services.AddScoped<IPrescriptionMedicamentRepository, PrescriptionMedicamentRepository>();

builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Auth:ValidIssuer"],
        ValidAudience = builder.Configuration["Auth:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Auth:SecretKey"]!))
    };
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();