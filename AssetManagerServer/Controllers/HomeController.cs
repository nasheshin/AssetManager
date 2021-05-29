﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AssetManagerServer.HelpObjects;
using AssetManagerServer.Models;
using AssetManagerServer.Utils;

namespace AssetManagerServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _database = new DataContext();
        
        private PrivacyValidator _privacyValidator = new PrivacyValidator();
        
        public ActionResult Index()
        {
            return Redirect("/Home/Sign");
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
            var matchedUser = _database.Users.FirstOrDefault(curUser => curUser.Name == user.Name && curUser.Password == user.Password);

            if (matchedUser != null)
            {
                Session["userId"] = matchedUser.Id;
                return Redirect("/Home/Profile");
            }
            else
            {
                return Redirect($"/Home/Sign?notifyMessage={Constants.Sign.WrongUsernameOrPassword}");
            }
        }

        [HttpGet]
        public ActionResult Register(string notifyMessage = "")
        {
            ViewBag.NotifyMessage = notifyMessage;
            return View();
        }
        
        [HttpPost]
        public ActionResult Register(RawUser rawUser)
        {
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

        [HttpGet]
        public new ActionResult Profile()
        {
            if (Session["userId"] == null)
                return Redirect("/Home/Sign");
            
            ViewBag.Operations = _database.Operations;
            ViewBag.Brokers = _database.Brokers;
            ViewBag.AssetAnalytics = _database.AssetAnalytics;
            return View();
        }

        [HttpGet]
        public ActionResult Operations()
        {
            if (Session["userId"] == null)
                return Redirect("/Home/Sign");
            
            ViewBag.Operations = _database.Operations;
            ViewBag.Brokers = _database.Brokers;
            ViewBag.AssetAnalytics = _database.AssetAnalytics;
            return View();
        }
        
        [HttpGet]
        public ActionResult Analytics()
        {
            if (Session["userId"] == null)
                return Redirect("/Home/Sign");
            
            ViewBag.AssetAnalytics = _database.AssetAnalytics;
            return View();
        }

        [HttpGet]
        public ActionResult Posts()
        {
            if (Session["userId"] == null)
                return Redirect("/Home/Sign");
            
            ViewBag.Posts = _database.Posts;
            ViewBag.Users = _database.Users;
            return View();
        }

        [HttpPost]
        public ActionResult Posts(Post post)
        {
            post.Datetime = DateTime.Now;
            _database.Posts.Add(post);
            _database.SaveChanges();
            
            return Redirect("/Home/Posts");
        }

        [HttpGet]
        public ActionResult News()
        {
            if (Session["userId"] == null)
                return Redirect("/Home/Sign");
            
            ViewBag.NewsItems = _database.NewsItems;
            return View();
        }

        [HttpGet]
        public ActionResult AddAsset(int operationId = -1, string notifyMessage = "")
        {
            if (Session["userId"] == null)
                return Redirect("/Home/Sign");
            
            if (!_privacyValidator.IsUserHasOperation(operationId, int.Parse(Session["userId"].ToString()), _database))
                return HttpNotFound();
            
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
            if (Session["userId"] == null)
                return Redirect("/Home/Sign");
            
            if (!_privacyValidator.IsUserHasOperation(operationId, int.Parse(Session["userId"].ToString()), _database))
                return HttpNotFound();
            
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
            if (Session["userId"] == null)
                return Redirect("/Home/Sign");
            
            if (!_privacyValidator.IsUserHasOperation(operationId, int.Parse(Session["userId"].ToString()), _database))
                return HttpNotFound();
            
            var operations = _database.Operations.ToList();
            var operationToCopy = operations.First(operation => operation.Id == operationId);
            _database.Operations.Add(operationToCopy);
            _database.SaveChanges();
            
            return Redirect("/Home/Operations");
        }

        [HttpGet]
        public ActionResult DeleteOperation(int operationId = -1)
        {
            if (Session["userId"] == null)
                return Redirect("/Home/Sign");
            
            if (!_privacyValidator.IsUserHasOperation(operationId, int.Parse(Session["userId"].ToString()), _database))
                return HttpNotFound();
            
            var operationToDelete = _database.Operations.FirstOrDefault(o => o.Id == operationId);

            if (operationToDelete == null)
                return HttpNotFound();
            
            _database.Operations.Remove(operationToDelete);
            _database.SaveChanges();
            
            return Redirect("/Home/Operations");
        }
    }
}