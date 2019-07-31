using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PEPDetails.Models.Data
{
    public partial class PEP_Test2Context : DbContext
    {
        public virtual DbSet<ActivityType> ActivityType { get; set; }
        public virtual DbSet<Billing> Billing { get; set; }
        public virtual DbSet<BillingHistory> BillingHistory { get; set; }
        public virtual DbSet<Cmimdata> Cmimdata { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectActivity> ProjectActivity { get; set; }
        public virtual DbSet<ProjectTask> ProjectTask { get; set; }
        public virtual DbSet<ProjectType> ProjectType { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
        public virtual DbSet<Stage> Stage { get; set; }
        public virtual DbSet<Task> Task { get; set; }

        public PEP_Test2Context(DbContextOptions<PEP_Test2Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:DefaultSchema", "db_datareader");

            modelBuilder.Entity<ActivityType>(entity =>
            {
                entity.ToTable("ActivityType", "dbo");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Billing>(entity =>
            {
                entity.HasKey(e => e.ProjectId);

                entity.ToTable("Billing", "dbo");

                entity.Property(e => e.ProjectId).ValueGeneratedNever();

                entity.Property(e => e.LastBillDate).HasColumnType("date");

                entity.Property(e => e.Quote).HasColumnType("money");

                entity.Property(e => e.UnbilledCost).HasColumnType("money");

                entity.Property(e => e.UnbilledTime).HasColumnType("money");

                entity.Property(e => e.UnbilledTotal)
                    .HasColumnType("money")
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(d => d.Project)
                    .WithOne(p => p.Billing)
                    .HasForeignKey<Billing>(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Billing_Project");
            });

            modelBuilder.Entity<BillingHistory>(entity =>
            {
                entity.ToTable("BillingHistory", "dbo");

                entity.Property(e => e.BilledCost).HasColumnType("money");

                entity.Property(e => e.BilledTime).HasColumnType("money");

                entity.Property(e => e.BilledTotal)
                    .HasColumnType("money")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.BillingDate)
                    .HasColumnType("date")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.BillingMonth).HasColumnType("numeric(2, 0)");

                entity.Property(e => e.BillingYear).HasColumnType("numeric(4, 0)");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.BillingHistory)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BillingHistory_Project");
            });

            modelBuilder.Entity<Cmimdata>(entity =>
            {
                entity.ToTable("CMIMData", "dbo");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ClientMatterId)
                    .HasMaxLength(61)
                    .IsUnicode(false)
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.ClientName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ContactAddressCity)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ContactAddressLine1)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ContactAddressLine2)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ContactAddressPostalCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ContactAddressState)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.ContactEmail)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ContactFirstName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ContactLastName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ContactPhone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MatterName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MatterTypeCode)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project", "dbo");

                entity.Property(e => e.Cmimid).HasColumnName("CMIMId");

                entity.Property(e => e.InitialMeetingDate).HasColumnType("date");

                entity.Property(e => e.LastTaskDate).HasColumnType("date");

                entity.Property(e => e.MailingDate).HasColumnType("date");

                entity.Property(e => e.Notes).IsUnicode(false);

                entity.Property(e => e.ProjectStartDate).HasColumnType("date");

                entity.Property(e => e.SigningDate).HasColumnType("date");

                entity.Property(e => e.SigningDeadline).HasColumnType("date");

                entity.Property(e => e.CompletionDate).HasColumnType("date");

                entity.HasOne(d => d.BillingAtty)
                    .WithMany(p => p.ProjectBillingAtty)
                    .HasForeignKey(d => d.BillingAttyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Project_Staff_BillingAtty");

                entity.HasOne(d => d.Cmim)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.Cmimid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Project_CMIMData");

                entity.HasOne(d => d.ProjectType)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.ProjectTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Project_ProjectType");

                entity.HasOne(d => d.ResponsibleAtty)
                    .WithMany(p => p.ProjectResponsibleAtty)
                    .HasForeignKey(d => d.ResponsibleAttyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Project_Staff_ResponsibleAtty");
            });

            modelBuilder.Entity<ProjectActivity>(entity =>
            {
                entity.ToTable("ProjectActivity", "dbo");

                entity.Property(e => e.ActivityDate).HasColumnType("date");

                entity.Property(e => e.Notes).IsUnicode(false);

                entity.HasOne(d => d.ActivityType)
                    .WithMany(p => p.ProjectActivity)
                    .HasForeignKey(d => d.ActivityTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectActivity_ActivityType");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectActivity)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectActivity_Project");
            });

            modelBuilder.Entity<ProjectTask>(entity =>
            {
                entity.ToTable("ProjectTask", "dbo");

                entity.Property(e => e.CompletionDate).HasColumnType("date");

                entity.Property(e => e.Notes).IsUnicode(false);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectTask)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectTask_Project");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.ProjectTask)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectTask_Task");
            });

            modelBuilder.Entity<ProjectType>(entity =>
            {
                entity.ToTable("ProjectType", "dbo");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.ToTable("Staff", "dbo");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasMaxLength(153)
                    .IsUnicode(false)
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleInitial)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Stage>(entity =>
            {
                entity.ToTable("Stage", "dbo");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("Task", "dbo");

                entity.Property(e => e.MatterTypeCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.Stage)
                    .WithMany(p => p.Task)
                    .HasForeignKey(d => d.StageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Task_Stage");
            });
        }
    }
}
