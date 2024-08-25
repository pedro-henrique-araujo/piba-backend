using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Piba;
using Piba.Data;
using Piba.Repositories;
using Piba.Services;

var builder = WebApplication.CreateBuilder(args);

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PibaDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Piba");
    options.UseSqlServer(connectionString);
});


var authConfiguring = new AuthConfiguring(builder);

authConfiguring.Configure();

ServiceInjections.Inject(builder.Services);
RepositoryInjections.Inject(builder.Services);


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<PibaDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
