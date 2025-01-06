using System.ComponentModel.DataAnnotations;

namespace garagebackend.Models
{
    public class Credentials
    {
        [Key]
        public int credID { get; set; }
        public string credentials { get; set; }
        public string secret { get; set; }
        public DateTime createdAt { get; set; }
    }
}
