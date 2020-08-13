using MaxisSmartHomeControlSystemMVCDemo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MaxisSmartHomeControlSystemMVCDemo.Helper
{
    public class ProcessJsonFunctions
    {

        public string getBleDevice = "";
        public int i = 0;
        public RexliteDBFunctions rexliteDBFunctions = new RexliteDBFunctions();
        public string pathToFiles = HttpContext.Current.Server.MapPath("~/Helper/");
        //string pathToFiles = HttpContext.Current.Server.MapPath("Helper/blelist.json");
        private string getDeviceJsonPathFileName = ExtensionMethods.JsonFileNameLocal;
        private readonly RexliteDBEntities rexliteDBEntities = new RexliteDBEntities();

        public string createLocalJsonFiles(string jsonFileName)
        {
            string result = "";
            try
            {
                //This will locate the json file in the Helper folder.
                //string BleJsonListFilePath = HttpContext.Current.Server.MapPath("~/Helper/");
                //string jsonLocalFile = BleJsonListFilePath + jsonFileName;
                //result = File.ReadAllText(jsonLocalFile);
            }
            catch (Exception ex)
            {
                result = ex.InnerException + ex.Message;
            }
            return result;
        }


        public string getLocalJsonContent(string jsonFileName)
        {
            string result = "";
            try
            {
                //This will locate the json file in the Helper folder.
                string BleJsonListFilePath = HttpContext.Current.Server.MapPath("~/Helper/");
                string jsonLocalFile = BleJsonListFilePath + jsonFileName;
                result = File.ReadAllText(jsonLocalFile);
            }
            catch (Exception ex)
            {
                result = ex.InnerException + ex.Message;
            }
            return result;
        }

        public string ConvertDeviceStrArrayToJson(string[] deviceList)
        {
            string json = "";
            string result = "";
            StringBuilder sb = new StringBuilder();
            try
            {
                for (int i = 0; i < deviceList.Length; i++)
                {
                    string id = deviceList[i].Substring(0, 2);
                    string sn = deviceList[i].Substring(2, 12);
                    string name = GetDeviceType(id);

                    var device = new
                    {
                        BleID = id,
                        BleSN = sn,
                        BleName = name
                    };
                    json = JsonConvert.SerializeObject(device);
                    sb.Append(json.ToString() + ",");
                }
                result = sb.ToString();
                result = "[" + result.TrimEnd(',') + "]";
            }
            catch (Exception ex)
            {
                result = ex.InnerException.ToString();
            }
            return result;
        }

        //This section will successfully insert json data into device table
        public string AddLocalDeviceJsonDataIntoDB(string deviceTableName)
        {
            string result = "";
            try
            {
                string deviceJsonfile = pathToFiles + getDeviceJsonPathFileName;
                bool isFileExists = ExtensionMethods.CheckJsonFile(deviceJsonfile);

                if (isFileExists)
                {
                    using (StreamReader sr = new StreamReader(deviceJsonfile))
                    {
                        getBleDevice = sr.ReadToEnd();
                        List<BleJsonDevice> bleDeviceList = JsonConvert.DeserializeObject<List<BleJsonDevice>>(getBleDevice);
                        if (deviceTableName == "Device")
                        {
                            Device deviceTable = new Device();
                            for (i = 0; i < bleDeviceList.Count; i++)
                            {
                                deviceTable.DeviceID = bleDeviceList[i].BleID;
                                deviceTable.DeviceSN = bleDeviceList[i].BleSN;
                                deviceTable.DeviceDefaultName = bleDeviceList[i].BleName;
                                deviceTable.CreateDate = DateTime.Now;

                                rexliteDBEntities.Devices.Add(deviceTable);
                                rexliteDBEntities.SaveChanges();
                            }
                        }
                        if (deviceTableName == "DeviceTmp")
                        {
                            DeviceTmp deviceTmpTable = new DeviceTmp();

                            for (i = 0; i < bleDeviceList.Count; i++)
                            {
                                deviceTmpTable.DeviceID = bleDeviceList[i].BleID;
                                deviceTmpTable.DeviceSN = bleDeviceList[i].BleSN;
                                deviceTmpTable.DeviceDefaultName = bleDeviceList[i].BleName;
                                deviceTmpTable.CreateDate = DateTime.Now;

                                rexliteDBEntities.DeviceTmps.Add(deviceTmpTable);
                                rexliteDBEntities.SaveChanges();
                            }
                        }

                    }
                    result = "Device Json has been written to DB successfully!";
                }
                else
                {
                    result = "Device Json Local File not found!";
                }

            }
            catch (DbUpdateException ex)
            {
                throw ex.InnerException;
                SqlException s = ex.InnerException.InnerException as SqlException;
                if (s != null && s.Number == 2627)
                {
                    //ModelState.AddModelError(string.Empty,
                    //   string.Format("Part number '{0}' already exists.", part.Number));
                }
                else
                {
                    //ModelState.AddModelError(string.Empty,
                    //    "An error occured - please contact your system administrator.");
                }
            }
            return result;
        }

        public bool SaveJsonLocalFile(string listJson, string jsonFileName)
        {
            bool fileSaved = false;
            string jsonFile = string.Empty;
            try
            {
                //This will locate the Example json file in the App_Data folder.. (App_Data is a good place to put data files.)
                string BleJsonListFilePath = HttpContext.Current.Server.MapPath("~/Helper/");
                string getSaveFileName = string.Empty;

                string FileToBeSaved = BleJsonListFilePath + jsonFileName;
                if (File.Exists(FileToBeSaved))
                {
                    File.Delete(@FileToBeSaved);  //Response.Write(AirBleFile + " 檔案存在");
                    File.WriteAllText(FileToBeSaved, listJson, Encoding.UTF8);
                    fileSaved = ExtensionMethods.CheckJsonFile(jsonFileName);
                }
                else
                {   //Response.Write(BleJsonFileName + " 檔案不存在");
                    File.WriteAllText(FileToBeSaved, listJson, Encoding.UTF8);
                    fileSaved = ExtensionMethods.CheckJsonFile(jsonFileName);
                }
            }
            catch (IOException ex)
            {
                //MessageBox.Show(pushUp.StrIOExcepion);
            }
            catch (ObjectDisposedException ex)
            {
                //MessageBox.Show(pushUp.StrObjectDisposedException);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(pushUp.StrException);
            }
            return fileSaved;
        }

        public string GetDeviceType(string device)
        {
            string dtype = "";
            switch (device)
            {
                case "0a":
                case "0A":
                    dtype = "MAXBook";              //接線盒
                    break;
                case "0b":
                case "0B":
                    dtype = "MAXScene";             //情境開關
                    break;
                case "13":
                    dtype = "MAXAir";               //空調開關
                    break;
                case "14":
                    dtype = "MAXLite M’L - 1";      //觸控調光開關單迴路 
                    break;
                case "16":
                    dtype = "MAXLite M’L - 2";      //觸控調光開關雙迴路
                    break;
                case "18":
                    dtype = "MAXLite M’L - 3";      //觸控調光開關三迴路
                    break;
            }
            return dtype;
        }

        public string GetDeviceID(string deviceName)
        {
            string deviceID = "";
            switch (deviceName)
            {
                case "MAXBook":
                    deviceID = "0a";            //接線盒
                    break;
                case "MAXScene":
                    deviceID = "0b";            //情境開關
                    break;
                case "MAXAir":
                    deviceID = "13";            //空調開關
                    break;
                case "MAXLite1":
                case "MAXLite M’L - 1":
                    deviceID = "14";            //觸控調光開關單迴路 
                    break;
                case "MAXLite2":
                case "MAXLite M’L - 2":
                    deviceID = "16";            //觸控調光開關雙迴路
                    break;
                case "MAXLite3":
                case "MAXLite M’L - 3":
                    deviceID = "18";            //觸控調光開關三迴路
                    break;
            }
            return deviceID;
        }



    }
}