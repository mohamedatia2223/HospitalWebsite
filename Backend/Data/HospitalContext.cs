namespace Hospital.Data
{
	public class HospitalContext : DbContext
	{
		public HospitalContext(DbContextOptions<HospitalContext> options) : base(options)
		{
		}
		public DbSet<Patient> Patients { get; set; }
		public DbSet<Doctor> Doctors { get; set; }
		
		public DbSet<MedicalRecord> MedicalRecords { get; set; }
		public DbSet<Appointment> Appointments { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Doctor>()
				.HasMany(d => d.Patients)
				.WithOne(p => p.Doctor)
				.HasForeignKey(p => p.DoctorId)
				.OnDelete(DeleteBehavior.SetNull);
	

			modelBuilder.Entity<Patient>()
				.HasMany(a => a.MedicalRecords)
				.WithOne(b => b.Patient)
				.HasForeignKey(c => c.PatientId)
				.OnDelete(DeleteBehavior.Cascade);


			modelBuilder.Entity<Appointment>()
				.HasOne(a => a.Patient)
				.WithMany(p => p.Appointments)
				.HasForeignKey(a => a.PatientId)
				.OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<Appointment>()
				.HasOne(a => a.Doctor)
				.WithMany(d => d.Appointments)
				.HasForeignKey(a => a.DoctorId)
				.OnDelete(DeleteBehavior.SetNull);
		}
	}
}
