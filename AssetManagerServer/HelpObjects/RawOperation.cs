using System;
using System.Linq;
using AssetManagerServer.Models;

namespace AssetManagerServer.HelpObjects
{
    public class RawOperation
    {
        public int OperationId { get; set; }
        public string AssetName { get; set; }
        public string AssetTicker { get; set; }
        public string AssetType { get; set; }
        public DateTime Datetime { get; set; }
        public float Price { get; set; }
        public string BrokerName { get; set; }
        public int Count { get; set; }

        public void SaveFormattedOperation(DataContext database, int userId, int type)
        {
            var assetNameLower = AssetName.ToLower();
            var brokerNameLower = BrokerName.ToLower();

            var assetAnalytics = database.AssetAnalytics;
            var brokers = database.Brokers;

            var assetAnalyticId = 3;
            var assetAnalyticsMatched =
                assetAnalytics.Where(analytic => analytic.AssetName.ToLower() == assetNameLower);
            if (assetAnalyticsMatched.Any())
                assetAnalyticId = assetAnalyticsMatched.First().Id;

            int brokerId;
            var brokerMatched = brokers.Where(broker => broker.Name.ToLower() == brokerNameLower);
            if (brokerMatched.Any())
                brokerId = brokerMatched.First().Id;
            else
            {
                brokers.Add(new Broker {Name = BrokerName});
                brokerId = brokers.Last().Id;
            }

            for (var i = 0; i < Count; i++)
            {
                database.Operations.Add(new Operation
                {
                    AssetName = AssetName, AssetTicker = AssetTicker, AssetType = AssetType, Datetime = Datetime,
                    Type = type, Price = Price, UserId = userId, BrokerId = brokerId,
                    AssetAnalyticId = assetAnalyticId
                });
            }

            database.SaveChanges();
        }
    }
}