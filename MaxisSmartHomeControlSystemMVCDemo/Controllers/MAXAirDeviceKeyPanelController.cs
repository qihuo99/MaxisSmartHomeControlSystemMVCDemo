using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaxisSmartHomeControlSystemMVCDemo.Controllers
{
    public class MAXAirDeviceKeyPanelController : Controller
    {
        // GET: MAXAirDeviceKeyPanel
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SelectedMAXAirDevice(string BleID, string BleSN)
        {
            Session["MAXAirSelectedBleID"] = BleID;
            Session["MAXAirSelectedBleSN"] = BleSN;
            ViewBag.MAXAirSelectedBleID = BleID;
            ViewBag.MAXAirSelectedBleSN = BleSN;

            return View("Index");
        }

    }
}