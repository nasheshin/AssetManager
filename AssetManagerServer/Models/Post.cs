using System;

namespace AssetManagerServer.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Datetime { get; set; }
        public int UserId { get; set; }
    }
}