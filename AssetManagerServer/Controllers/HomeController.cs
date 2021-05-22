using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetManagerServer.Models;

namespace AssetManagerServer.Controllers
{
    public class HomeController : Controller
    {
        private DataContext _database = new DataContext();
        
        public ActionResult Index()
        {
            var users = _database.Users;
            ViewBag.Users = users;
            return View();
        }
    }
}