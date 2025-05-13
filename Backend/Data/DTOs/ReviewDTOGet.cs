namespace Hospital.Data.DTOs
{
    public class ReviewDTOGet
    {
        public Guid ReviewId { get; set; } = Guid.NewGuid();
        public string ReviewText { get; set; } = string.Empty;
        public double Rating { get; set; } = 0;
        public DateTime ReviewDate { get; set; } = DateTime.Now;
        public Guid PatientId { get; set; }

    }
}
