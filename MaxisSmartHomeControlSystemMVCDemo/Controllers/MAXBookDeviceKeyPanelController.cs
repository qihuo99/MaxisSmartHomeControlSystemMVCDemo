using MaxisSmartHomeControlSystemMVCDemo.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaxisSmartHomeControlSystemMVCDemo.Controllers
{
    public class MAXBookDeviceKeyPanelController : Controller
    {
        // GET: MAXBookDeviceKeyPanel
        private RexliteDBFunctions rexliteDBFunctions = new RexliteDBFunctions();
        private ProcessJsonFunctions processJsonFunction = new ProcessJsonFunctions();
        private TCPIPClientCallFunctions tcpipClientCallFunctions = new TCPIPClientCallFunctions();
        string getTCPIPCallCondition = "";
        string getTCPIPCallConditionDataStr = "";

        // GET: MAXBookDeviceKeyPanel
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SelectedMAXBookDevice(string BleID, string BleSN)
        {
            Session["MAXBookSelectedBleID"] = BleID;
            Session["MAXBookSelectedBleSN"] = BleSN;
            ViewBag.MAXBookSelectedBleID = BleID;
            ViewBag.MAXBookSelectedBleSN = BleSN;

            return View("Index");
        }

    }
}