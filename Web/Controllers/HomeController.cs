using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using Dapper;
using Microsoft.AspNet.Identity;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }




        public ActionResult BookSearch()
        {
            ViewBag.Message = "Here we search books";
            ViewBag.AvailableBooks = BookController.AvailableBooks();
            var userId = User.Identity.GetUserId(); 
            ViewBag.UnavailableBooks = BookController.UserBorrowedBooks(userId);
            return View();
        }

        
    }
}