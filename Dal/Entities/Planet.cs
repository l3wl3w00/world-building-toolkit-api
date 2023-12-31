﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Entities;

public class Planet
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string CreatorUsername { get; set; } = null!;
    public User Creator { get; set; } = null!;
    public float Radius { get; set; }
    public ICollection<Continent> Continents { get; set; } = new List<Continent>();
}

