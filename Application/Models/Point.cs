﻿namespace Application.Models;

public readonly record struct Point(double X, double Y)
{
    public double DistanceTo(Point other) => Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
}
