namespace Hospital.Data.DTOs
{
    public class RegisterModel
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; }
        [Required, MaxLength(100)]
        public string LastName { get; set; }
        [Required, MaxLength(100)]
        public string Email { get; set; }
        [Required, MaxLength(100)]
        public string UserName { get; set; }
        [Required, MaxLength(100)]
        public string Password { get; set; }
    }
}
