using System;
using System.Data.Entity;

namespace AssetManagerServer.Models
{
    public class TestDataInitializer : DropCreateDatabaseAlways<DataContext>
    {
        protected override void Seed(DataContext database)
        {
            // Инициализация пользователей
            database.Users.Add(new User {Name = "TestUser", Password = "TestPassword", BrokerId = 1});

            // Инициализация брокеров
            database.Brokers.Add(new Broker {Name = "TestBroker", Id = 1});

            // Инициализация операций
            database.Operations.Add(new Operation
            {
                AssetName = "TestName", AssetTicker = "TestTicker", AssetType = "TestType",
                Datetime = new DateTime(2021, 05, 22), Type = 1, Price = 0, UserId = 1, BrokerId = 1,
                AssetAnalyticId = 1
            });

            // Инициализация аналитики
            database.AssetAnalytics.Add(new AssetAnalytic {AssetName = "TestAnalytic", BuyRate = 1, SellRate = 1});

            // Инициализация постов
            database.Posts.Add(new Post
            {
                Text = "TestText",
                Datetime = new DateTime(2021, 05, 23, 14, 1, 57),
                UserId = 1
            });

            // Инициализация новостей
            database.NewsItems.Add(new NewsItem
            {
                Header = "TestHeade",
                Text = "TestText",
                Datetime = new DateTime(2021, 05, 21, 15, 23, 40)
            });
            base.Seed(database);
        }
    }
}