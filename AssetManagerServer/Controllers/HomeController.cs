using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetManagerServer.HelpObjects;
using AssetManagerServer.Models;
using AssetManagerServer.Utils;

namespace AssetManagerServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _database = new DataContext();

        public HomeController(HttpContextBase context)
        {
            ControllerContext = new ControllerContext {HttpContext = context};
            Database.SetInitializer(new TestDataInitializer());
        }
        
        public HomeController()
        {
            
        }
        
        public ActionResult Index()
        {
            return Redirect("/Home/Sign");
        }

        [HttpGet]
        public ActionResult Sign(string notifyMessage = "")
        {
            try
            {
                ViewBag.NotifyMessage = notifyMessage;
                return View("Sign");
            }
            catch (Exception e)
            {
                Logger.AddLog(e.Message, -1, HttpContext.Request, _database);
                return HttpNotFound();
            }
        }
        
        [HttpPost]
        public ActionResult Sign(User user)
        {
            try
            {
                if (!PropertiesValidator.UsernameValidate(user.Name))
                    return Redirect($"/Home/Sign?notifyMessage={Constants.Sign.WrongUsernameOrPassword}");

                if (!PropertiesValidator.PasswordValidate(user.Password))
                    return Redirect($"/Home/Sign?notifyMessage={Constants.Sign.WrongUsernameOrPassword}");

                var matchedUser = _database.Users.FirstOrDefault(curUser =>
                    curUser.Name == user.Name && curUser.Password == user.Password);

                if (matchedUser == null)
                    return Redirect($"/Home/Sign?notifyMessage={Constants.Sign.WrongUsernameOrPassword}");

                Session["userId"] = matchedUser.Id;
                return Redirect("/Home/Profile");
            }
            catch (Exception e)
            {
                Logger.AddLog(e.Message, -1, HttpContext.Request, _database);
                return HttpNotFound();
            }

        }

        [HttpGet]
        public ActionResult Register(string notifyMessage = "")
        {
            try
            {
                ViewBag.NotifyMessage = notifyMessage;
                return View();
            }
            catch (Exception e)
            {
                Logger.AddLog(e.Message, -1, HttpContext.Request, _database);
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult Register(RawUser rawUser)
        {
            try
            {
                if (!PropertiesValidator.UsernameValidate(rawUser.Name))
                    return Redirect($"/Home/Register?notifyMessage={Constants.Register.InvalidUsername}");

                if (!PropertiesValidator.PasswordValidate(rawUser.Password))
                    return Redirect($"/Home/Register?notifyMessage={Constants.Register.InvalidPassword}");

                if (rawUser.Password != rawUser.PasswordAgain)
                    return Redirect($@"/Home/Register?notifyMessage={Constants.Register.WrongRepeatPassword}");

                var users = _database.Users;
                var matchedByNameUsers = users.Where(curUser => curUser.Name == rawUser.Name);

                if (matchedByNameUsers.Any())
                    return Redirect($@"/Home/Register?notifyMessage={Constants.Register.UsernameNotAvailable}");

                var brokers = _database.Brokers;
                var matchedBroker = brokers.FirstOrDefault(curBroker => curBroker.Name == rawUser.BrokerName);
                if (matchedBroker == null)
                {
                    matchedBroker = new Broker {Name = rawUser.BrokerName};
                    brokers.Add(matchedBroker);
                }

                users.Add(new User {Name = rawUser.Name, Password = rawUser.Password, BrokerId = matchedBroker.Id});
                _database.SaveChanges();

                return Redirect($@"/Home/Sign");
            }
            catch (Exception e)
            {
                Logger.AddLog(e.Message, -1, HttpContext.Request, _database);
                return HttpNotFound();
            }
        }

        [HttpGet]
        public ActionResult Profile(int sort = 4, bool asc = false)
        {
            if (Session["userId"] == null)
                return Redirect("/Home/Sign");
            var userId = int.Parse(Session["userId"].ToString());

            try
            {
                ViewBag.IsExportPortfolio = true;
                ViewBag.Sort = sort;
                ViewBag.Asc = asc;
                ViewBag.Operations = _database.Operations;
                ViewBag.Brokers = _database.Brokers;
                ViewBag.AssetAnalytics = _database.AssetAnalytics;
                return View();
            }
            catch (Exception e)
            {
                Logger.AddLog(e.Message, userId, HttpContext.Request, _database);
                return HttpNotFound();
            }
        }

        [HttpGet]
        public ActionResult ExportPortfolio(int sort = 4, bool asc = false)
        {
            if (Session["userId"] == null)
                return Redirect("/Home/Sign");
            var userId = int.Parse(Session["userId"].ToString());

            try
            {
                var brokers = _database.Brokers.ToList();
                var operations = _database.Operations.ToList();
                var assetAnalytics = _database.AssetAnalytics.ToList();
                var portfolio = ConvertingSortingUtils.ConvertSortPortfolio(
                    brokers, operations, assetAnalytics, sort, asc, userId);

                var (filePath, fileName) = ExportUtils.CreatePortfolioCsv(portfolio, userId);
                var fileType = "application/csv";
                
                return File(filePath, fileType, fileName);
            }
            catch (Exception e)
            {
                Logger.AddLog(e.Message, userId, HttpContext.Request, _database);
                return HttpNotFound();
            }
        }

        [HttpGet]
        public ActionResult Operations(int sort = 3, bool asc = false)
        {
            if (Session["userId"] == null)
                return Redirect("/Home/Sign");
            var userId = int.Parse(Session["userId"].ToString());
            
            try
            {
                ViewBag.IsExportOperations = true;
                ViewBag.Sort = sort;
                ViewBag.Asc = asc;
                ViewBag.Operations = _database.Operations;
                ViewBag.Brokers = _database.Brokers;
                ViewBag.AssetAnalytics = _database.AssetAnalytics;
                return View();
            }
            catch (Exception e)
            {
                Logger.AddLog(e.Message, userId, HttpContext.Request, _database);
                return HttpNotFound();
            }
        }
        
        [HttpGet]
        public ActionResult ExportOperations(int sort = 4, bool asc = false)
        {
            if (Session["userId"] == null)
                return Redirect("/Home/Sign");
            var userId = int.Parse(Session["userId"].ToString());

            try
            {
                var brokers = _database.Brokers.ToList();
                var operations = _database.Operations.ToList();
                var assetAnalytics = _database.AssetAnalytics.ToList();
                var convertedOperations = ConvertingSortingUtils.ConvertSortOperations(
                    brokers, operations, assetAnalytics, sort, asc, userId);

                var (filePath, fileName) = ExportUtils.CreateOperationsCsv(convertedOperations, userId);
                var fileType = "application/csv";
                
                return File(filePath, fileType, fileName);
            }
            catch (Exception e)
            {
                Logger.AddLog(e.Message, userId, HttpContext.Request, _database);
                return HttpNotFound();
            }
        }
        
        [HttpGet]
        public ActionResult Analytics(int sort = 0, bool asc = true)
        {
            if (Session["userId"] == null)
                return Redirect("/Home/Sign");
            var userId = int.Parse(Session["userId"].ToString());

            try
            {
                ViewBag.Sort = sort;
                ViewBag.Asc = asc;
                ViewBag.AssetAnalytics = _database.AssetAnalytics;
                return View();
            }
            catch (Exception e)
            {
                Logger.AddLog(e.Message, userId, HttpContext.Request, _database);
                return HttpNotFound();
            }
        }

        [HttpGet]
        public ActionResult Posts()
        {
            if (Session["userId"] == null)
                return Redirect("/Home/Sign");
            var userId = int.Parse(Session["userId"].ToString());
            
            try
            {
                ViewBag.Posts = _database.Posts;
                ViewBag.Users = _database.Users;
                return View();
            }
            catch (Exception e)
            {
                Logger.AddLog(e.Message, userId, HttpContext.Request, _database);
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult Posts(Post post)
        {
            var userId = int.Parse(Session["userId"].ToString());

            try
            {
                if (post.Text == null) 
                    return Redirect("/Home/Posts");
                
                post.Datetime = DateTime.Now;
                _database.Posts.Add(post);
                _database.SaveChanges();

                return Redirect("/Home/Posts");
            }
            catch (Exception e)
            {
                Logger.AddLog(e.Message, userId, HttpContext.Request, _database);
                return HttpNotFound();
            }
        }

        [HttpGet]
        public ActionResult News()
        {
            if (Session["userId"] == null)
                return Redirect("/Home/Sign");
            var userId = int.Parse(Session["userId"].ToString());

            try
            {
                ViewBag.NewsItems = _database.NewsItems;
                return View();
            }
            catch (Exception e)
            {
                Logger.AddLog(e.Message, userId, HttpContext.Request, _database);
                return HttpNotFound();
            }
        }

        [HttpGet]
        public ActionResult AddAsset(int operationId = -1, string notifyMessage = "")
        {
            if (Session["userId"] == null)
                return Redirect("/Home/Sign");
            var userId = int.Parse(Session["userId"].ToString());
            
            try
            {
                if (!PrivacyValidator.IsUserHasOperation(operationId, int.Parse(Session["userId"].ToString()),
                    _database))
                    if (operationId != -1)
                        return HttpNotFound();

                ViewBag.NotifyMessage = notifyMessage;
                ViewBag.OperationId = operationId;
                ViewBag.Operations = _database.Operations;
                ViewBag.Brokers = _database.Brokers;
                return View();
            }
            catch (Exception e)
            {
                Logger.AddLog(e.Message, userId, HttpContext.Request, _database);
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult AddAsset(RawOperation rawOperation)
        {
            var userId = int.Parse(Session["userId"].ToString());
            try
            {
                if (!PropertiesValidator.AssetTickerValidate(rawOperation.AssetTicker))
                    return Redirect(
                        $"/Home/AddAsset?operationId={rawOperation.OperationId}&notifyMessage={Constants.AddDeleteAsset.InvalidAssetTicker}");

                if (!PropertiesValidator.DatetimeValidate(rawOperation.Datetime))
                    return Redirect(
                        $"/Home/AddAsset?operationId={rawOperation.OperationId}&notifyMessage={Constants.AddDeleteAsset.InvalidDatetime}");

                rawOperation.SaveFormattedOperation(_database, int.Parse(Session["userId"].ToString()), 1);
                return Redirect("/Home/Profile");
            }
            catch (Exception e)
            {
                Logger.AddLog(e.Message, userId, HttpContext.Request, _database);
                return HttpNotFound();
            }
        }

        [HttpGet]
        public ActionResult DeleteAsset(int operationId = -1, string notifyMessage = "")
        {
            if (Session["userId"] == null)
                return Redirect("/Home/Sign");
            var userId = int.Parse(Session["userId"].ToString());
            
            try
            {
                if (!PrivacyValidator.IsUserHasOperation(operationId, userId, _database))
                    return HttpNotFound();

                ViewBag.NotifyMessage = notifyMessage;
                ViewBag.OperationId = operationId;
                ViewBag.Operations = _database.Operations;
                ViewBag.Brokers = _database.Brokers;
                return View();
            }
            catch (Exception e)
            {
                Logger.AddLog(e.Message, userId, HttpContext.Request, _database);
                return HttpNotFound();
            }
        }
        
        [HttpPost]
        public ActionResult DeleteAsset(RawOperation rawOperation)
        {
            var userId = int.Parse(Session["userId"].ToString());

            try
            {
                if (!PropertiesValidator.DatetimeValidate(rawOperation.Datetime))
                    return Redirect(
                        $"/Home/DeleteAsset?operationId={rawOperation.OperationId}&notifyMessage={Constants.AddDeleteAsset.InvalidDatetime}");

                var operationsCurrentUser = _database.Operations.Where(o => o.UserId == userId).ToList();
                var similarOperations = operationsCurrentUser.Where(operation =>
                    rawOperation.AssetName == operation.AssetName &&
                    rawOperation.AssetTicker == operation.AssetTicker &&
                    rawOperation.AssetType == operation.AssetType);
                var maxCount = similarOperations.Count();

                if (rawOperation.Count > maxCount)
                    return Redirect(
                        $"/Home/DeleteAsset?operationId={rawOperation.OperationId}&notifyMessage={Constants.AddDeleteAsset.DeleteCountIsBig}");

                rawOperation.SaveFormattedOperation(_database, int.Parse(Session["userId"].ToString()), -1);
                return Redirect("/Home/Profile");
            }
            catch (Exception e)
            {
                Logger.AddLog(e.Message, userId, HttpContext.Request, _database);
                return HttpNotFound();
            }
        }
        
        [HttpGet]
        public ActionResult CopyOperation(int operationId = -1)
        {
            if (Session["userId"] == null)
                return Redirect("/Home/Sign");
            var userId = int.Parse(Session["userId"].ToString());

            try
            {
                if (!PrivacyValidator.IsUserHasOperation(operationId, int.Parse(Session["userId"].ToString()),
                    _database))
                    return HttpNotFound();

                var operations = _database.Operations.ToList();
                var operationToCopy = operations.First(operation => operation.Id == operationId);
                _database.Operations.Add(operationToCopy);
                _database.SaveChanges();

                return Redirect("/Home/Operations");
            }
            catch (Exception e)
            {
                Logger.AddLog(e.Message, userId, HttpContext.Request, _database);
                return HttpNotFound();
            }
        }

        [HttpGet]
        public ActionResult DeleteOperation(int operationId = -1)
        {
            if (Session["userId"] == null)
                return Redirect("/Home/Sign");
            var userId = int.Parse(Session["userId"].ToString());
            
            try
            {
                if (!PrivacyValidator.IsUserHasOperation(operationId, userId, _database))
                    return HttpNotFound();

                var operationToDelete = _database.Operations.FirstOrDefault(o => o.Id == operationId);

                if (operationToDelete == null)
                    return HttpNotFound();

                _database.Operations.Remove(operationToDelete);
                _database.SaveChanges();

                return Redirect("/Home/Operations");
            }
            catch (Exception e)
            {
                Logger.AddLog(e.Message, userId, HttpContext.Request, _database);
                return HttpNotFound();
            }
        }
    }
}