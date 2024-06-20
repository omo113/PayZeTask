﻿using MassTransit;
using Microsoft.EntityFrameworkCore;
using PayZe.Identity.Domain.Aggregates;
using PayZe.Shared.Abstractions;

namespace PayZe.Identity.Infrastructure.Persistence;

public class IdentityDbContext : DbContext, IMigrationDbContext
{
    public IdentityDbContext()
    {
    }
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost; Database=dotnet-PayZe-CompanyIdentity; Username=myuser; Password=mypassword;Pooling=true;");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.AddInboxStateEntity();
        builder.AddOutboxMessageEntity();
        builder.AddOutboxStateEntity();
    }
    public DbSet<Company> Companies { get; set; }
}

//var(hash, salt) = SecurityService.GenerateHash("VerySecret_Strong_System_Company_Secret");
//var adminCompany = Company.CreateSystemCompany(
//    "Admin Company",
//    "Admin City",
//    "admin@example.com",
//    "admin-api-key",
//    hash,
//    salt);