using MaxisSmartHomeControlSystemMVCDemo.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaxisSmartHomeControlSystemMVCDemo.Controllers
{
    public class MAXBookDeviceListController : Controller
    {
        // GET: MAXBookDeviceList
        private RexliteDBFunctions rexliteDBFunctions = new RexliteDBFunctions();
        private ProcessJsonFunctions processJsonFunctions = new ProcessJsonFunctions();
        private TCPIPClientCallFunctions tcpipClientCallFunctions = new TCPIPClientCallFunctions();

        // GET: MAXBookDeviceList
        public ActionResult Index()
        {

            if (!string.IsNullOrEmpty(Session["DeviceJsonList"] as string))
            {
                ViewBag.DeviceJsonList = Session["DeviceJsonList"].ToString();
            }
            else
            {
                string getDeviceJsonPathFileName = ExtensionMethods.JsonFileNameLocal;
                string localJsonContent = processJsonFunctions.getLocalJsonContent(getDeviceJsonPathFileName);
                ViewBag.DeviceJsonList = localJsonContent;
                Session["DeviceJsonList"] = localJsonContent;
            }

            ViewBag.MAXBookBleId = tcpipClientCallFunctions.getBleDeviceID("MAXBook");

            return View();
        }

    }
}