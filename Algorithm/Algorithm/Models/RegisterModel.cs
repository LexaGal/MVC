using System.ComponentModel.DataAnnotations;

namespace Algorithm.Models
{
    public class RegisterModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        public string StringView
        {
            get { return string.Format("Name: {0}, Password: {1}, ConfirmPassword: {2}", Name, Password, ConfirmPassword); }
        }
    }
}
