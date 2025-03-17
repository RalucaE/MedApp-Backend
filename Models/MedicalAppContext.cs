using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MedicalAPI.Models;

public partial class MedicalAppContext : DbContext
{
    public MedicalAppContext()
    {
    }

    public MedicalAppContext(DbContextOptions<MedicalAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Observation> Observations { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-VS7EGU2;Database=MedicalApp3;Integrated Security=true;encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Observation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Observat__3213E83F744BC119");

            entity.ToTable("Observation");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LowerLimit).HasColumnName("lowerLimit");
            entity.Property(e => e.MeasurementDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("measurementDate");
            entity.Property(e => e.ObservationType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("observationType");
            entity.Property(e => e.ReportId).HasColumnName("reportId");
            entity.Property(e => e.Unit)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("unit");
            entity.Property(e => e.UpperLimit).HasColumnName("upperLimit");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.Report).WithMany(p => p.Observations)
                .HasForeignKey(d => d.ReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Observati__repor__2E1BDC42");

            entity.HasOne(d => d.User).WithMany(p => p.Observations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Observati__userI__2D27B809");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Report__3213E83FD5FC457A");

            entity.ToTable("Report");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.FilePath)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("filePath");
            entity.Property(e => e.ReportDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("reportDate");
            entity.Property(e => e.ReportType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("reportType");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UploadDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("uploadDate");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Reports)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Report__userId__2A4B4B5E");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3213E83FE469036F");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3213E83FBC9B9683");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("fullName");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.RoleId)
                .HasDefaultValue(2)
                .HasColumnName("roleId");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__roleId__276EDEB3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
