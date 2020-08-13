using MaxisSmartHomeControlSystemMVCDemo.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MaxisSmartHomeControlSystemMVCDemo.Controllers
{
    public class MAXLite1DeviceKeyPanelController : Controller
    {
        private RexliteDBFunctions rexliteDBFunctions = new RexliteDBFunctions();
        private ProcessJsonFunctions processJsonFunction = new ProcessJsonFunctions();
        private TCPIPClientCallFunctions tcpipClientCallFunctions = new TCPIPClientCallFunctions();
        string getTCPIPCallCondition = "";
        string getTCPIPCallConditionDataStr = "";

        // GET: MAXLite1DeviceKeyPanel
        public ActionResult Index()
        {
            return View();
        }

        //this will set the selected MAXLite1 device to turn on/off key 1 or key 2
        [HttpPost]
        public ActionResult SelectedMAXLite1DeviceKey(string BleDeviceCmdStr, string BleID, string BleSN, string KeySelected)
        {
            //======================================================================================
            //starting format the correct command to be send via tcpip client call
            StringBuilder sb = new StringBuilder();

            getTCPIPCallCondition = tcpipClientCallFunctions.getTCPIPCallCondition("getMAXLiteDeviceData");  //08

            if (!String.IsNullOrEmpty(KeySelected))
            {
                var requestDataStr = "getMAXLiteDeviceDataKey" + KeySelected;
                getTCPIPCallConditionDataStr = tcpipClientCallFunctions.getTCPIPCallCondition(requestDataStr);  //0801, key #1 for example
            }
            else
            {
                getTCPIPCallConditionDataStr = tcpipClientCallFunctions.getTCPIPCallCondition("getMAXLiteDeviceData");  //08
            }
            string getBleDeviceData = tcpipClientCallFunctions.getBleDeviceData(getTCPIPCallConditionDataStr);  //1905AA03
            string getCallCmd = tcpipClientCallFunctions.getClientCallCommand(getTCPIPCallCondition);  //6
            if (getCallCmd.Length == 1)
            {
                getCallCmd = "0" + getCallCmd;
            }

            // 14060000000000891905AA03
            Byte[] getBytesPre = ExtensionMethods.GetBytes(getCallCmd); //改成byte
            if (getCallCmd.Length < 30)
            {
                var str1 = tcpipClientCallFunctions.GetCRC16ByPoly(getBytesPre);
                var strCRC = tcpipClientCallFunctions.ToHexString(str1);
                getCallCmd = getCallCmd + strCRC;
            }

            var strTestKey1 = "14060000000000891905A200009842";      //(89) 一切第一鍵
            //////ID = 14 , SN = 000000000089
            //////strlight = "18060000000000161905AA03";
            sb.Append(BleID).Append(getCallCmd).Append(BleSN).Append(getBleDeviceData);
            string MAXLite1DeviceCmdToSend = sb.ToString();
            Response.Headers["MAXSceneDeviceCmdToSend"] = MAXLite1DeviceCmdToSend;
            Session["MAXSceneDeviceCmdToSend"] = MAXLite1DeviceCmdToSend;
            int len = MAXLite1DeviceCmdToSend.Length;
            int len2 = strTestKey1.Length;

            //======================================================================================
            string currentGatewayIPAddr = ExtensionMethods.InternalIP;
            string outMsg = "";
            try
            {
                //bool out1 = tcpipClientCallFunctions.SendTCPIPClientCallCmd(currentGatewayIPAddr, MAXLite1DeviceCmdToSend);
                bool out1 = tcpipClientCallFunctions.SendTCPIPClientCallCmd(currentGatewayIPAddr, BleDeviceCmdStr);
                outMsg = "";
            }
            catch (Exception ex)
            {
                outMsg = ex.Message;
            }
            return View();
        }

        [HttpGet]
        public ActionResult FormatMAXLite1DeviceKeyOptionCallCmd(string BleDeviceCmdStr, string BleID, string BleSN, string BarSelected, string OptionSelection)
        {    //      14060000000000891905AA03    getBleDeviceData("08CH1BT")
            StringBuilder sb = new StringBuilder();
            string getBleDeviceData = "";
            getTCPIPCallCondition = tcpipClientCallFunctions.getTCPIPCallCondition("getMAXLiteDeviceData");  //08

            if (!String.IsNullOrEmpty(BarSelected) && !String.IsNullOrEmpty(OptionSelection))
            {   //"getMAXLiteDeviceDataCh1BT"     
                var requestDataStr = "getMAXLiteDeviceData" + "Ch" + BarSelected + OptionSelection; // "getMAXLiteDeviceDataCh1BT"
                getTCPIPCallConditionDataStr = tcpipClientCallFunctions.getTCPIPCallCondition(requestDataStr);  //08CH1BT, "1905AA03", key #1 for example
                // getBleDeviceData("08CH1BT")  ==>  "1905AA03"
                getBleDeviceData = tcpipClientCallFunctions.getBleDeviceData(getTCPIPCallConditionDataStr);  //1905AA03
            }
            //else
            //{
            //    getTCPIPCallConditionDataStr = tcpipClientCallFunctions.getTCPIPCallCondition("getMAXLiteDeviceData");  //08
            //} 14060000000000891905AA03

            string getCallCmd = tcpipClientCallFunctions.getClientCallCommand(getTCPIPCallCondition);  //6
            if (getCallCmd.Length == 1)
            {
                getCallCmd = "0" + getCallCmd;
            }

            sb.Append(BleID).Append(getCallCmd).Append(BleSN).Append(getBleDeviceData);
            var result = sb.ToString();    //14060000000000891905AA03
            Session["formatMaxlite1OptionCallCmd"] = result;
            Session["CurrentSliderOptionSelected"] = OptionSelection;
            return Json(result, JsonRequestBehavior.AllowGet);
            //return View();
        }

        [HttpPost]
        public ActionResult MakeSelectedMAXLite1DeviceKeyOptionCall(string BleDeviceCmdStr, string SelectedBar, string OptionSelection)
        {
            //Slider #1 selected  --- var strlightBT = "14060000000000891905AA03";
            //      14060000000000891905AA03 
            //1 % --14060000000000891905AA0301  str = strlight+ tmp;
            //1 % --14060000000000891905AA0301D8B0  str1 = str+ToHexString(GetCRC16ByPoly(GetBytes(str)));
            //2 % --14060000000000891905AA030298B1
            //3 % --14060000000000891905AA03035971
            //4 % --14060000000000891905AA030418B3
            //5 % --14060000000000891905AA0305D973     str1 = str+ToHexString(GetCRC16ByPoly(GetBytes(str)));
            //6 % --14060000000000891905AA03069972     str1 = str+ToHexString(GetCRC16ByPoly(GetBytes(str)));
            //7 % --14060000000000891905AA030758B2     str1 = str+ToHexString(GetCRC16ByPoly(GetBytes(str)));
            //8 % --14060000000000891905AA030818B6    str1 = str+ToHexString(GetCRC16ByPoly(GetBytes(str)));
            //5string getCurrentMaxlite1OptionCmd = Session["formatMaxlite1OptionCallCmd"].ToString();
            string tempStr = BleDeviceCmdStr.ToString();

            string result = "";

            //string tmpVal = Convert.ToString(BTValue, 16);
            //if (tmpVal.Length == 1)
            //{
            //    tmpVal = "0" + tmpVal;
            //}

            byte[] getCmdArrInBytes = ExtensionMethods.GetBytes(BleDeviceCmdStr);
            byte[] GetCRCPoly = tcpipClientCallFunctions.GetCRC16ByPoly(getCmdArrInBytes);

            string CRCStr = tcpipClientCallFunctions.ByteToHexString(GetCRCPoly);
            string FormatFullMakeMAXLite1BTCmd = BleDeviceCmdStr + CRCStr;
            //======================================================================================
            string currentGatewayIPAddr = ExtensionMethods.InternalIP;
            string outMsg = "";
            try
            {
                bool out1 = tcpipClientCallFunctions.SendTCPIPClientCallCmd(currentGatewayIPAddr, FormatFullMakeMAXLite1BTCmd);
                //bool outSt = tcpipClientCallFunctions.SendTCPIPOptionClientCallCmd(currentGatewayIPAddr, FormatFullMakeMAXLite1BTCmd);

                outMsg = "";
            }
            catch (Exception ex)
            {
                outMsg = ex.Message;
            }
            Session["LastSavedSliderOptionCmd"] = FormatFullMakeMAXLite1BTCmd;

            return View("Index");
        }

        [HttpPost]
        public ActionResult SelectedMAXLite1Device(string BleID, string BleSN)
        {
            Session["MAXLite1SelectedBleID"] = BleID;
            Session["MAXLite1SelectedBleSN"] = BleSN;
            ViewBag.MAXLite1SelectedBleID = BleID;
            ViewBag.MAXLite1SelectedBleSN = BleSN;

            //return RedirectToAction("Index", "MAXLite1DeviceKeyPanel");
            return View("Index");
        }

    }
}