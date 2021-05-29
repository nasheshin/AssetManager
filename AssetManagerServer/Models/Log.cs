using System;

namespace AssetManagerServer.Models
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime Datetime { get; set; }
        public int UserId { get; set; }
        public string HostAddress { get; set; }
        public string UserAgent { get; set; }
        public string Message { get; set; }
    }
}