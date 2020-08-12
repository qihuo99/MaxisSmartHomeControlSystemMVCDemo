using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MaxisSmartHomeControlSystemMVCDemo.Controllers
{
    public class LoginController : Controller
    {

        public bool IsNetworkConnected = false;
        //private TCPIPClientCallFunctions tcpipClientCallFunctions = new TCPIPClientCallFunctions();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(Models.UserLogin userModel)
        {
            using (Models.RexliteDBEntities db = new Models.RexliteDBEntities())
            {
                var userDetail = db.UserLogins.Where(x => x.UserName == userModel.UserName && x.UserPassword == userModel.UserPassword).FirstOrDefault();

                if (userDetail == null)
                {
                    userModel.LoginErrorMessage = "Invalid user name or password";
                    return View("Index", userModel);
                }
                else
                {
                    //Temporary comment out the network connection section
                    //IsNetworkConnected = tcpipClientCallFunctions.IsNetworkConnected;
                    //if (!IsNetworkConnected)
                    //{
                    //    tcpipClientCallFunctions.ConnectToNetwork(ExtensionMethods.InternalIP);
                    //}

                    Session["userID"] = userDetail.ID;
                    Session["userName"] = userDetail.UserName;
                    Session["userPwd"] = userDetail.UserPassword;
                    return RedirectToAction("Index", "Home");
                }
            }
            //return View();
        }

        public ActionResult LogOut()
        {
            //if (User.Identity.IsAuthenticated)
            //{
            TempData["LogoutMessage"] = "You are now successful loged out.";
            var userId = Session["userID"];
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            //}

            return RedirectToAction("Index", "Login");
        }

    }
}