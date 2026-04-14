using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LMS.Models.LMSModels
{
    public partial class LMSContext : DbContext
    {
        public LMSContext()
        {
        }

        public LMSContext(DbContextOptions<LMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; } = null!;
        public virtual DbSet<Assignment> Assignments { get; set; } = null!;
        public virtual DbSet<AssignmentCategory> AssignmentCategories { get; set; } = null!;
        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Enrolled> Enrolleds { get; set; } = null!;
        public virtual DbSet<Professor> Professors { get; set; } = null!;
        public virtual DbSet<Sshkey> Sshkeys { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<Submission> Submissions { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("name=LMS:LMSConnectionString", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.11.16-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("latin1_swedish_ci")
                .HasCharSet("latin1");

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.UId)
                    .HasName("PRIMARY");

                entity.Property(e => e.UId)
                    .HasMaxLength(8)
                    .HasColumnName("uID")
                    .IsFixedLength();

                entity.Property(e => e.Dob).HasColumnName("DOB");

                entity.Property(e => e.FName)
                    .HasMaxLength(100)
                    .HasColumnName("fName");

                entity.Property(e => e.LName)
                    .HasMaxLength(100)
                    .HasColumnName("lName");
            });

            modelBuilder.Entity<Assignment>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.AcId }, "Name")
                    .IsUnique();

                entity.HasIndex(e => e.AcId, "acID");

                entity.Property(e => e.AssignmentId)
                    .HasColumnType("smallint(5) unsigned")
                    .HasColumnName("assignmentID");

                entity.Property(e => e.AcId)
                    .HasColumnType("smallint(5) unsigned")
                    .HasColumnName("acID");

                entity.Property(e => e.Contents).HasMaxLength(8192);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Points).HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.Ac)
                    .WithMany(p => p.Assignments)
                    .HasForeignKey(d => d.AcId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Assignments_ibfk_1");
            });

            modelBuilder.Entity<AssignmentCategory>(entity =>
            {
                entity.HasKey(e => e.AcId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => new { e.ClassId, e.Name }, "classID")
                    .IsUnique();

                entity.Property(e => e.AcId)
                    .HasColumnType("smallint(5) unsigned")
                    .HasColumnName("acID");

                entity.Property(e => e.ClassId)
                    .HasColumnType("smallint(5) unsigned")
                    .HasColumnName("classID");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Weight).HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.AssignmentCategories)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AssignmentCategories_ibfk_1");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasIndex(e => new { e.Year, e.Season, e.CourseId }, "Year")
                    .IsUnique();

                entity.HasIndex(e => e.CourseId, "courseID");

                entity.HasIndex(e => e.PuId, "puID");

                entity.Property(e => e.ClassId)
                    .HasColumnType("smallint(5) unsigned")
                    .HasColumnName("classID");

                entity.Property(e => e.CourseId)
                    .HasColumnType("smallint(5) unsigned")
                    .HasColumnName("courseID");

                entity.Property(e => e.End).HasColumnType("time");

                entity.Property(e => e.Loc).HasMaxLength(100);

                entity.Property(e => e.PuId)
                    .HasMaxLength(8)
                    .HasColumnName("puID")
                    .IsFixedLength();

                entity.Property(e => e.Season).HasMaxLength(6);

                entity.Property(e => e.Start).HasColumnType("time");

                entity.Property(e => e.Year).HasColumnType("smallint(5) unsigned");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Classes_ibfk_1");

                entity.HasOne(d => d.Pu)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.PuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Classes_ibfk_2");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasIndex(e => new { e.Subject, e.Num }, "Subject")
                    .IsUnique();

                entity.Property(e => e.CourseId)
                    .HasColumnType("smallint(5) unsigned")
                    .HasColumnName("courseID");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Num).HasColumnType("smallint(5) unsigned");

                entity.Property(e => e.Subject).HasMaxLength(4);

                entity.HasOne(d => d.SubjectNavigation)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.Subject)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Courses_ibfk_1");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Subject)
                    .HasName("PRIMARY");

                entity.Property(e => e.Subject).HasMaxLength(4);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Enrolled>(entity =>
            {
                entity.HasKey(e => new { e.ClassId, e.StudentId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("Enrolled");

                entity.HasIndex(e => e.StudentId, "studentID");

                entity.Property(e => e.ClassId)
                    .HasColumnType("smallint(5) unsigned")
                    .HasColumnName("classID");

                entity.Property(e => e.StudentId)
                    .HasMaxLength(8)
                    .HasColumnName("studentID")
                    .IsFixedLength();

                entity.Property(e => e.Grade).HasMaxLength(2);

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Enrolleds)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Enrolled_ibfk_1");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Enrolleds)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Enrolled_ibfk_2");
            });

            modelBuilder.Entity<Professor>(entity =>
            {
                entity.HasKey(e => e.UId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Subject, "Subject");

                entity.Property(e => e.UId)
                    .HasMaxLength(8)
                    .HasColumnName("uID")
                    .IsFixedLength();

                entity.Property(e => e.Dob).HasColumnName("DOB");

                entity.Property(e => e.FName)
                    .HasMaxLength(100)
                    .HasColumnName("fName");

                entity.Property(e => e.LName)
                    .HasMaxLength(100)
                    .HasColumnName("lName");

                entity.Property(e => e.Subject).HasMaxLength(4);

                entity.HasOne(d => d.SubjectNavigation)
                    .WithMany(p => p.Professors)
                    .HasForeignKey(d => d.Subject)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Professors_ibfk_1");
            });

            modelBuilder.Entity<Sshkey>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("sshkey");

                entity.Property(e => e.Sshkey1)
                    .HasColumnType("text")
                    .HasColumnName("sshkey");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.UId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Subject, "Subject");

                entity.Property(e => e.UId)
                    .HasMaxLength(8)
                    .HasColumnName("uID")
                    .IsFixedLength();

                entity.Property(e => e.Dob).HasColumnName("DOB");

                entity.Property(e => e.FName)
                    .HasMaxLength(100)
                    .HasColumnName("fName");

                entity.Property(e => e.LName)
                    .HasMaxLength(100)
                    .HasColumnName("lName");

                entity.Property(e => e.Subject).HasMaxLength(4);

                entity.HasOne(d => d.SubjectNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.Subject)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Students_ibfk_1");
            });

            modelBuilder.Entity<Submission>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.AssignmentId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.HasIndex(e => e.AssignmentId, "assignmentID");

                entity.Property(e => e.StudentId)
                    .HasMaxLength(8)
                    .HasColumnName("studentID")
                    .IsFixedLength();

                entity.Property(e => e.AssignmentId)
                    .HasColumnType("smallint(5) unsigned")
                    .HasColumnName("assignmentID");

                entity.Property(e => e.Contents).HasMaxLength(8192);

                entity.Property(e => e.Score).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Time).HasColumnType("time");

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.Submissions)
                    .HasForeignKey(d => d.AssignmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Submissions_ibfk_2");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Submissions)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Submissions_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
