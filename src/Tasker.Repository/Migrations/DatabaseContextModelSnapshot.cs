﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tasker.Repository;

#nullable disable

namespace Tasker.Repository.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Tasker.Repository.Entity.GlobalConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CurrentlyRetries")
                        .HasColumnType("int");

                    b.Property<string>("ExecutedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("InProgress")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastExecution")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<int>("MaximumRetries")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StatusId");

                    b.ToTable("GlobalConfigurations");
                });

            modelBuilder.Entity("Tasker.Repository.Entity.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Status");
                });

            modelBuilder.Entity("Tasker.Repository.Entity.GlobalConfiguration", b =>
                {
                    b.HasOne("Tasker.Repository.Entity.Status", "Status")
                        .WithMany("GlobalConfiguration")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Status");
                });

            modelBuilder.Entity("Tasker.Repository.Entity.Status", b =>
                {
                    b.Navigation("GlobalConfiguration");
                });
#pragma warning restore 612, 618
        }
    }
}