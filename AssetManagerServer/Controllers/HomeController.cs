using System;
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

            if (matchedUsers.Any())
            {
                Session["userId"] = matchedUsers.ToList()[0].Id;
                return $"Успешно! <a href={'"' + "/Home/Profile" + '"'}>Войти в профиль</a>";
            }
            else
            {
                return $"Неправильный логин или пароль <a href={'"' + "/Home/Sign" + '"'}>Повторить вход</a>";
            }
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
                brokers.Add(new Broker { Name = rawUser.BrokerName });

            var brokerInDb = brokers.Last();
            users.Add(new User {Name = rawUser.Name, Password = rawUser.Password, BrokerId = brokerInDb.Id});

            _database.SaveChanges();

            return $"Регистрация прошла успешно <a href={'"' + "/Home/Sign" + '"'}>Вернуться к окну входа</a>";
        }

        [HttpGet]
        public new ActionResult Profile()
        {
            ViewBag.Operations = _database.Operations;
            ViewBag.Brokers = _database.Brokers;
            ViewBag.AssetAnalytics = _database.AssetAnalytics;
            return View();
        }

        [HttpGet]
        public ActionResult Operations()
        {
            ViewBag.Operations = _database.Operations;
            ViewBag.Brokers = _database.Brokers;
            ViewBag.AssetAnalytics = _database.AssetAnalytics;
            return View();
        }
        
        [HttpGet]
        public ActionResult Analytics()
        {
            ViewBag.AssetAnalytics = _database.AssetAnalytics;
            return View();
        }

        [HttpGet]
        public ActionResult Posts()
        {
            ViewBag.Posts = _database.Posts;
            ViewBag.Users = _database.Users;
            return View();
        }

        [HttpGet]
        public ActionResult News()
        {
            ViewBag.NewsItems = _database.NewsItems;
            return View();
        }

        [HttpGet]
        public ActionResult AddAsset(int operationId = -1)
        {
            ViewBag.OperationId = operationId;
            return View();
        }

        [HttpPost]
        public string AddAsset(RawOperationToAdd rawOperation)
        {
            var (isValid, message) = rawOperation.Validate();
            if (!isValid)
                return $"{message} <a href={'"' + "/Home/AddAsset/" + ViewBag.OperationId + '"'}>Повторить регистрацию</a>";
            
            rawOperation.SaveFormattedOperation(_database, int.Parse(Session["userId"].ToString()));
            return $"{message} <a href={'"' + "/Home/Profile" + '"'}>Вернуться к профилю</a>";
        }
    }
}