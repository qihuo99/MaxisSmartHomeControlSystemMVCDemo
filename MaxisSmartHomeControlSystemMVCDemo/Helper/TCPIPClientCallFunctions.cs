using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;

namespace MaxisSmartHomeControlSystemMVCDemo.Helper
{
    public class TCPIPClientCallFunctions
    {

        int i = 0, j = 0;
        private ProcessJsonFunctions processJsonFunctions = new ProcessJsonFunctions();
        private RexliteDBFunctions rexliteDBFunctions = new RexliteDBFunctions();
        public TcpClient myTcpClient = new TcpClient();
        public NetworkStream myNetworkStream;
        public bool IsNetworkConnected = false;

        public string InitMAXBookBroadCastCall()
        {
            //Below functions are working correct! Only under situation that internal ip address is fixed and known already
            string initMaxBookBroadCastStr = ExtensionMethods.BroadcastStr;
            string[] deviceArr = MakeTCPIPBroadcastClientCall(initMaxBookBroadCastStr);

            //// Process the retrieved the Ble Device List via the above TCPIP Client Call into json
            string getJsonOutput = processJsonFunctions.ConvertDeviceStrArrayToJson(deviceArr);

            //// Store the json into a file in a local folder (/Helper) and also in Session variables and Viewbag var
            bool saveLocalJsonFile = processJsonFunctions.SaveJsonLocalFile(getJsonOutput, ExtensionMethods.JsonFileNameLocal);

            return getJsonOutput;  //should return this for storing in session variables and local json file content
        }

        public bool ConnectToNetwork(string currGatewayIPAddr)
        {
            bool connectionStatus = false;
            string result = "";
            int PortNum = ExtensionMethods.PortNumber;

            try
            {
                //測試連線至遠端主機 
                myTcpClient = new TcpClient();
                myTcpClient.Connect(currGatewayIPAddr, PortNum);
                //myNetworkClient.myTcpClient.Connect(ipname, portname);
                //Console.WriteLine("連線成功 !!\n");
                connectionStatus = true;
                IsNetworkConnected = true;
            }
            catch (IOException ex)
            {
                //Console.WriteLine("主機 {0} 通訊埠 {1} 無法連接  !!", ipname, portname);
                result = ex.InnerException.Message;
                connectionStatus = false;
                IsNetworkConnected = false;
            }
            return connectionStatus;
        }

        public bool SendTCPIPOptionClientCallCmd(string currGatewayIPAddr, string cmd)
        {
            bool connectionStatus = false;

            try
            {
                //if (!IsNetworkConnected)
                //{
                //    ConnectToNetwork(currGatewayIPAddr);
                //}

                //var strTestKey1 = "14060000000000891905A200009842";   

                Byte[] getBytes = ExtensionMethods.GetBytes(cmd); //改成byte

                //Start making network stream 建立網路資料流
                myNetworkStream = myTcpClient.GetStream();
                //myNetworkStream.WriteTimeout = 1000;
                Thread.Sleep(300);
                myNetworkStream.Write(getBytes, 0, getBytes.Length);

                ////讀取字串的長度[18]
                //int bufferSize = myTcpClient.ReceiveBufferSize;
                //byte[] myBufferBytes = new byte[1500];
                ////讀取字串的長度[18]
                //Thread.Sleep(300);
                //int read_byte_len = 0;
                //try
                //{
                //    read_byte_len = myNetworkStream.Read(myBufferBytes, 0, 1500);
                //}
                //catch (Exception e) { }
                ////myNetworkStream.Flush();
                //int bit_cal = read_byte_len * 8;
                //myNetworkStream.ReadTimeout = 1000;
                //String cal_str = "總共收到" + bit_cal + "bit\n";
                connectionStatus = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                connectionStatus = false;
            }
            return connectionStatus;
        }

