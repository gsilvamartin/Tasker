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

    public virtual DbSet<JobHistory> JobHistories { get; set; }

    public virtual DbSet<JobHistoryStatus> JobHistoryStatuses { get; set; }

    public virtual DbSet<SchedulingType> SchedulingTypes { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=SCHEDULER_SQL_CONNECTION_STRING");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Job__3214EC07F1838238");

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
                .HasConstraintName("FK__Job__SchedulingT__42E1EEFE");

            entity.HasOne(d => d.Status).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Job__StatusId__41EDCAC5");
        });

        modelBuilder.Entity<JobHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobHisto__3214EC077BF5C2BF");

            entity.ToTable("JobHistory");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ExceptionMessage)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.HistoryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Message)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.StackTrace)
                .HasMaxLength(1)
                .IsUnicode(false);

            entity.HasOne(d => d.Job).WithMany(p => p.JobHistories)
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JobHistor__JobId__4A8310C6");

            entity.HasOne(d => d.JobStatusNavigation).WithMany(p => p.JobHistories)
                .HasForeignKey(d => d.JobStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JobHistor__JobSt__498EEC8D");
        });

        modelBuilder.Entity<JobHistoryStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobHisto__3214EC0797D18B08");

            entity.ToTable("JobHistoryStatus");

            entity.Property(e => e.Name)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SchedulingType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Scheduli__3214EC07E8D3B665");

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
            entity.HasKey(e => e.Id).HasName("PK__Status__3214EC07CF3BDAF6");

            entity.ToTable("Status");

            entity.Property(e => e.Name)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}