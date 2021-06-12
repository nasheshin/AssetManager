using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using AssetManagerServer.Controllers;
using AssetManagerServer.Models;
using Moq;
using NUnit.Framework;

namespace UnitTesting
{
    [TestFixture]
    public class Tests
    {
        private readonly HomeController _standardHomeController = new HomeController();
        
        [Test]
        public void IndexTest()
        {
            var redirectResult = _standardHomeController.Index() as RedirectResult;
            
            Assert.IsNotNull(redirectResult);
        }
        
        [Test]
        public void SignTest()
        {
            var viewResult = _standardHomeController.Sign() as ViewResult;
            
            Assert.IsNotNull(viewResult);
        }
        
        [Test]
        public void SignTestNotifyMessageNotEmpty()
        {
            var viewResult = _standardHomeController.Sign("Message") as ViewResult;
            
            Assert.AreEqual("Message", viewResult?.ViewBag.NotifyMessage);
        }
        
        [Test]
        public void RegisterTest()
        {
            var viewResult = _standardHomeController.Register() as ViewResult;
            
            Assert.IsNotNull(viewResult);
        }
        
        [Test]
        public void RegisterTestNotifyMessageNotEmpty()
        {
            var viewResult = _standardHomeController.Register("Message") as ViewResult;
            
            Assert.AreEqual("Message", viewResult?.ViewBag.NotifyMessage);
        }
        
        [Test]
        public void ProfileTestWithoutUser()
        {
            var homeController = GetHomeControllerWithUserId(null);
            
            var redirectResult = homeController.Profile() as RedirectResult;
            
            Assert.IsNotNull(redirectResult);
        }
        
        [Test]
        public void ProfileTest()
        {
            var homeController = GetHomeControllerWithUserId("1");
            
            var viewResult = homeController.Profile() as ViewResult;

            Assert.IsNotNull(viewResult);
        }
        
        [Test]
        public void ProfileTestViewBag()
        {
            var homeController = GetHomeControllerWithUserId("1");
            
            var viewResult = homeController.Profile(0, true) as ViewResult;

            Assert.AreEqual(true, viewResult?.ViewBag.IsExportPortfolio);
            Assert.AreEqual(0, viewResult?.ViewBag.Sort);
            Assert.AreEqual(true, viewResult?.ViewBag.Asc);
            Assert.IsInstanceOf(typeof(DbSet<Broker>), viewResult?.ViewBag.Brokers);
            Assert.IsInstanceOf(typeof(DbSet<Operation>), viewResult?.ViewBag.Operations);
            Assert.IsInstanceOf(typeof(DbSet<AssetAnalytic>), viewResult?.ViewBag.AssetAnalytics);
        }
        
        [Test]
        public void ExportPortfolioTestWithoutUser()
        {
            var homeController = GetHomeControllerWithUserId(null);
            
            var redirectResult = homeController.ExportPortfolio() as RedirectResult;
            
            Assert.IsNotNull(redirectResult);
        }
        
        [Test]
        public void ExportPortfolioTest()
        {
            var homeController = GetHomeControllerWithUserId("1");
            
            var filePathResult = homeController.ExportPortfolio() as FilePathResult;

            Assert.IsNotNull(filePathResult);
        }
        
        [Test]
        public void OperationsTestWithoutUser()
        {
            var homeController = GetHomeControllerWithUserId(null);
            
            var redirectResult = homeController.Operations() as RedirectResult;
            
            Assert.IsNotNull(redirectResult);
        }
        
        [Test]
        public void OperationsTest()
        {
            var homeController = GetHomeControllerWithUserId("1");
            
            var viewResult = homeController.Operations() as ViewResult;

            Assert.IsNotNull(viewResult);
        }
        
        [Test]
        public void OperationsTestViewBag()
        {
            var homeController = GetHomeControllerWithUserId("1");
            
            var viewResult = homeController.Operations(0, true) as ViewResult;

            Assert.AreEqual(true, viewResult?.ViewBag.IsExportOperations);
            Assert.AreEqual(0, viewResult?.ViewBag.Sort);
            Assert.AreEqual(true, viewResult?.ViewBag.Asc);
            Assert.IsInstanceOf(typeof(DbSet<Broker>), viewResult?.ViewBag.Brokers);
            Assert.IsInstanceOf(typeof(DbSet<Operation>), viewResult?.ViewBag.Operations);
            Assert.IsInstanceOf(typeof(DbSet<AssetAnalytic>), viewResult?.ViewBag.AssetAnalytics);
        }
        
