namespace Hospital.Data.DTOs
{
    public class ReviewDTOUpdate
    {
        public string ReviewText { get; set; } = string.Empty;
        public double Rating { get; set; } = 0;
        public DateTime ReviewDate { get; set; } = DateTime.Now;
    }
}
