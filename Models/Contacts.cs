using System.ComponentModel.DataAnnotations;

namespace garagebackend.Models
{
    public class Contacts
    {
        [Key]
        public int contactID { get; set; }
        public string company {  get; set; }
        public string contactname { get; set; }
        public string phoneno { get; set; }
        public string email { get; set; }
        public string note { get; set; }
        public DateTime createdAt { get; set; }
        public int credID { get; set; }
    }
}
