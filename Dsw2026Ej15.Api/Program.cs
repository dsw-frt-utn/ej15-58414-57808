using Dsw2026Ej15.Api;
using Dsw2026Ej15.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHealthChecks();

//la de abajo tenes que desconectar lou porque aun no tengo tu clase 
// builder.Services.AddSingleton<IPersistence, PersistenceInMemory>();
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health-check");

app.Run();