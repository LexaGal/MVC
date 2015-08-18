using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Algorithm.Authentication
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}