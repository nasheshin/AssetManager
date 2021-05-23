using System;

namespace AssetManagerServer.Models
{
    public class NewsItem
    {
        public NewsItem()
        {
            
        }

        public NewsItem(NewsItem newsItem)
        {
            Id = newsItem.Id;
            Header = newsItem.Header;
            Text = newsItem.Text;
            Datetime = newsItem.Datetime;
        }
        
        public int Id { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }
        public DateTime Datetime { get; set; }
    }
}