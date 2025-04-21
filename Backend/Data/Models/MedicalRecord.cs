namespace Hospital.Data.Models
{
	[Index(nameof(Diagnosis))]
	public class MedicalRecord
	{
		[Key]
		public Guid MedicalRecordId { get; set; } = Guid.NewGuid();
		[Required]
		public DateTime RecordDate { get; set; }
		public string? Notes { get; set; }
		[Required, MaxLength(200)]
		public string? Diagnosis { get; set; }
		[Required, MaxLength(200)]
		public string? Treatment { get; set; }
		public Patient? Patient { get; set; }
		public Guid? PatientId { get; set; }
	}
}
