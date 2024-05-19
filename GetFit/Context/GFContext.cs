using GetFit.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GetFit.Context;

public class GFContext(DbContextOptions<GFContext> options) : IdentityDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<Member> MemberDetails { get; set; }
    public DbSet<MembershipType> MembershipTypes { get; set; }
    public DbSet<Trainer> Trainers { get; set; }
    public DbSet<FitnessClass> FitnessClasses { get; set; }
    /*public DbSet<MemberClass> MemberClasses { get; set; }
    public DbSet<Payment> Payments { get; set; }*/
}
