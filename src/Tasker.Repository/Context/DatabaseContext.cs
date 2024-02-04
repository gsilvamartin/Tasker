using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Tasker.Repository.Entity;

namespace Tasker.Repository.Context;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<SchedulingType> SchedulingTypes { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=SCHEDULER_SQL_CONNECTION_STRING");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Job__3214EC0707378F7D");

            entity.ToTable("Job");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CronExpression)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.ExecutedBy)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.LastExecution).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(1)
                .IsUnicode(false);

            entity.HasOne(d => d.SchedulingType).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.SchedulingTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Job__SchedulingT__245D67DE");

            entity.HasOne(d => d.Status).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Job__StatusId__236943A5");
        });

        modelBuilder.Entity<SchedulingType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Scheduli__3214EC07F2FDC90E");

            entity.ToTable("SchedulingType");

            entity.Property(e => e.Description)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Status__3214EC07B6AA8766");

            entity.ToTable("Status");

            entity.Property(e => e.Name)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}