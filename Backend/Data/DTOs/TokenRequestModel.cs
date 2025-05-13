namespace Hospital.Data.DTOs
{
    public class TokenRequestModel
    {
        [Required,MaxLength(200)]
        public string Email{ get; set; }
        [Required, MaxLength(200)]
        public string Password { get; set; }
    }
}
