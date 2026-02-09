using BuildingBlocks.Application;
using BuildingBlocks.Domain;
using MasterData.Infrastructure;
using MasterData.Infrastructure.Configuration;
using BuildingBlocks.Infrastructure.Email;
using Hellang.Middleware.ProblemDetails;
using ddd.API.Extensions;
using ddd.API.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddProblemDetails(x =>
{
    x.IncludeExceptionDetails = (ctx, ex) => false;
    x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
    x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
});


var emailConfig      = new EmailConfiguration("noreply@example.com");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=:memory:";

builder.Services.AddMasterDataServices(connectionString, emailConfig);

var app = builder.Build();

MasterDataCompositionRoot.SetServiceProvider(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseProblemDetails();

app.MapControllers();
app.Run();