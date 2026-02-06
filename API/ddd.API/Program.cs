using MasterData.Infrastructure;
using MasterData.Infrastructure.Configuration;
using BuildingBlocks.Infrastructure.Email;
using ddd.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerDocumentation();

var emailConfig      = new EmailConfiguration("noreply@example.com");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=:memory:";
builder.Services.AddMasterDataServices(connectionString, emailConfig);

var app = builder.Build();

MasterDataCompositionRoot.SetServiceProvider(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.MapControllers();
app.Run();