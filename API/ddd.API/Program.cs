using BuildingBlocks.Application;
using BuildingBlocks.Application.Email;
using BuildingBlocks.Domain;
using MasterData.Infrastructure;
using MasterData.Infrastructure.Configuration;
using Hellang.Middleware.ProblemDetails;
using ddd.API.Extensions;
using ddd.API.Validation;
using Procurement.Infrastructure;
using Procurement.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddProblemDetails(x =>
{
    x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
    x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
});


var emailConfig = builder.Configuration
    .GetSection("EmailConfiguration")
    .Get<EmailConfiguration>() ?? new EmailConfiguration();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=:memory:";

builder.Services.AddMasterDataServices(connectionString, emailConfig);
builder.Services.AddProcurementProcurementServices(connectionString, emailConfig);


var app = builder.Build();

MasterDataCompositionRoot.SetServiceProvider(app.Services);
ProcurementCompositionRoot.SetServiceProvider(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseProblemDetails();

app.MapControllers();
app.Run();