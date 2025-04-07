using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SzerveroldaliHF03.Entities.Entity;

namespace SzerveroldaliHF03.Data;

public class HFContext:IdentityDbContext
{
    public DbSet<Ticket> Tickets { get; set; }
    
    public HFContext(DbContextOptions<HFContext> options): base(options)
    {
        //  Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>()
            .HasKey(t => new { t.Id });
        
        
        
        base.OnModelCreating(modelBuilder);
    }


}