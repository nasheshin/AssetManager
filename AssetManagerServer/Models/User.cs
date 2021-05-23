namespace AssetManagerServer.Models
{
    public class User
    {
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