        [Test]
        public void ExportOperationsTestWithoutUser()
        {
            var homeController = GetHomeControllerWithUserId(null);
            
            var redirectResult = homeController.ExportOperations() as RedirectResult;
            
            Assert.IsNotNull(redirectResult);
        }
        
        [Test]
        public void ExportOperationsTest()
        {
            var homeController = GetHomeControllerWithUserId("1");
            
            var filePathResult = homeController.ExportOperations() as FilePathResult;

            Assert.IsNotNull(filePathResult);
        }
        
        [Test]
        public void AnalyticsTestWithoutUser()
        {
            var homeController = GetHomeControllerWithUserId(null);
            
            var redirectResult = homeController.Analytics() as RedirectResult;
            
            Assert.IsNotNull(redirectResult);
        }
        
        [Test]
        public void AnalyticsTest()
        {
            var homeController = GetHomeControllerWithUserId("1");
            
            var viewResult = homeController.Analytics() as ViewResult;

            Assert.IsNotNull(viewResult);
        }
        
        [Test]
        public void AnalyticsTestViewBag()
        {
            var homeController = GetHomeControllerWithUserId("1");
            
            var viewResult = homeController.Analytics() as ViewResult;
            
            Assert.AreEqual(0, viewResult?.ViewBag.Sort);
            Assert.AreEqual(true, viewResult?.ViewBag.Asc);
            Assert.IsInstanceOf(typeof(DbSet<AssetAnalytic>), viewResult?.ViewBag.AssetAnalytics);
        }
        
        [Test]
        public void PostsTestWithoutUser()
        {
            var homeController = GetHomeControllerWithUserId(null);
            
            var redirectResult = homeController.Posts() as RedirectResult;
            
            Assert.IsNotNull(redirectResult);
        }
        
        [Test]
        public void PostsTest()
        {
            var homeController = GetHomeControllerWithUserId("1");
            
            var viewResult = homeController.Posts() as ViewResult;

            Assert.IsNotNull(viewResult);
        }
        
        [Test]
        public void PostsTestViewBag()
        {
            var homeController = GetHomeControllerWithUserId("1");
            
            var viewResult = homeController.Posts() as ViewResult;
            
            Assert.IsInstanceOf(typeof(DbSet<User>), viewResult?.ViewBag.Users);
            Assert.IsInstanceOf(typeof(DbSet<Post>), viewResult?.ViewBag.Posts);
        }
        
        [Test]
        public void NewsTestWithoutUser()
        {
            var homeController = GetHomeControllerWithUserId(null);
            
            var redirectResult = homeController.News() as RedirectResult;
            
            Assert.IsNotNull(redirectResult);
        }
        
        [Test]
        public void NewsTest()
        {
            var homeController = GetHomeControllerWithUserId("1");
            
            var viewResult = homeController.News() as ViewResult;

            Assert.IsNotNull(viewResult);
        }
        
        [Test]
        public void NewsTestViewBag()
        {
            var homeController = GetHomeControllerWithUserId("1");
            
            var viewResult = homeController.News() as ViewResult;
            
            Assert.IsInstanceOf(typeof(DbSet<NewsItem>), viewResult?.ViewBag.NewsItems);
        }
        
        [Test]
        public void AddAssetTestWithoutUser()
        {
            var homeController = GetHomeControllerWithUserId(null);
            
            var redirectResult = homeController.AddAsset() as RedirectResult;
            
            Assert.IsNotNull(redirectResult);
        }
        
        [Test]
        public void AddAssetTest()
        {
            var homeController = GetHomeControllerWithUserId("1");
            
            var viewResult = homeController.AddAsset() as ViewResult;

            Assert.IsNotNull(viewResult);
        }
        
