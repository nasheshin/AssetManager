using System;

namespace AssetManagerServer.Models
{
    public class Operation
    {
        public int Id { get; set; }
        public string AssetName { get; set; }
        public string AssetTicker { get; set; }
        public string AssetType { get; set; }
        public DateTime Datetime { get; set; }
        public int Type { get; set; }
        public float Price { get; set; }
        public int UserId { get; set; }
        public int BrokerId { get; set; }
        public int AssetAnalyticId { get; set; }
    }
}