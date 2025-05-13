namespace Hospital.Helper
{
    public class JWT
    {
        public string Key{ get; set; }
        public string Issure { get; set; }
        public string Audience { get; set; }
        public double  DurationInDays { get; set; }
    }
}
