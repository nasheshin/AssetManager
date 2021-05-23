using System;

namespace AssetManagerServer.Models
{
    public class Post
    {
        public Post()
        {
            
        }

        public Post(Post post)
        {
            Id = post.Id;
            Text = post.Text;
            Datetime = post.Datetime;
            UserId = post.UserId;
        }
        
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Datetime { get; set; }
        public int UserId { get; set; }
    }
}