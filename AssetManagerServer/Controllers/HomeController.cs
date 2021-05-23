using System.Linq;
using System.Web.Mvc;
using AssetManagerServer.HelpObjects;
using AssetManagerServer.Models;

namespace AssetManagerServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _database = new DataContext();
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Sign()
        {
            return View();
        }
        
        [HttpPost]
        public string Sign(User user)
        {
            var users = _database.Users;
            var matchedUsers = users.Where(curUser => curUser.Name == user.Name && curUser.Password == user.Password);

            return matchedUsers.Any()
                ? $"Успешно! <a href={'"' + "/Home/Profile" + '"'}>Войти в профиль</a>"
                : $"Неправильный логин или пароль <a href={'"' + "/Home/Sign" + '"'}>Повторить вход</a>";
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public string Register(RawUser rawUser)
        {
            if (rawUser.Password != rawUser.PasswordAgain)
                return $"Пароли не совпадают <a href={'"' + "/Home/Register" + '"'}>Повторить регистрацию</a>";
            
            var users = _database.Users;
            var matchedByNameUsers = users.Where(curUser => curUser.Name == rawUser.Name);

            if (matchedByNameUsers.Any())
                return $"Данное имя занято <a href={'"' + "/Home/Register" + '"'}>Повторить регистрацию</a>";
            
            var brokers = _database.Brokers;
            var matchedBrokers = brokers.Where(curBroker => curBroker.Name == rawUser.BrokerName);
            if (!matchedBrokers.Any())
            {
                brokers.Add(new Broker { Name = rawUser.BrokerName });
            }

            _database.SaveChanges();

            var brokerInDb = brokers.Where(broker => broker.Name == rawUser.BrokerName).ToList()[0];
            users.Add(new User {Name = rawUser.Name, Password = rawUser.Password, BrokerId = brokerInDb.Id});

            _database.SaveChanges();

            return $"Регистрация прошла успешно <a href={'"' + "/Home/Sign" + '"'}>Вернуться к окну входа</a>";
        }

        [HttpGet]
        public ActionResult Profile()
        {
            return View();
        }
        
    }
}