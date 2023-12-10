using System.Collections;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Dal.Entities;

public class Calendar : IModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    
    public ulong FirstYear { get; set; }
    public List<YearPhase> YearPhases { get; set; } = new();

    public Planet Planet { get; set; } = null!;
    public Guid PlanetId { get; set; }
    public ICollection<HistoricalEvent> Events { get; set; } = new HashSet<HistoricalEvent>();

    public bool AreYearPhasesValidForPlanet(Planet planet) => 
        YearPhases.Sum(p => p.NumberOfDays) == planet.NumberOfDaysInYear;
}

public record YearPhase(string Name, uint NumberOfDays)
{
    public uint DaysBefore(IEnumerable<YearPhase> yearPhases) => 
        (uint) yearPhases.TakeWhile(yp => yp.Name != Name).Sum(yp => yp.NumberOfDays);
}
