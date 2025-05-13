namespace Hospital.Data.Models
{
    public class ReviewForHospital
    {
        [Key]
        public Guid ReviewId { get; set; } = Guid.NewGuid();
        [Required]
        public string ReviewText { get; set; } = string.Empty;
        [Required]
        public double Rating { get; set; } = 0;
        [Required]
        public DateTime ReviewDate { get; set; } = DateTime.Now;
        [Required]
        public Guid PatientId { get; set; } = Guid.NewGuid();
        public Patient? Patient { get; set; }
    
      
    }
}
