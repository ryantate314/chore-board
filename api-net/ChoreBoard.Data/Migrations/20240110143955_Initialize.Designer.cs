﻿// <auto-generated />
using System;
using ChoreBoard.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChoreBoard.Data.Migrations
{
    [DbContext(typeof(TaskContext))]
    [Migration("20240110143955_Initialize")]
    partial class Initialize
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.26")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ChoreBoard.Data.Models.TaskDefinition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("Uuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Uuid")
                        .IsUnique();

                    b.ToTable("TaskDefinitions");
                });

            modelBuilder.Entity("ChoreBoard.Data.Models.TaskInstance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("StatusId")
                        .HasColumnType("int");

                    b.Property<int>("TaskDefinitionId")
                        .HasColumnType("int");

                    b.Property<Guid>("Uuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("StatusId");

                    b.HasIndex("TaskDefinitionId");

                    b.HasIndex("Uuid")
                        .IsUnique();

                    b.ToTable("TaskInstances");
                });

            modelBuilder.Entity("ChoreBoard.Data.Models.TaskSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RRule")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TaskDefinitionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TaskDefinitionId");

                    b.ToTable("TaskSchedules");
                });

            modelBuilder.Entity("ChoreBoard.Data.Models.TaskStatus", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TaskStatuses");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Upcoming"
                        },
                        new
                        {
                            Id = 2,
                            Name = "ToDo"
                        },
                        new
                        {
                            Id = 3,
                            Name = "InProgress"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Done"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Excluded"
                        });
                });

            modelBuilder.Entity("ChoreBoard.Data.Models.TaskInstance", b =>
                {
                    b.HasOne("ChoreBoard.Data.Models.TaskStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId");

                    b.HasOne("ChoreBoard.Data.Models.TaskDefinition", "TaskDefinition")
                        .WithMany("TaskInstances")
                        .HasForeignKey("TaskDefinitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Status");

                    b.Navigation("TaskDefinition");
                });

            modelBuilder.Entity("ChoreBoard.Data.Models.TaskSchedule", b =>
                {
                    b.HasOne("ChoreBoard.Data.Models.TaskDefinition", "TaskDefinition")
                        .WithMany("Schedules")
                        .HasForeignKey("TaskDefinitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TaskDefinition");
                });

            modelBuilder.Entity("ChoreBoard.Data.Models.TaskDefinition", b =>
                {
                    b.Navigation("Schedules");

                    b.Navigation("TaskInstances");
                });
#pragma warning restore 612, 618
        }
    }
}
