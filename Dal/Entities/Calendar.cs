using System.Collections;
using System.Collections.ObjectModel;

namespace Dal.Entities;

public class Calendar
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    
    public uint FirstYear { get; set; }
    public List<YearPhase> YearPhases { get; set; } = new();

    public Planet? Planet { get; set; }
}

public record CalendarRelativeTimeInstance(GlobalTimeInstance GlobalTime, Calendar Calendar, Planet Planet)
{
    // public long Year
    // {
    //     get
    //     {
    //         GlobalTime.Time.
    //         return Calendar.FirstYear - GlobalTime.Time.Year;
    //     }
    // }
    //
    // public YearPhase Phase
    // {
    //     get
    //     {
    //         
    //         foreach (var phase in Calendar.YearPhases)
    //         {
    //             phase.NumberOfDays;
    //         }
    //     }
    // }
}
public record YearPhase(string Name, uint NumberOfDays);

