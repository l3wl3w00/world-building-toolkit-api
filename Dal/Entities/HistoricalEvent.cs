namespace Dal.Entities;

public class HistoricalEvent : IModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ulong StartSeconds { get; set; }
    public ulong EndSeconds { get; set; }

    public GlobalTimeInstance Start => new(StartSeconds);
    public GlobalTimeInstance End => new(EndSeconds);

    public RelativeTimeInstance RelativeStart { get; set; }
    public RelativeTimeInstance RelativeEnd { get; set; }
    public Guid RegionId { get; set; }
    public Guid DefaultCalendarId { get; set; }
    public Calendar DefaultCalendar { get; set; } = null!;
    public Region Region { get; set; } = null!;
}

public readonly struct GlobalTimeInstance(ulong seconds)
{
    private const decimal SecondsInOneMinute = 60;
    private const decimal MinutesInOneHour = 60;
    private const decimal SecondsInOneHour = MinutesInOneHour * SecondsInOneMinute;

    public readonly ulong Seconds = seconds;
    
    public decimal Hours => seconds / SecondsInOneHour;
    public uint WholeHours => (uint) Math.Floor(Hours);
    public decimal HoursInDay(Planet planet) => Hours % (decimal)planet.DayLength.TotalHours;
    public uint WholeHoursInDay(Planet planet) => (uint)HoursInDay(planet);
    public decimal Minutes => Hours * MinutesInOneHour;
    public decimal WholeMinutes => WholeHours * MinutesInOneHour;
    public decimal MinutesInHour => Minutes % MinutesInOneHour;
    public decimal WholeMinutesInHour => WholeMinutes - WholeHours * MinutesInOneHour;
    public decimal Days(Planet planet) => Hours / (decimal) planet.DayLength.TotalHours;
    public uint WholeDays(Planet planet) => (uint) Math.Floor(Days(planet));
    public decimal DaysInYear(Planet planet) => Days(planet) % planet.NumberOfDaysInYear;
    public uint WholeDaysInYear(Planet planet) => (uint) Math.Floor(DaysInYear(planet));
    public decimal Years(Planet planet) => Days(planet) / planet.NumberOfDaysInYear;
    public uint WholeYears(Planet planet) => (uint) Math.Floor(Years(planet));
    
    public static GlobalTimeInstance FromTimeSpan(TimeSpan timeSpan)
    {
        return new GlobalTimeInstance((ulong)timeSpan.TotalSeconds);
    }
}

public readonly struct RelativeTimeInstance(uint year, string yearPhase, uint day, uint hour, uint minute)
{
    public uint Year { get; } = year;
    public string YearPhase { get; } = yearPhase;
    public uint Day { get; } = day;
    public uint Hour { get; } = hour;
    public uint Minute { get; } = minute;
}