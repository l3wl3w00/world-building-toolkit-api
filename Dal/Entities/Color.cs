﻿namespace Dal.Entities;

public record Color(byte R, byte G, byte B, byte A) : IModel
{
    public static Color Black => new Color(0,0,0,255);
}