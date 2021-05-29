using System;

namespace AssetManagerServer.Models
{
    public class NewsItem
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }
        public DateTime Datetime { get; set; }
    }
}