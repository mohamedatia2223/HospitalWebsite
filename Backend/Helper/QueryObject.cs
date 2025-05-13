namespace Finance_Project.Helper
{
    public class QueryObject
    {
        public string? DoctorName { get; set; } = null;
        public string? Specialty { get; set; } = null;
        public int? YearsOfExperience { get; set; } = null;
        public string? SortBy { get; set; } = null;
        public bool IsDescending { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