        public bool SendTCPIPClientCallCmd(string currGatewayIPAddr, string cmd)
        {
            bool connectionStatus = false;

            try
            {
                if (!IsNetworkConnected)
                {
                    ConnectToNetwork(currGatewayIPAddr);
                }

                //var strTestKey1 = "14060000000000891905A200009842";   

                Byte[] getBytes = ExtensionMethods.GetBytes(cmd); //改成byte

                //Start making network stream 建立網路資料流
                myNetworkStream = myTcpClient.GetStream();
                //myNetworkStream.WriteTimeout = 1000;
                Thread.Sleep(500); //
                myNetworkStream.Write(getBytes, 0, getBytes.Length);

                ////讀取字串的長度[18]
                //int bufferSize = myTcpClient.ReceiveBufferSize;              
                //byte[] myBufferBytes = new byte[1500];
                ////讀取字串的長度[18]
                //Thread.Sleep(300);
                //int read_byte_len = 0;
                //try
                //{
                //    read_byte_len = myNetworkStream.Read(myBufferBytes, 0, 1500);
                //}
                //catch (Exception e) { }
                ////myNetworkStream.Flush();
                ////int bit_cal = read_byte_len * 8;
                ////myNetworkStream.ReadTimeout = 1000;
                ////String cal_str = "總共收到" + bit_cal + "bit\n";
                connectionStatus = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                connectionStatus = false;
            }
            return connectionStatus;
        }

        public bool SendTCPIPClientCallCmdForMaxSceneOnly(string currGatewayIPAddr, string cmd)
        {
            bool connectionStatus = false;

            try
            {
                if (!IsNetworkConnected)
                {
                    ConnectToNetwork(currGatewayIPAddr);
                }

                //var strTestKey1 = "14060000000000891905A200009842";   
                Byte[] getBytes = ExtensionMethods.GetBytes(cmd); //改成byte

                //Start making network stream 建立網路資料流
                myNetworkStream = myTcpClient.GetStream();
                //myNetworkStream.WriteTimeout = 1000;
                Thread.Sleep(500); //
                myNetworkStream.Write(getBytes, 0, getBytes.Length);
                connectionStatus = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                connectionStatus = false;
            }
            return connectionStatus;
        }


        //CRC for Rexlite -- CRC method created by Jack
        public byte[] GetCRC16ByPoly(byte[] Cmd)
        {
            byte[] CRC = new byte[2];
            ushort CRCValue = 0xFFFF;
            for (int i = 0; i < Cmd.Length; i++)
            {
                CRCValue = (ushort)(CRCValue ^ Cmd[i]);
                for (int j = 0; j < 8; j++)
                {
                    if ((CRCValue & 0x0001) != 0)
                    {
                        CRCValue = (ushort)((CRCValue >> 1) ^ 0xA001);
                        //poly 運算式 A001
                    }
                    else
                    {
                        CRCValue = (ushort)(CRCValue >> 1);
                    }
                }
            }
            return BitConverter.GetBytes(CRCValue);
        }

        public string[] MakeTCPIPBroadcastClientCall(string initMAXBookStr)
        {
            try
            {
                if (!IsNetworkConnected)
                {
                    ConnectToNetwork(ExtensionMethods.InternalIP);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            Byte[] data = ExtensionMethods.GetBytes(initMAXBookStr); //改成byte
                                                                     //把傳輸的資料轉成unicode並用byte陣列儲存

            //Start making network stream 建立網路資料流 "傳送中...");
            myNetworkStream = myTcpClient.GetStream();
            //myNetworkStream.WriteTimeout = 1000;
            Thread.Sleep(300);
            myNetworkStream.Write(data, 0, data.Length);

            //讀取字串的長度[18] 
            int bufferSize = myTcpClient.ReceiveBufferSize; //Console.Write("接收資料中...");
            byte[] getDeviceCount = new byte[1];
            byte[] BufferBytes2 = new byte[1500];
            //Thread.Sleep(2000);

            myNetworkStream.Read(getDeviceCount, 0, 1);
            string getTotalDevice = ByteToHexString(getDeviceCount);
            int deviceCount = Convert.ToInt32(getTotalDevice);

            string[] output = new string[deviceCount];
            //Start reading the remaining bytes in buffer according to deviceCount
            for (i = 0; i < deviceCount; i++)
            {
                byte[] BufferBytes = new byte[11];
                myNetworkStream.Read(BufferBytes, 0, 11);
                output[i] = ByteToHexString(BufferBytes);
            }
            return output;
        }

        //byte轉string 
        public string ToHexString(byte[] bytes)
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder str = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    str.Append(bytes[i].ToString("X2"));
                }
                hexString = str.ToString();
            }
            return hexString;
        }

