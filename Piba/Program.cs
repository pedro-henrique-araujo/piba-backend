using Microsoft.EntityFrameworkCore;
using Piba.Data;
using Piba.Repositories;
using Piba.Repositories.Interfaces;
using Piba.Services;
using Piba.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddScoped<MemberService, MemberServiceImp>();
builder.Services.AddScoped<SchoolAttendanceService, SchoolAttendanceServiceImp>();
builder.Services.AddScoped<SaturdayWithoutClassService, SaturdayWithoutClassServiceImp>();

builder.Services.AddScoped<MemberRepository, MemberRepositoryImp>();
builder.Services.AddScoped<SchoolAttendanceRepository, SchoolAttendanceRepositoryImp>();
builder.Services.AddScoped<SaturdayWithoutClassRepository, SaturdayWithoutClassRepositoryImp>();

builder.Services.AddScoped<SmtpClientWrapper, SmtpClientWrapperImp>();
builder.Services.AddScoped<EnvironmentVariables, EnvironmentVariablesImp>();

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

app.UseAuthorization();

app.MapControllers();

app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.Run();
