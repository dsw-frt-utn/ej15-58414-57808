using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Data.Dtos;
using System.Text.Json;

namespace Dsw2026Ej15.Data;

public static class DataExtensions
{
    public static void SeedworkSpecialities(this Dsw2026Ej15DbContext context, string dataSource)
    {
        if (context.Set<Speciality>().Any()) return;
        var json = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Sources", dataSource));
        var entities = JsonSerializer.Deserialize<List<SpecialityDto>>(json,
            new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            }) ?? [];
        var specialities = entities.Select(s => new Speciality(s.Name, s.Description, s.Id));
        context.Set<Speciality>().AddRange(specialities);
        context.SaveChanges();
    }
}