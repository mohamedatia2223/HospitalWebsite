namespace Hospital.Data.DTOs
{
	public class MedicalRecordDTOGet
    {
        public Guid MedicalRecordId { get; set; } = Guid.NewGuid();
        public DateTime RecordDate { get; set; }
		public string? Notes { get; set; }
		public string? Diagnosis { get; set; }
		public string? Treatment { get; set; }
		public Guid? PatientId { get; set; }
	}
}
