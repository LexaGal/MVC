using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Algorithm.Authentication
{
    public class User
    {
        public User(string name, string passwordHash)
        {
            Name = name;
            PasswordHash = passwordHash;
        }

        public User()
        {}

        [Key]
        public int Id { get; set; }
        public string Name { get; private set; }
        public string PasswordHash { get; private set; }

        public string Data
        {
            get { return string.Format("Name: {0}, PasswordHash: {1}", Name, PasswordHash); }
        }
    }
}