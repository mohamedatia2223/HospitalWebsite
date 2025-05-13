using Microsoft.AspNetCore.Identity;

namespace Hospital.Data.Models
{
    public class AppUser : IdentityUser
    {
        [Required, MaxLength(200)]
        public string FirstName { get; set; }
        [Required, MaxLength(200)]
        public string LastName { get; set; }
    }
}
