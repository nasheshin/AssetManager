using System;
using System.Collections.Generic;
using System.Linq;
using AssetManagerServer.Models;

namespace AssetManagerServer.Utils
{
    public static class ConvertingSortingUtils
    {
        public class PortfolioElement
        {
            public string AssetName { get; set; }
            public string AssetTicker { get; set; }
            public string AssetType { get; set; }
            public string BrokerName { get; set; }
            public int Count { get; set; }
            public string BuyRate { get; set; }
            public string SellRate { get; set; }
            public int? Id { get; set; }

            public override string ToString()
            {
                return $"{AssetName},{AssetTicker},{AssetType},{BrokerName},{Count},{BuyRate},{SellRate}";
            }
        }
        
        public class OperationElement
        {
            public string AssetName { get; set; }
            public string AssetTicker { get; set; }
            public string AssetType { get; set; }
            public DateTime Datetime { get; set; }
            public string OperationType { get; set; }
            public string BrokerName { get; set; }
            public float Price { get; set; }
            public string BuyRate { get; set; }
            public string SellRate { get; set; }
            public int? Id { get; set; }

            public override string ToString()
            {
                return $"{AssetName},{AssetTicker},{AssetType},{Datetime:dd.MM.yyyy},{OperationType},{BrokerName},{Price},{BuyRate},{SellRate}";
            }
        }
        
        public static IEnumerable<PortfolioElement> ConvertSortPortfolio(IReadOnlyList<Broker> brokers, IReadOnlyList<Operation> operations,
            IReadOnlyList<AssetAnalytic> assetAnalytics, int sort, bool asc, int userId)
        {
            var operationsCurrentUser = operations.Where(o => o.UserId == userId);
            var operationGroups = operationsCurrentUser.GroupBy(operation =>
                (operation.AssetName, operation.AssetTicker, operation.AssetType)).ToList();

            var portfolio = operationGroups.Select(g => new PortfolioElement
            {
                AssetName = g.Key.Item1, AssetTicker = g.Key.Item2, AssetType = g.Key.Item3,
                BrokerName = brokers.FirstOrDefault(broker => broker.Id == g.FirstOrDefault()?.BrokerId)?.Name,
                Count = g.Sum(o => o.Type),
                BuyRate = assetAnalytics.FirstOrDefault(analytic => analytic.Id == g.First().AssetAnalyticId)?.BuyRate.ToString() ?? "Неизвестно",
                SellRate = assetAnalytics.FirstOrDefault(analytic => analytic.Id == g.First().AssetAnalyticId)?.SellRate.ToString() ?? "Неизвестно",
                Id = g.FirstOrDefault()?.Id
            });

            switch (sort)
            {
                case 0:
                    portfolio = asc
                        ? portfolio.OrderBy(x => x.AssetName)
                        : portfolio.OrderByDescending(x => x.AssetName);
                    break;
                case 1:
                    portfolio = asc
                        ? portfolio.OrderBy(x => x.AssetTicker)
                        : portfolio.OrderByDescending(x => x.AssetTicker);
                    break;
                case 2:
                    portfolio = asc
                        ? portfolio.OrderBy(x => x.AssetType)
                        : portfolio.OrderByDescending(x => x.AssetType);
                    break;
                case 3:
                    portfolio = asc
                        ? portfolio.OrderBy(x => x.BrokerName)
                        : portfolio.OrderByDescending(x => x.BrokerName);
                    break;
                case 4:
                    portfolio = asc ? portfolio.OrderBy(x => x.Count) : portfolio.OrderByDescending(x => x.Count);
                    break;
                case 5:
                    portfolio = asc ? portfolio.OrderBy(x => x.BuyRate) : portfolio.OrderByDescending(x => x.BuyRate);
                    break;
                case 6:
                    portfolio = asc ? portfolio.OrderBy(x => x.SellRate) : portfolio.OrderByDescending(x => x.SellRate);
                    break;
            }

            return portfolio;
        }

        public static IEnumerable<OperationElement> ConvertSortOperations(IReadOnlyList<Broker> brokers,
            IReadOnlyList<Operation> operations, IReadOnlyList<AssetAnalytic> assetAnalytics, int sort, bool asc, int userId)
        {
            var operationsCurrentUser = operations.Where(o => o.UserId == userId).ToList();

            var convertedOperations = operationsCurrentUser.Select(o => new OperationElement
            {
                AssetName = o.AssetName, AssetTicker = o.AssetTicker, AssetType = o.AssetType, Datetime = o.Datetime,
                OperationType = (o.Type == 1 ? "Покупка" : "Продажа"),
                BrokerName = brokers.FirstOrDefault(broker => broker.Id == o.BrokerId)?.Name, Price = o.Price,
                BuyRate = assetAnalytics.FirstOrDefault(analytic => analytic.Id == o.AssetAnalyticId)?.BuyRate.ToString() ?? "Неизвестно",
                SellRate = assetAnalytics.FirstOrDefault(analytic => analytic.Id == o.AssetAnalyticId)?.SellRate.ToString() ?? "Неизвестно",
                Id = o.Id
            });

            switch (sort)
            {
                case 0:
                    convertedOperations =
                        asc
                            ? convertedOperations.OrderBy(x => x.AssetName)
                            : convertedOperations.OrderByDescending(x => x.AssetName);
                    break;
                case 1:
                    convertedOperations =
                        asc
                            ? convertedOperations.OrderBy(x => x.AssetTicker)
                            : convertedOperations.OrderByDescending(x => x.AssetTicker);
                    break;
                case 2:
                    convertedOperations =
                        asc
                            ? convertedOperations.OrderBy(x => x.AssetType)
                            : convertedOperations.OrderByDescending(x => x.AssetType);
                    break;
                case 3:
                    convertedOperations =
                        asc
                            ? convertedOperations.OrderBy(x => x.Datetime)
                            : convertedOperations.OrderByDescending(x => x.Datetime);
                    break;
                case 4:
                    convertedOperations =
                        asc
                            ? convertedOperations.OrderBy(x => x.OperationType)
                            : convertedOperations.OrderByDescending(x => x.OperationType);
                    break;
                case 5:
                    convertedOperations =
                        asc
                            ? convertedOperations.OrderBy(x => x.BrokerName)
                            : convertedOperations.OrderByDescending(x => x.BrokerName);
                    break;
                case 6:
                    convertedOperations =
                        asc
                            ? convertedOperations.OrderBy(x => x.Price)
                            : convertedOperations.OrderByDescending(x => x.Price);
                    break;
                case 7:
                    convertedOperations =
                        asc
                            ? convertedOperations.OrderBy(x => x.BuyRate)
                            : convertedOperations.OrderByDescending(x => x.BuyRate);
                    break;
                case 8:
                    convertedOperations =
                        asc
                            ? convertedOperations.OrderBy(x => x.SellRate)
                            : convertedOperations.OrderByDescending(x => x.SellRate);
                    break;
            }

            return convertedOperations;
        }
    }
}