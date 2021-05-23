namespace AssetManagerServer.Models
{
    public class AssetAnalytic
    {
        public AssetAnalytic()
        {
            
        }

        public AssetAnalytic(AssetAnalytic analytic)
        {
            Id = analytic.Id;
            AssetName = analytic.AssetName;
            BuyRate = analytic.BuyRate;
            SellRate = analytic.SellRate;
        }
        
        public int Id { get; set; }
        public string AssetName { get; set; }
        public int BuyRate { get; set; }
        public int SellRate { get; set; }
    }
}