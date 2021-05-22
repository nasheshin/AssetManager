using System.Data.Entity;

namespace AssetManagerServer.Models
{
    public class DataInitializer : DropCreateDatabaseAlways<DataContext>
    {
        protected override void Seed(DataContext database)
        {
            //database.Users.Add(new User { Name = "Kolyan", Password = "Л. Толстой", BrokerId = 220 });
            //database.Users.Add(new User { Name = "Отцы и дети", Password = "И. Тургенев", BrokerId = 180 });
            //database.Users.Add(new User { Name = "Чайка", Password = "А. Чехов", BrokerId = 150 });
 
            base.Seed(database);
        }
    }
}