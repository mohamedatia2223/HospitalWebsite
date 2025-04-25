namespace Hospital.Data.Models
{
    public class Admin
    {
        [Key]
        public Guid AdminId {get; set;} = Guid.NewGuid();
        [Required]        
        public string AdminName { get; set; } = "";
        [Required]        
        public string Email { get; set; } = "";
        [Required]        
        public string Password { get; set; } = ""; 
    }

}