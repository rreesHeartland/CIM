using HeartlandCIM.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace HeartlandCIM.Web.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<CalibrationInstrument> CalibrationInstruments => Set<CalibrationInstrument>();
    public DbSet<AreaStatus> AreaStatuses => Set<AreaStatus>();
    public DbSet<InstrumentLog> InstrumentLogs => Set<InstrumentLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CalibrationInstrument>(e =>
        {
            e.ToTable("CalibrationInstruments");
            e.HasKey(x => x.Id);
            e.Property(x => x.Next_Cal_Date).HasColumnType("date");
            e.Ignore(x => x.Status); // computed in code, never persisted
            e.HasIndex(x => x.Area);
            e.HasIndex(x => x.Title);
        });

        modelBuilder.Entity<AreaStatus>(e =>
        {
            e.ToTable("AreaStatus");
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Title);
        });

        modelBuilder.Entity<InstrumentLog>(e =>
        {
            e.ToTable("InstrumentLog");
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.ItemID);
        });
    }
}
