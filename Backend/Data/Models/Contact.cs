using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Data.Models
{

    [NotMapped]
    public class Contact
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
    }
}
