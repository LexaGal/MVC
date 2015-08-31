using System.ComponentModel.DataAnnotations;

namespace Entities.DatabaseModels
{
    public class User
    {
        public User()
        {}

        public User(string name, string passwordHash)
        {
            Name = name;
            PasswordHash = passwordHash;
        }
        
        [Key]
        public int Id { get; set; }
        public string Name { get; private set; }
        public string PasswordHash { get; private set; }

        public string StringView
        {
            get { return string.Format("Name: {0}, PasswordHash: {1}", Name, PasswordHash); }
        }
    }
}