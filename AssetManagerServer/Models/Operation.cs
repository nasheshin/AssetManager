namespace AssetManagerServer.Models
{
    public class Operation
    {
        public int Id { get; set; }
        public string AssetName { get; set; }
        public string AssetTicker { get; set; }
        public string AssetType { get; set; }
        public int Type { get; set; }
        public int UserId { get; set; }
        public int BrokerId { get; set; }
        public int AssetAnalyticId { get; set; }
    }
}