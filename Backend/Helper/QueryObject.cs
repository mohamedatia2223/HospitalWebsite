namespace HospitalApp.Helper
{
    public class QueryObject
    {
        public string? DoctorName { get; set; } = null;
        public string? Specialty { get; set; } = null;
        public int? YearsOfExperience { get; set; } = null;
        public float minRating { get; set; } = 0; 
        public string? SortBy { get; set; } = null;
        public bool IsDescending { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
