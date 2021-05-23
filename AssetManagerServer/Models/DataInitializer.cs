using System;
using System.Data.Entity;

namespace AssetManagerServer.Models
{
    public class DataInitializer : DropCreateDatabaseAlways<DataContext>
    {
        protected override void Seed(DataContext database)
        {
            // Инициализация пользователей
            database.Users.Add(new User { Name = "Kolyan", Password = "123456", BrokerId = 1 });
            database.Users.Add(new User {Name = "admin", Password = "123", BrokerId = 2});
            
            // Инициализация брокеров
            database.Brokers.Add(new Broker {Name = "Тинькофф", Id = 1});
            database.Brokers.Add(new Broker {Name = "Открытие", Id = 2});
            
            // Инициализация операций
            database.Operations.Add(new Operation
            {
                AssetName = "ПермЭнергоСбыт", AssetTicker = "PMSB", AssetType = "Акция",
                Datetime = new DateTime(2021, 05, 22), Type = 1, Price = 100, UserId = 1, BrokerId = 1,
                AssetAnalyticId = 1
            });
            database.Operations.Add(new Operation
            {
                AssetName = "ПермЭнергоСбыт", AssetTicker = "PMSB", AssetType = "Акция",
                Datetime = new DateTime(2021, 05, 20), Type = 1, Price = 90, UserId = 1, BrokerId = 1,
                AssetAnalyticId = 1
            });
            database.Operations.Add(new Operation
            {
                AssetName = "Сбербанк", AssetTicker = "SBER", AssetType = "Акция",
                Datetime = new DateTime(2021, 04, 10), Type = 1, Price = 300, UserId = 1, BrokerId = 1,
                AssetAnalyticId = -1
            });
            database.Operations.Add(new Operation
            {
                AssetName = "Сбербанк", AssetTicker = "SBER", AssetType = "Акция",
                Datetime = new DateTime(2021, 03, 10), Type = -1, Price = 500, UserId = 1, BrokerId = 1,
                AssetAnalyticId = -1
            });
            database.Operations.Add(new Operation
            {
                AssetName = "Сбербанк", AssetTicker = "SBER", AssetType = "Акция",
                Datetime = new DateTime(2021, 02, 10), Type = 1, Price = 400, UserId = 1, BrokerId = 1,
                AssetAnalyticId = -1
            });
            database.Operations.Add(new Operation
            {
                AssetName = "ФБК", AssetTicker = "FFCN", AssetType = "Фонд",
                Datetime = new DateTime(2021, 02, 10), Type = 1, Price = 10, UserId = 1, BrokerId = 1,
                AssetAnalyticId = -1
            });
            database.Operations.Add(new Operation
            {
                AssetName = "МТС", AssetTicker = "MTSS", AssetType = "Акция",
                Datetime = new DateTime(2020, 01, 22), Type = 1, Price = 200, UserId = 2, BrokerId = 2,
                AssetAnalyticId = 2
            });
            
            // Инициализация аналитики
            database.AssetAnalytics.Add(new AssetAnalytic {AssetName = "ПермЭнергоСбыт", BuyRate = 2, SellRate = 7});
            database.AssetAnalytics.Add(new AssetAnalytic {AssetName = "МТС", BuyRate = 7, SellRate = 2});

            // Инициализация постов
            database.Posts.Add(new Post
            {
                Text = "Офигеть, как много денег я зработал за сегодня!!!!!",
                Datetime = new DateTime(2021, 05, 23, 14, 1, 57),
                UserId = 1
            });
            database.Posts.Add(new Post
            {
                Text = "Блин, а я тольк все потерял((((",
                Datetime = new DateTime(2021, 05, 23, 14, 10, 00),
                UserId = 2
            });
            
            // Инициализация новостей
            database.NewsItems.Add(new NewsItem
            {
                Header = "Немецкую трейдинговую платформу Trade Republic оценили в $5 млрд",
                Text = "\nТрейдинговая платформа Trade Republic Bank GmbH пытается повторить успех американской Robinhood" +
                       " Market Inc. среди молодых инвесторов." + 
                       "\nВ новом раунде помимо Sequoia Capital приняли участие венчурные инвесторы TCV и Thrive Capital" + 
                       " и действующие инвесторы компании Creandum, Accel, Project A и Founders Fund." + 
                       "\nВ итоге Trade Republic стала одной из наиболее дорогих частных финтех-компаний в Европе",
                Datetime = new DateTime(2021, 05, 21, 15, 23, 40)
            });
            database.NewsItems.Add(new NewsItem
            {
                Header = "Lamborghini вложит €1,5 млрд в электрификацию своих моделей",
                Text = "Гендиректор Lamborghini Стефан Винкельманн заявил, что гибридные версии появятся, в том числе " +
                       "кроссовера Urus и 6,5-литрового Aventador. Lamborghini также выпустит свой первый полностью " +
                       "электрический автомобиль во второй половине десятилетия." + "\nКак сообщает Bloomebrg, бренд " +
                       "потратит €1,5 млрд на разработку нового парка автомобилей, который, начиная с 2025 года, вдвое" +
                       " сократит выбросы по всей линейке продуктов. Эти расходы станут крупнейшей инвестицией Lamborghini" +
                       " за всю историю.",
                Datetime = new DateTime(2021, 05, 19, 21, 33, 11)
            });
            base.Seed(database);
        }
    }
}