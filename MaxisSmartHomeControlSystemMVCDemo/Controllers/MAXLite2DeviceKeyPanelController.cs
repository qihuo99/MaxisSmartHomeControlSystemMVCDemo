using MaxisSmartHomeControlSystemMVCDemo.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaxisSmartHomeControlSystemMVCDemo.Controllers
{
    public class MAXLite2DeviceKeyPanelController : Controller
    {
        private RexliteDBFunctions rexliteDBFunctions = new RexliteDBFunctions();
        private ProcessJsonFunctions processJsonFunction = new ProcessJsonFunctions();
        private TCPIPClientCallFunctions tcpipClientCallFunctions = new TCPIPClientCallFunctions();

        // GET: MAXLite2DeviceKeyPanel
        public ActionResult Index()
        {
            return View();
        }

        //this will set the selected MAXLite2 device to turn on key 1 ~ key 4
        [HttpPost]
        public ActionResult SelectedMAXLite2DeviceKey(string BleDeviceCmdStr)
        {
            string currentGatewayIPAddr = ExtensionMethods.InternalIP;
            string outMsg = "";
            try
            {
                bool outStatus = tcpipClientCallFunctions.SendTCPIPClientCallCmd(currentGatewayIPAddr, BleDeviceCmdStr);

                outMsg = "";
            }
            catch (Exception ex)
            {
                outMsg = ex.Message;
            }

            return View();
        }

        [HttpPost]
        public ActionResult SelectedMAXLite2Device(string BleID, string BleSN)
        {
            Session["MAXLite2SelectedBleID"] = BleID;
            Session["MAXLite2SelectedBleSN"] = BleSN;
            ViewBag.MAXLite2SelectedBleID = BleID;
            ViewBag.MAXLite2SelectedBleSN = BleSN;

            //return RedirectToAction("Index", "MAXLite1DeviceKeyPanel");
            return View("Index");
        }

    }
}