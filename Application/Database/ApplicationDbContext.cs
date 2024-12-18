﻿using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Application.Entities;
using Application.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Application.Identity;

namespace Application.Database;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public DbSet<TravelingSalesmanInputData> InputData { get; set; }
    public DbSet<TravelingSalesmanResult> Results { get; set; }
    public DbSet<ProblemSolvingProgress> Progresses { get; set; }

    public ApplicationDbContext() : base() { }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        var inputDataBuilder = modelBuilder.Entity<TravelingSalesmanInputData>();
        var resultBuilder = modelBuilder.Entity<TravelingSalesmanResult>();
        var progressBuilder = modelBuilder.Entity<ProblemSolvingProgress>();
        var serializerOptions = new JsonSerializerOptions();

        inputDataBuilder.HasKey(x => x.Id);
        inputDataBuilder.Property(x => x.Points).HasConversion(
                p => JsonSerializer.Serialize(p, serializerOptions),
                p => JsonSerializer.Deserialize<List<Point>>(p, serializerOptions) ?? new List<Point>()
        );
        inputDataBuilder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");

        resultBuilder.HasKey(x => x.Id);
        resultBuilder.Property(x => x.Path).HasConversion(
                p => JsonSerializer.Serialize(p, serializerOptions),
                p => JsonSerializer.Deserialize<List<Point>>(p, serializerOptions) ?? new List<Point>()
        );
        resultBuilder.Property(x => x.ComputedAt).HasDefaultValueSql("GETDATE()");

        progressBuilder.HasKey(x => x.Id);
        progressBuilder.Property(x => x.Progress).HasDefaultValue(0);
    }
}
