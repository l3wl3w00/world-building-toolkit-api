using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Entities;

public class Planet : IModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string CreatorUsername { get; set; } = "";
    public float Radius { get; set; }
    public uint NumberOfDaysInYear { get; set; }
    public long DayTicks { get; set; }
    public TimeSpan DayLength => new(DayTicks);
    public TimeSpan YearLength => DayLength * NumberOfDaysInYear;
    public Color LandColor { get; set; } = Color.Black;
    public Color AntiLandColor { get; set; } = Color.Black;

    public ICollection<Calendar> Calendars { get; set; } = new HashSet<Calendar>();
    public User Creator { get; set; } = null!;
    public ICollection<Continent> Continents { get; set; } = new List<Continent>();
}