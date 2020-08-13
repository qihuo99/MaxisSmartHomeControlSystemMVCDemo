using MaxisSmartHomeControlSystemMVCDemo.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaxisSmartHomeControlSystemMVCDemo.Controllers
{
    public class MAXSceneDeviceKeyPanelController : Controller
    {
        // GET: MAXSceneDeviceKeyPanel
        private RexliteDBFunctions rexliteDBFunctions = new RexliteDBFunctions();
        private ProcessJsonFunctions processJsonFunction = new ProcessJsonFunctions();
        private TCPIPClientCallFunctions tcpipClientCallFunctions = new TCPIPClientCallFunctions();

        // GET: MAXSceneDeviceKeyPanel
        public ActionResult Index()
        {
            return View();
        }

        //this will set the selected MAXScene device to turn on/off key 1 or key 2
        [HttpPost]
        public ActionResult SelectedMAXSceneDeviceKey(string BleDeviceCmdStr, string BleID, string BleSN)
        {
            string currentGatewayIPAddr = ExtensionMethods.InternalIP;
            string outMsg = "";
            try
            {
                bool out1 = tcpipClientCallFunctions.SendTCPIPClientCallCmd(currentGatewayIPAddr, BleDeviceCmdStr);
                outMsg = "";
            }
            catch (Exception ex)
            {
                outMsg = ex.Message;
            }
            return View();
        }

        [HttpPost]
        public ActionResult SelectedMAXSceneDevice(string BleID, string BleSN)
        {
            Session["MAXSceneSelectedBleID"] = BleID;
            Session["MAXSceneSelectedBleSN"] = BleSN;
            ViewBag.MAXSceneSelectedBleID = BleID;
            ViewBag.MAXSceneSelectedBleSN = BleSN;

            return View("Index");
        }

    }
}