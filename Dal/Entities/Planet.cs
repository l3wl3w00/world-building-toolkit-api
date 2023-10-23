using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Entities;

public class Planet
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string CreatorUsername { get; set; } = "";
    public User Creator { get; set; } = null!;
    public float Radius { get; set; }
    public Color LandColor { get; set; } = Color.Black;
    public Color AntiLandColor { get; set; } = Color.Black;
    public ICollection<Continent> Continents { get; set; } = new List<Continent>();
}

