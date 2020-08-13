using MaxisSmartHomeControlSystemMVCDemo.Helper;
using MaxisSmartHomeControlSystemMVCDemo.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaxisSmartHomeControlSystemMVCDemo.Controllers
{
    public class SignUpController : Controller
    {
        // GET: SignUp
        private readonly RexliteDBEntities rexliteDBEntities = new RexliteDBEntities();
        private RexliteDBFunctions rexliteDBFunctions = new RexliteDBFunctions();

        // GET: SignUp
        public ActionResult Index()
        {
            ///This section is for getting user role data from database
            List<GetUserRole_Result> userRoleList = rexliteDBFunctions.GetUserRole();

            ////ViewBag.UserRoleJsonList = JsonConvert.SerializeObject(userRoleList);
            ViewBag.UserRoleJsonList = userRoleList;

            return View();
        }

        [HttpPost]
        public ActionResult CreateAccount(Models.UserLogin userModel, string UserRoleSelect)
        {
            string result = "";
            bool createAcctOK = false;
            bool userEmailExists = false;
            bool st = ModelState.IsValid;

            ///This section is for getting user role data from database
            List<GetUserRole_Result> userRoleList = rexliteDBFunctions.GetUserRole();

            ////ViewBag.UserRoleJsonList = JsonConvert.SerializeObject(userRoleList);
            ViewBag.UserRoleJsonList = userRoleList;

            bool hasErrors = ViewData.ModelState.Values.Any(x => x.Errors.Count > 1);

            //foreach (ModelState state in ViewData.ModelState.Values.Where(x => x.Errors.Count > 0))
            //{
            //    Console.Write(state.Errors);
            //}

            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();

            if (hasErrors)
            {
                return View(userModel);
            }
            else
            {
                List<GetUserByUserEmail_Result> checkUserData = rexliteDBFunctions.GetUserDataByUserEmail(userModel.UserEmailAddress);//userModel.UserName, userModel.UserPassword
                //int getExistingUserID = Convert.ToInt32(rexliteDBEntities.GetUserIDByUserNameAndUserPwd(userModel.UserName, userModel.UserPassword));
                //var getExistingUserID = rexliteDBEntities.GetUserIDByUserNameAndUserPwd(userModel.UserName, userModel.UserPassword);

                if (checkUserData.Any())
                {
                    result = "The user name and password entered already existed! => " + userModel.UserName + " - " + userModel.UserPassword;
                    userEmailExists = true;
                }
                else
                {
                    using (Models.RexliteDBEntities db = new Models.RexliteDBEntities())
                    using (RexliteDBEntities db1 = new RexliteDBEntities())
                    {
                        try
                        {
                            UserLogin userTable = new UserLogin();

                            userTable.UserName = userModel.UserName;
                            userTable.UserPassword = userModel.UserPassword;
                            //userTable.ConfirmPassword = userModel.UserPassword;
                            userTable.UserEmailAddress = userModel.UserEmailAddress;
                            userTable.UserMobilePhoneNumber = userModel.UserMobilePhoneNumber;
                            //userTable.UserRoleAssigned = Convert.ToInt32(UserRoleSelect);
                            userTable.CreateDate = DateTime.Now;

                            db1.UserLogins.Add(userTable);
                            db1.SaveChanges();
                            createAcctOK = true;
                        }
                        catch (DbEntityValidationException ex)
                        {
                            var entityError = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                            var getFullMessage = string.Join("; ", entityError);
                            result = string.Concat(ex.Message, "errors are: ", getFullMessage);
                            //LogException(new Exception(string.Format("File : {0} {1}.", logFile.FullName, exceptionMessage), ex));
                        }
                        catch (SqlException exc)
                        {
                            //here you might still get some exceptions but not about validation.
                            //ExceptionManager.Log(exc);
                            //sometimes you may want to throw the exception to upper layers for handle it better over there!
                            //throw;
                            result = "Error number: " + exc.Number + " - message =" + exc.Message + " - InnerException =" + exc.InnerException;
                        }
                    }

                }

                ////updateStatusOK = true;
                //if (createAcctOK)
                //{
                //    ViewBag.CreateNewAcctMsg = "New User Account: " + userModel.UserName + " has been created successfully!";
                //    //ViewData["CreateNewAcctMsg2"] = "New User Account: " + userModel.UserName + " has been created successfully!";
                //    //return RedirectToAction("Success"); //Should return 1 if successful since only one row should be affected
                //    //return RedirectToAction("Index", "SignUp");
                //    return View("Index");
                //}
                //else
                //{
                //    ViewBag.CreateNewAcctMsg = "New User Account has failed to be created - Error Message:" + result;
                //    return View("Index");
                //}

            }
            return View("Index");
        }

    }
}