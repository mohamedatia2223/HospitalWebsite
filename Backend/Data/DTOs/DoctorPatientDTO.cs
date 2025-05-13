namespace Hospital.Data.DTOs
{
    public class DoctorPatientDTO
    {   
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public double Rating { get; set; }
        public string? ReviewText { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.Now;

    }
}
