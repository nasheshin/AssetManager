using System.Collections.Generic;
using System.Data.Entity;

namespace AssetManagerServer.Models
{
    public class User
    {
        public static List<User> GetUsersFromDbset(IEnumerable<User> dbsetUsers)
        {
            var users = new List<User>();
            foreach (var user in dbsetUsers)
            {
                users.Add(new User((User) user));
            }
            return users;
        }
        
        public User()
        {
            
        }

        public User(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Password = user.Password;
            BrokerId = user.BrokerId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int BrokerId { get; set; }
    }
}