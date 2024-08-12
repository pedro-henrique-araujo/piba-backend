using Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using Piba.Data;
using Piba.Repositories;
using Piba.Repositories.Interfaces;
using Piba.Services;
using Piba.Services.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PibaDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Piba");
    options.UseSqlServer(connectionString);
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
      .AddEntityFrameworkStores<PibaDbContext>()
      .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


builder.Services.AddScoped<EmailService, EmailServiceImp>();
builder.Services.AddScoped<EnvironmentVariables, WebApiEnvironmentVariables>();
builder.Services.AddScoped<ExcelService, ExcelServiceImp>();
builder.Services.AddScoped<LogService, LogServiceImp>();
builder.Services.AddScoped<MemberService, MemberServiceImp>();
builder.Services.AddScoped<SaturdayWithoutClassService, SaturdayWithoutClassServiceImp>();
builder.Services.AddScoped<SchoolAttendanceService, SchoolAttendanceServiceImp>();
builder.Services.AddScoped<SmtpClientWrapper, SmtpClientWrapperImp>();
builder.Services.AddScoped<StatusHistoryService, StatusHistoryServiceImp>();

builder.Services.AddScoped<LogRepository, LogRepositoryImp>();
builder.Services.AddScoped<MemberRepository, MemberRepositoryImp>();
builder.Services.AddScoped<SaturdayWithoutClassRepository, SaturdayWithoutClassRepositoryImp>();
builder.Services.AddScoped<SchoolAttendanceRepository, SchoolAttendanceRepositoryImp>();
builder.Services.AddScoped<StatusHistoryItemRepository, StatusHistoryItemRepositoryImp>();
builder.Services.AddScoped<StatusHistoryRepository, StatusHistoryRepositoryImp>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<PibaDbContext>();
    dbContext.Database.Migrate();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.Run();
