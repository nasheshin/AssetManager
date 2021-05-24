using System;
using System.Collections.Generic;
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
        public ActionResult Sign(string notifyMessage = "")
        {
            ViewBag.NotifyMessage = notifyMessage;
            return View();
        }
        
        [HttpPost]
        public ActionResult Sign(User user)
        {
            var users = Models.User.GetUsersFromDbset(_database.Users);
            var matchedUser = users.FirstOrDefault(curUser => curUser.Name == user.Name && curUser.Password == user.Password);

            if (matchedUser != null)
            {
                Session["userId"] = matchedUser.Id;
                return Redirect("/Home/Profile");
            }
            else
            {
                return Redirect("/Home/Sign?notifyMessage=Неправильный логин или пароль");
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
        public ActionResult AddAsset(int operationId = -1, string notifyMessage = "")
        {
            ViewBag.NotifyMessage = notifyMessage;
            ViewBag.OperationId = operationId;
            ViewBag.Operations = _database.Operations;
            ViewBag.Brokers = _database.Brokers;
            return View();
        }

        [HttpPost]
        public RedirectResult AddAsset(RawOperation rawOperation)
        {
            var (isValid, message) = rawOperation.Validate();
            if (!isValid)
                return Redirect($"/Home/AddAsset?operationId={rawOperation.OperationId}&notifyMessage={message}");
            
            rawOperation.SaveFormattedOperation(_database, int.Parse(Session["userId"].ToString()), 1);
            return Redirect("/Home/Profile");
        }

        [HttpGet]
        public ActionResult DeleteAsset(int operationId = -1, string notifyMessage = "")
        {
            ViewBag.NotifyMessage = notifyMessage;
            ViewBag.OperationId = operationId;
            ViewBag.Operations = _database.Operations;
            ViewBag.Brokers = _database.Brokers;
            return View();
        }
        
        [HttpPost]
        public ActionResult DeleteAsset(RawOperation rawOperation)
        {
            var (isValid, message) = rawOperation.Validate();
            if (!isValid)
                return Redirect($"/Home/AddAsset?operationId={rawOperation.OperationId}&notifyMessage={message}");
            
            rawOperation.SaveFormattedOperation(_database, int.Parse(Session["userId"].ToString()), -1);
            return Redirect("/Home/Profile");
        }
        
        [HttpGet]
        public ActionResult CopyOperation(int operationId = -1)
        {
            ViewBag.OperationId = operationId;
            return View();
        }

        [HttpPost]
        public string CopyOperation(RawOperation rawOperation)
        {
            var operations = new List<Operation>();
            foreach (var o in _database.Operations)
            {
                operations.Add(new Operation(o as Operation));
            }
            var operationToCopy = operations.First(operation => operation.Id == rawOperation.OperationId);
            _database.Operations.Add(operationToCopy);
            _database.SaveChanges();
            
            var message = "Успешное копирование";
            return $"{message} <a href={'"' + "/Home/Operations" + '"'}>Вернуться к операциям</a>";
        }
        
        [HttpGet]
        public ActionResult DeleteOperation(int operationId = -1)
        {
            ViewBag.OperationId = operationId;
            return View();
        }
        
        [HttpPost]
        public string DeleteOperation(RawOperation rawOperation)
        {
            Operation operationToDelete = null;
            foreach (var o in _database.Operations)
            {
                if (o.Id != rawOperation.OperationId)
                    continue;
                
                operationToDelete = o;
            }

            if (operationToDelete == null)
                return $"Ошибка <a href={'"' + "/Home/Operations" + '"'}>Вернуться к операциям</a>";
            
            _database.Operations.Remove(operationToDelete);
            _database.SaveChanges();
            
            var message = "Успешное удаление";
            return $"{message} <a href={'"' + "/Home/Operations" + '"'}>Вернуться к операциям</a>";
        }

        [HttpGet]
        public ActionResult NewPost()
        {
            ViewBag.Posts = _database.Posts;
            return View();
        }

        [HttpPost]
        public string NewPost(Post post)
        {
            post.Datetime = DateTime.Now;
            _database.Posts.Add(post);
            _database.SaveChanges();

            var message = "Пост был успешно отправлен";
            return $"{message} <a href={'"' + "/Home/Posts" + '"'}>Вернуться к постам</a>";
        }
    }
}