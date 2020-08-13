using MaxisSmartHomeControlSystemMVCDemo.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaxisSmartHomeControlSystemMVCDemo.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
    public class HomeController : Controller
    {
        public string currentGatewayIPAddr = string.Empty;

        //private string getDeviceJsonPathFileName = ExtensionMethods.JsonFileNameLocal;
        private RexliteDBFunctions rexliteDBFunctions = new RexliteDBFunctions();
        private ProcessJsonFunctions processJsonFunctions = new ProcessJsonFunctions();
        private TCPIPClientCallFunctions tcpipClientCallFunctions = new TCPIPClientCallFunctions();

        public ActionResult Index()
        {
            //HomeController process flow
            //Step #1
            //After Login page validated, store user PID and username in session variables
            //Make UDP Call here and get the latest Ble Device List on real time
            //string deviceNewList = tcpipClientCallFunctions.InitMAXBookBroadCastCall();

            //=====================================================================================
            ////This section will check the latest ble device retrieved and save the data into DeviceTmp table

            ////// Truncate the table DeviceTmp and insert the json data content of the newly stored json file
            //int getClearDeviceTmpTableStatus = rexliteDBFunctions.TruncateDeviceTmpTableData();

            ////// Run store procedure to check if there is any difference between tables Device and eviceTmp
            ////// getClearDeviceTmpTableStatus should return -1 if truncating DeviceTmp Table sql query executed successfully
            //if (getClearDeviceTmpTableStatus == -1)
            //{
            //    //call function for getting local json content and insert into DeviceTmp Table
            //    string getUpdateDeviceTmpSt = processJsonFunctions.AddLocalDeviceJsonDataIntoDB("DeviceTmp");
            //}
            //int CheckIfDeviceNeedToUpdateCnt = rexliteDBFunctions.CheckIfDeviceDataNeedToUpdate();
            //if (CheckIfDeviceNeedToUpdateCnt > 0)
            //{
            //    int GetDeviceUpdateCnt = rexliteDBFunctions.UpdateDeviceBleListData();
            //    if (GetDeviceUpdateCnt == 0)
            //    {
            //        string msg = "Device table has been updated successfully!!";
            //    }
            //}
            //====================================================================================
            string deviceNewJson = "";

            if (string.IsNullOrEmpty(deviceNewJson))
            {
                string getDeviceJsonPathFileName = ExtensionMethods.JsonFileNameLocal;
                string localJsonContent = processJsonFunctions.getLocalJsonContent(getDeviceJsonPathFileName);
                ViewBag.DeviceJsonList = localJsonContent;
                Session["DeviceJsonList"] = localJsonContent;
            }
            else
            {
                ViewBag.DeviceJsonList = deviceNewJson;
            }


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
    }
}