using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ChoreBoard.Data.Models
{
    public partial class ChoreBoardContext : DbContext
    {
        public ChoreBoardContext()
        {
        }

        public ChoreBoardContext(DbContextOptions<ChoreBoardContext> options)
            : base(options)
        {
        }

        public virtual DbSet<FamilyMember> FamilyMembers { get; set; } = null!;
        public virtual DbSet<TaskDefinition> TaskDefinitions { get; set; } = null!;
        public virtual DbSet<TaskInstance> TaskInstances { get; set; } = null!;
        public virtual DbSet<TaskSchedule> TaskSchedules { get; set; } = null!;
        public virtual DbSet<TaskStatus> TaskStatuses { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Family> Families { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:ChoreBoard");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FamilyMember>(entity =>
            {
                entity.ToTable("FamilyMember", "app");

                entity.HasIndex(e => e.Uuid, "UQ_FamilyMember_Uuid")
                    .IsUnique();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Uuid).HasDefaultValueSql("(newid())");

                entity.HasOne(e => e.Family)
                    .WithMany(p => p.FamilyMembers)
                    .HasForeignKey(e => e.FamilyId)
                    .HasConstraintName("FK_FamilyMember_Family");
            });

            modelBuilder.Entity<TaskDefinition>(entity =>
            {
                entity.ToTable("TaskDefinition", "app");

                entity.HasIndex(e => e.Uuid, "UQ_TaskDefinition_Uuid")
                    .IsUnique();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.ShortDescription).HasMaxLength(128);

                entity.Property(e => e.Uuid).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<TaskInstance>(entity =>
            {
                entity.ToTable("TaskInstance", "app");

                entity.HasIndex(e => e.Uuid, "UQ_TaskInstance_Uuid")
                    .IsUnique();

                entity.Property(e => e.InstanceDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Uuid).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.TaskInstances)
                    .HasForeignKey(d => d.Status)
                    .HasConstraintName("FK_TaskInstance_TaskStatus");

                entity.HasOne(d => d.TaskDefinition)
                    .WithMany(p => p.TaskInstances)
                    .HasForeignKey(d => d.TaskDefinitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TaskInstance_TaskDefinitionId_TaskDefinition");
            });

            modelBuilder.Entity<TaskSchedule>(entity =>
            {
                entity.ToTable("TaskSchedule", "app");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.RRule)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.TaskDefinition)
                    .WithMany(p => p.TaskSchedules)
                    .HasForeignKey(d => d.TaskDefinitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TaskSchedule_TaskDefinitionId_TaskDefinition");
            });

            modelBuilder.Entity<TaskStatus>(entity =>
            {
                entity.HasKey(e => e.StatusCode);

                entity.ToTable("TaskStatus", "app");

                entity.Property(e => e.StatusCode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Description)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.HasData(
                    new TaskStatus() { StatusCode = Service.Models.TaskStatus.STATUS_UPCOMING, Description = "Upcoming" },
                    new TaskStatus() { StatusCode = Service.Models.TaskStatus.STATUS_TODO, Description = "To Do" },
                    new TaskStatus() { StatusCode = Service.Models.TaskStatus.STATUS_IN_PROGRESS, Description = "In Progress" },
                    new TaskStatus() { StatusCode = Service.Models.TaskStatus.STATUS_COMPLETED, Description = "Completed" },
                    new TaskStatus() { StatusCode = Service.Models.TaskStatus.STATUS_DELETED, Description = "Deleted" }
                );
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("User", "auth");

                entity.HasIndex(e => e.Uuid, "UQ_User_Uuid")
                    .IsUnique();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Uuid).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<Family>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("Family", "app");

                entity.Property(e => e.Uuid)
                   .HasDefaultValueSql("(newid())");

                entity.HasIndex(e => e.Uuid, "UQ_Family_Uuid")
                    .IsUnique();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(64)
                    .IsUnicode(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
