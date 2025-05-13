namespace Hospital.Data
{
	public class HospitalContext : DbContext
	{
		public HospitalContext(DbContextOptions<HospitalContext> options) : base(options)
		{
		}
		public DbSet<Patient> Patients { get; set; }
		public DbSet<Doctor> Doctors { get; set; }
		public DbSet<Admin> Admins { get; set; }
		public DbSet<MedicalRecord> MedicalRecords { get; set; }
		public DbSet<Appointment> Appointments { get; set; }
        public DbSet<ReviewForHospital> Reviews { get; set; }
		public DbSet<DoctorPatient> DoctorPatients { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<DoctorPatient>()
				.HasKey(dp =>dp.DoctorPatientId);

			modelBuilder.Entity<DoctorPatient>()
				.HasOne(dp => dp.Doctor)
				.WithMany(d => d.DoctorPatients)
				.HasForeignKey(dp => dp.DoctorId)
				.OnDelete(DeleteBehavior.Cascade);
			
			
			modelBuilder.Entity<DoctorPatient>()
				.HasOne(dp => dp.Patient)
				.WithMany(p => p.DoctorPatients)
				.HasForeignKey(dp => dp.PatientId)
				.OnDelete(DeleteBehavior.Cascade);


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
