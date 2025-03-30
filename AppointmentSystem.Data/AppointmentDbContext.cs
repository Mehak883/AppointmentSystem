using AppointmentSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Data
{
    public class AppointmentDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options) : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<DoctorSpecialization> DoctorSpecializations { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Many-to-Many Relationship: Doctor ↔ Specialization
            modelBuilder.Entity<DoctorSpecialization>()
                .HasKey(ds => new { ds.DoctorId, ds.SpecializationId });

            modelBuilder.Entity<DoctorSpecialization>()
                .HasOne(ds => ds.Doctor)
                .WithMany(d => d.DoctorSpecializations)
                .HasForeignKey(ds => ds.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DoctorSpecialization>()
                .HasOne(ds => ds.Specialization)
                .WithMany(s => s.DoctorSpecializations)
                .HasForeignKey(ds => ds.SpecializationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ensure SpecializationName is unique
            modelBuilder.Entity<Specialization>()
                .HasIndex(s => s.SpecializationName)
                .IsUnique();
        
        // ✅ One-to-Many Relationship: Patient ↔ Appointments
        //modelBuilder.Entity<Appointment>()
        //    .HasOne(a => a.Patient)
        //    .WithMany(p => p.Appointments)  // Ensure Patient has `ICollection<Appointment>`
        //    .HasForeignKey(a => a.PatientId)
        //    .OnDelete(DeleteBehavior.Restrict);

        // ✅ One-to-Many Relationship: Slot ↔ Appointments
        modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Slot)
                .WithMany(s => s.Appointments)  // Ensure Slot has `ICollection<Appointment>`
                .HasForeignKey(a => a.SlotId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ One-to-Many Relationship: Doctor ↔ Slots
            //modelBuilder.Entity<Slot>()
            //    .HasOne(s => s.Doctor)
            //    .WithMany(d => d.Slots)  // Ensure Doctor has `ICollection<Slot>`
            //    .HasForeignKey(s => s.DoctorId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
