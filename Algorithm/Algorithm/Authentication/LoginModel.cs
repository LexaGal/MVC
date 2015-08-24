using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Algorithm.Authentication
{
    public class LoginModel
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public LoginModel(){}

        public string Data
        {
            get { return string.Format("Name: {0}, Password: {1}", Name, Password); }
        }
    }
}