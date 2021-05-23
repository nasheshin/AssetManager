using System;
using System.Linq;
using AssetManagerServer.Models;

namespace AssetManagerServer.HelpObjects
{
    public class RawOperationToAdd
    {
        public string AssetName { get; set; }
        public string AssetTicker { get; set; }
        public string AssetType { get; set; }
        public string Datetime { get; set; }
        public string Price { get; set; }
        public string BrokerName { get; set; }
        public string Count { get; set; }

        public Tuple<bool, string> Validate()
        {
            if (!DateTime.TryParse(Datetime, out _))
                return new Tuple<bool, string>(false, "Неккоретно введена дата - попробуйте формат: 01.01.0001");
            if (!float.TryParse(Price, out _))
                return new Tuple<bool, string>(false, "Неккоректно введена цена сделки - проверьте не написали ли вы число через точку");
            if (!int.TryParse(Count, out _))
                return new Tuple<bool, string>(false, "Неверно введено кол-во активо, которые необходимо добавить");
            return new Tuple<bool, string>(true, "Актив был добавлен");
        }

        public void SaveFormattedOperation(DataContext database, int userId)
        {
            var assetNameLower = AssetName.ToLower();
            var brokerNameLower = BrokerName.ToLower();
            var datetimeParsed = DateTime.Parse(Datetime);
            var priceParsed = float.Parse(Price);
            var countParsed = int.Parse(Count);

            var assetAnalytics = database.AssetAnalytics;
            var brokers = database.Brokers;

            var assetAnalyticId = -1;
            var assetAnalyticsMatched =
                assetAnalytics.Where(analytic => analytic.AssetName.ToLower() == assetNameLower);
            if (assetAnalyticsMatched.Any())
                assetAnalyticId = assetAnalyticsMatched.First().Id;

            var brokerId = -1;
            var brokerMatched = brokers.Where(broker => broker.Name.ToLower() == brokerNameLower);
            if (brokerMatched.Any())
                brokerId = brokerMatched.First().Id;
            else
            {
                brokers.Add(new Broker {Name = BrokerName});
                brokerId = brokers.Last().Id;
            }

            for (var i = 0; i < countParsed; i++)
            {
                database.Operations.Add(new Operation
                {
                    AssetName = AssetName, AssetTicker = AssetTicker, AssetType = AssetType, Datetime = datetimeParsed,
                    Type = 1, Price = priceParsed, UserId = userId, BrokerId = brokerId,
                    AssetAnalyticId = assetAnalyticId
                });
            }

            database.SaveChanges();
        }
    }
}