        [Test]
        public void AddAssetTestUserHasNotOperation()
        {
            var homeController = GetHomeControllerWithUserId("1");
            
            var httpNotFoundResult = homeController.AddAsset(10) as HttpNotFoundResult;

            Assert.IsNotNull(httpNotFoundResult);
        }
        
        [Test]
        public void AddAssetTestViewBag()
        {
            var homeController = GetHomeControllerWithUserId("1");
            
            var viewResult = homeController.AddAsset(-1, "Message") as ViewResult;
            
            Assert.AreEqual("Message", viewResult?.ViewBag.NotifyMessage);
            Assert.AreEqual(-1, viewResult?.ViewBag.OperationId);
            Assert.IsInstanceOf(typeof(DbSet<Operation>), viewResult?.ViewBag.Operations);
            Assert.IsInstanceOf(typeof(DbSet<Broker>), viewResult?.ViewBag.Brokers);
        }
        
        [Test]
        public void DeleteAssetTestWithoutUser()
        {
            var homeController = GetHomeControllerWithUserId(null);
            
            var redirectResult = homeController.DeleteAsset() as RedirectResult;
            
            Assert.IsNotNull(redirectResult);
        }
        
        [Test]
        public void DeleteAssetTest()
        {
            var homeController = GetHomeControllerWithUserId("1");
            
            var viewResult = homeController.DeleteAsset(1) as ViewResult;

            Assert.IsNotNull(viewResult);
        }
        
        [Test]
        public void DeleteAssetTestUserHasNotOperation()
        {
            var homeController = GetHomeControllerWithUserId("1");
            
            var httpNotFoundResult = homeController.DeleteAsset(10) as HttpNotFoundResult;

            Assert.IsNotNull(httpNotFoundResult);
        }
        
        [Test]
        public void DeleteAssetTestViewBag()
        {
            var homeController = GetHomeControllerWithUserId("1");
            
            var viewResult = homeController.DeleteAsset(1, "Message") as ViewResult;
            
            Assert.AreEqual("Message", viewResult?.ViewBag.NotifyMessage);
            Assert.AreEqual(1, viewResult?.ViewBag.OperationId);
            Assert.IsInstanceOf(typeof(DbSet<Operation>), viewResult?.ViewBag.Operations);
            Assert.IsInstanceOf(typeof(DbSet<Broker>), viewResult?.ViewBag.Brokers);
        }
        
        [Test]
        public void CopyOperationTestWithoutUser()
        {
            var homeController = GetHomeControllerWithUserId(null);
            
            var redirectResult = homeController.CopyOperation() as RedirectResult;
            
            Assert.IsNotNull(redirectResult);
        }
        
        [Test]
        public void CopyOperationTestUserHasNotOperation()
        {
            var homeController = GetHomeControllerWithUserId("1");
            
            var httpNotFoundResult = homeController.CopyOperation(10) as HttpNotFoundResult;

            Assert.IsNotNull(httpNotFoundResult);
        }
        
        [Test]
        public void CopyOperationTest()
        {
            var homeController = GetHomeControllerWithUserId("1");
            
            var redirectResult = homeController.CopyOperation(1) as RedirectResult;

            Assert.IsNotNull(redirectResult);
        }
        
        [Test]
        public void DeleteOperationTestWithoutUser()
        {
            var homeController = GetHomeControllerWithUserId(null);
            
            var redirectResult = homeController.DeleteOperation() as RedirectResult;
            
            Assert.IsNotNull(redirectResult);
        }
        
        [Test]
        public void DeleteOperationTestUserHasNotOperation()
        {
            var homeController = GetHomeControllerWithUserId("1");
            
            var httpNotFoundResult = homeController.DeleteOperation(10) as HttpNotFoundResult;

            Assert.IsNotNull(httpNotFoundResult);
        }
        
        [Test]
        public void DeleteOperationTest()
        {
            var homeController = GetHomeControllerWithUserId("1");
            
            var redirectResult = homeController.DeleteOperation(1) as RedirectResult;

            Assert.IsNotNull(redirectResult);
        }
        
        private static HomeController GetHomeControllerWithUserId(string userId)
        {
            var mock = new Mock<HttpContextBase>();
            mock.Setup(p => p.Session["userId"]).Returns(userId);
            return new HomeController(mock.Object);
        }
    }
}