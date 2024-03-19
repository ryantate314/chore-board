﻿// <auto-generated />
using System;
using ChoreBoard.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChoreBoard.Data.Migrations
{
    [DbContext(typeof(ChoreBoardContext))]
    [Migration("20240220203844_AddCompletedBy")]
    partial class AddCompletedBy
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.27")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ChoreBoard.Data.Models.Family", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(64)");

                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Uuid" }, "UQ_Family_Uuid")
                        .IsUnique();

                    b.ToTable("Family", "app");
                });

            modelBuilder.Entity("ChoreBoard.Data.Models.FamilyMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime");

                    b.Property<int>("FamilyId")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .IsUnicode(false)
                        .HasColumnType("varchar(32)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .IsUnicode(false)
                        .HasColumnType("varchar(32)");

                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.HasKey("Id");

                    b.HasAlternateKey("Uuid");

                    b.HasIndex("FamilyId");

                    b.HasIndex(new[] { "Uuid" }, "UQ_FamilyMember_Uuid")
                        .IsUnique();

                    b.ToTable("FamilyMember", "app");
                });

            modelBuilder.Entity("ChoreBoard.Data.Models.TaskDefinition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Points")
                        .HasColumnType("int");

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.HasKey("Id");

                    b.HasAlternateKey("Uuid");

                    b.HasIndex(new[] { "Uuid" }, "UQ_TaskDefinition_Uuid")
                        .IsUnique();

                    b.ToTable("TaskDefinition", "app");
                });

            modelBuilder.Entity("ChoreBoard.Data.Models.TaskInstance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("CompletedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CompletedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("InstanceDate")
                        .HasColumnType("datetime");

                    b.Property<int?>("Points")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasMaxLength(1)
                        .IsUnicode(false)
                        .HasColumnType("char(1)")
                        .IsFixedLength();

                    b.Property<int>("TaskDefinitionId")
                        .HasColumnType("int");

                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.HasKey("Id");

                    b.HasAlternateKey("Uuid");

                    b.HasIndex("CompletedById");

                    b.HasIndex("Status");

                    b.HasIndex("TaskDefinitionId");

                    b.HasIndex(new[] { "Uuid" }, "UQ_TaskInstance_Uuid")
                        .IsUnique();

                    b.ToTable("TaskInstance", "app");
                });

            modelBuilder.Entity("ChoreBoard.Data.Models.TaskSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime");

                    b.Property<string>("RRule")
                        .IsRequired()
                        .HasMaxLength(128)
                        .IsUnicode(false)
                        .HasColumnType("varchar(128)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime");

                    b.Property<int>("TaskDefinitionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TaskDefinitionId");

                    b.ToTable("TaskSchedule", "app");
                });

            modelBuilder.Entity("ChoreBoard.Data.Models.TaskStatus", b =>
                {
                    b.Property<string>("StatusCode")
                        .HasMaxLength(1)
                        .IsUnicode(false)
                        .HasColumnType("char(1)")
                        .IsFixedLength();

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(32)
                        .IsUnicode(false)
                        .HasColumnType("varchar(32)");

                    b.HasKey("StatusCode");

                    b.ToTable("TaskStatus", "app");

                    b.HasData(
                        new
                        {
                            StatusCode = "U",
                            Description = "Upcoming"
                        },
                        new
                        {
                            StatusCode = "T",
                            Description = "To Do"
                        },
                        new
                        {
                            StatusCode = "I",
                            Description = "In Progress"
                        },
                        new
                        {
                            StatusCode = "C",
                            Description = "Completed"
                        },
                        new
                        {
                            StatusCode = "D",
                            Description = "Deleted"
                        });
                });

            modelBuilder.Entity("ChoreBoard.Data.Models.User", b =>
                {
                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(128)
                        .IsUnicode(false)
                        .HasColumnType("varchar(128)");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(128)
                        .IsUnicode(false)
                        .HasColumnType("char(128)")
                        .IsFixedLength();

                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.HasIndex(new[] { "Uuid" }, "UQ_User_Uuid")
                        .IsUnique();

                    b.ToTable("User", "auth");
                });

            modelBuilder.Entity("ChoreBoard.Data.Models.FamilyMember", b =>
                {
                    b.HasOne("ChoreBoard.Data.Models.Family", "Family")
                        .WithMany("FamilyMembers")
                        .HasForeignKey("FamilyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_FamilyMember_Family");

                    b.Navigation("Family");
                });

            modelBuilder.Entity("ChoreBoard.Data.Models.TaskInstance", b =>
                {
                    b.HasOne("ChoreBoard.Data.Models.FamilyMember", "CompletedBy")
                        .WithMany("Tasks")
                        .HasForeignKey("CompletedById")
                        .HasConstraintName("FK_TaskInstance_CompletedById_FamilyMember");

                    b.HasOne("ChoreBoard.Data.Models.TaskStatus", "StatusNavigation")
                        .WithMany("TaskInstances")
                        .HasForeignKey("Status")
                        .HasConstraintName("FK_TaskInstance_TaskStatus");

                    b.HasOne("ChoreBoard.Data.Models.TaskDefinition", "TaskDefinition")
                        .WithMany("TaskInstances")
                        .HasForeignKey("TaskDefinitionId")
                        .IsRequired()
                        .HasConstraintName("FK_TaskInstance_TaskDefinitionId_TaskDefinition");

                    b.Navigation("CompletedBy");

                    b.Navigation("StatusNavigation");

                    b.Navigation("TaskDefinition");
                });

            modelBuilder.Entity("ChoreBoard.Data.Models.TaskSchedule", b =>
                {
                    b.HasOne("ChoreBoard.Data.Models.TaskDefinition", "TaskDefinition")
                        .WithMany("TaskSchedules")
                        .HasForeignKey("TaskDefinitionId")
                        .IsRequired()
                        .HasConstraintName("FK_TaskSchedule_TaskDefinitionId_TaskDefinition");

                    b.Navigation("TaskDefinition");
                });

            modelBuilder.Entity("ChoreBoard.Data.Models.Family", b =>
                {
                    b.Navigation("FamilyMembers");
                });

            modelBuilder.Entity("ChoreBoard.Data.Models.FamilyMember", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("ChoreBoard.Data.Models.TaskDefinition", b =>
                {
                    b.Navigation("TaskInstances");

                    b.Navigation("TaskSchedules");
                });

            modelBuilder.Entity("ChoreBoard.Data.Models.TaskStatus", b =>
                {
                    b.Navigation("TaskInstances");
                });
#pragma warning restore 612, 618
        }
    }
}
