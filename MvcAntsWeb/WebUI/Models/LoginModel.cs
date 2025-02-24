﻿using System.ComponentModel.DataAnnotations;

namespace Algorithm.Models
{
    public class LoginModel
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string StringView
        {
            get { return string.Format("Name: {0}, Password: {1}", Name, Password); }
        }
    }
}