        //byte轉string
        public string ByteToHexString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();

            if (bytes != null)
            {
                int len = bytes.Length;
                int deviceLength = 0;

                if (len >= 11)
                {
                    deviceLength = 7;
                }
                else
                {
                    deviceLength = len;
                }

                for (int i = 0; i < deviceLength; i++)
                {
                    sb.Append(bytes[i].ToString("X2"));
                }
            }
            return sb.ToString();
        }


        public string getTCPIPCallCondition(string request)
        {
            string dtype = "";

            switch (request)
            {
                case "getInitMAXBookID":
                    dtype = "00";               //get the first available MAXBook ID (連線哪台設備)
                    break;
                case "requestDataTransmissionActivation":
                    dtype = "01";               //request the device to activate the data transmission function (開通並讀取設備狀態資料)
                    break;
                case "blinkingDeviceLight":
                    dtype = "02";               //request the device to blink the light (閃爍面板)
                    break;
                case "broadcastCommandBluetooth123Device":
                    dtype = "03";       	    //broadcast command by using Bluetooth, 搜尋1.2.3級面板(藍芽用)
                    break;
                case "broadcastCommandBluetooth12OnlyDevice":
                    dtype = "04";       	    //broadcast command by using Bluetooth, 搜尋1.2級面板Only(藍芽用)
                    break;
                case "broadcastCommandEthernet":
                    dtype = "05";               //broadcast command by using Ethernet,  搜尋1.2.3級面板(網路用)
                    break;
                case "getMAXBookData":
                    dtype = "06";               //get MAXBook Device data and status (all or single)
                    break;
                case "getMAXSceneData":
                    dtype = "07";               //get MAXScene Device data and status (all or single)
                    break;
                case "getMAXLiteDeviceData":
                    dtype = "08";               //get MAXLite Device data and status (all or single)
                    break;
                case "getMAXLiteDeviceDataKey1":
                    dtype = "0801";               //get MAXLite Device Key #1 data and status (single)
                    break;
                case "getMAXLiteDeviceDataKey2":
                    dtype = "0802";               //get MAXLite Device Key #2 data and status (single)
                    break;
                case "getMAXLiteDeviceDataKey3":
                    dtype = "0803";               //get MAXLite Device Key #3 data and status (single)
                    break;
                case "getMAXLiteDeviceDataKey4":
                    dtype = "0804";               //get MAXLite Device Key #4 data and status (single)
                    break;
                case "getMAXLiteDeviceDataKey5":
                    dtype = "0805";               //get MAXLite Device Key #5 data and status (single)
                    break;
                case "getMAXLiteDeviceDataKey6":
                    dtype = "0806";               //get MAXLite Device Key #6 data and status (single)
                    break;
                case "getMAXLiteDeviceDataCh1BT":
                    dtype = "08CH1BT";               //get MAXLite Device Key #1 data and status (1.2.3切面板第一迴路調光(YY=0x00~0x33))
                    break;
                case "getMAXLiteDeviceDataCh2BT":
                    dtype = "08CH2BT";               //get MAXLite Device Key #2 data and status (2.3切面板第二迴路調光)
                    break;
                case "getMAXLiteDeviceDataCh3BT":
                    dtype = "08CH3BT";               //get MAXLite Device Key #3 data and status (3切面板第三迴路調光)
                    break;
                case "getMAXLiteDeviceDataCh1CT":
                    dtype = "08CH1CT";               //get MAXLite Device Key #1 data and status (1.2.3切面板第一迴路調色溫(YY=00~0x64))
                    break;
                case "getMAXLiteDeviceDataCh2CT":
                    dtype = "08CH2CT";               //get MAXLite Device Key #2 data and status (2.3切面板第二迴路調色溫)
                    break;
                case "getMAXLiteDeviceDataCh3CT":
                    dtype = "08CH3CT";               //get MAXLite Device Key #3 data and status (3切面板第三迴路調色溫)
                    break;
                case "getMAXAirDeviceData":
                    dtype = "09";               //get MAXAir Device data and status (all or single)
                    break;
            }
            return dtype;
        }

        public string getClientCallCommand(string cmdStr)
        {
            string result = string.Empty;

            switch (cmdStr)
            {
                case "00":
                    result = CallCommandSend.ConnectToMAXBook.ToString("d");                    //連線哪台設備
                    break;
                case "01":
                    result = CallCommandSend.ActivateSendBackFunction.ToString("d");            //開通並讀取設備狀態資料
                    break;
                case "02":
                    result = CallCommandSend.BlinkingPanel.ToString("d");                       //閃爍面板
                    break;
                case "03":
                    result = CallCommandSend.Search_1_2_3_Panel_BlueTooth.ToString("d");        //搜尋1.2.3級面板(藍芽用)
                    break;
                case "04":
                    result = CallCommandSend.Search_1_2_Panel_Only_BlueTooth.ToString("d");     //搜尋1.2級面板(藍芽用)
                    break;
                case "05":
                    result = CallCommandSend.Search_1_2_3_Panel_Ethernet.ToString("d");         //搜尋1.2.3級面板(網路用)
                    break;
                case "07":
                    result = CallCommandSend.MAXScene.ToString("d");                            //情境面板
                    break;
                case "08":
                    result = CallCommandSend.MAXLite.ToString("d");                             //切面板
                    break;
            }
            return result;
        }

        public string getBleDeviceData(string request)
        {
            string dtype = string.Empty;

            switch (request)
            {
                case "00":
                    dtype = "19820008FE";       //連線哪台設備
                    break;
                case "01":
                    dtype = "1905C70010";       //開通並讀取設備狀態資料
                    break;
                case "02":
                    dtype = "1905600000";       //閃爍面板
                    break;
                case "03":
                    dtype = "19300008";       //搜尋1.2.3級面板(藍芽用)
                    break;
                case "04":
                    dtype = "19300108";       //搜尋1.2級面板Only(藍芽用)
                    break;
                case "05":
                    dtype = "19300208";       //搜尋1.2.3級面板(網路用)
                    break;
                case "08":
                case "0801":
                    dtype = "1905A20000";       //1.2.3切面板第一鍵
                    break;
                case "0802":
                    dtype = "1905A30000";       //1.2.3切面板第二鍵
                    break;
                case "0803":
                    dtype = "1905A40000";       //1.2.3切面板第三鍵
                    break;
                case "0804":
                    dtype = "1905A50000";       //1.2.3切面板第四鍵
                    break;
                case "0805":
                    dtype = "1905A60000";       //1.2.3切面板第五鍵
                    break;
                case "0806":
                    dtype = "1905A70000";       //1.2.3切面板第六鍵
                    break;
                case "08CH1BT":
                    dtype = "1905AA03";       //1.2.3切面板第一迴路調光(YY=0x00~0x33)  00~33 according to % on 100% scale
                    break;
                case "08CH2BT":
                    dtype = "1905AC03";       //2.3切面板第二迴路調光
                    break;
                case "08CH3BT":
                    dtype = "1905AE03";       //3切面板第三迴路調光  
                    break;
                case "08CH1CT":
                    dtype = "1905AA04";       //1.2.3切面板第一迴路調色溫(YY=00~0x64)
                    break;
                case "08CH2CT":
                    dtype = "1905AE04";       //2.3切面板第二迴路調色溫
                    break;
                case "08CH3CT":
                    dtype = "1905AE04";       //3切面板第三迴路調色溫
                    break;
            }
            return dtype;
        }

        public string getBleDeviceID(string request)
        {
            string result = string.Empty;

            switch (request)
            {
                case "MAXBook":
                    result = "0A";
                    break;
                case "MAXScene":
                    result = "0B";
                    break;
                case "MAXAir":
                    result = "13";
                    break;
                case "MAXLite1":
                    result = "14";
                    break;
                case "MAXLite2":
                    result = "16";
                    break;
                case "MAXLite3":
                    result = "18";
                    break;
            }
            return result;
        }


    }
}