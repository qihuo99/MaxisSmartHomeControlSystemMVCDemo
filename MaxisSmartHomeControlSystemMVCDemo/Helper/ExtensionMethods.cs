using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace MaxisSmartHomeControlSystemMVCDemo.Helper
{
    public static class ExtensionMethods
    {

        //This is the very first command to send to request the MaxBook ID info via tcpip client call
        //public const string MaxBookStr = "0A13FFFFFFFFFFFF19820008FE7AF4";
        //public const string BroadcastStr = "0A12000000000001193002081957";
        public const string MaxBookStr = "0A13FFFFFFFFFFFF19820008FE7AF4";
        public const string BroadcastStr = "0A12000000000001193002081957";

        public const string InternalIP = "192.168.1.96";
        public const int PortNumber = 4000;
        public const string JsonFileNameLocal = "bleDeviceList.json";


        public static string FormatStringArrayForHex(string[] ccArr)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < ccArr.Length; i++)
            {
                if (ccArr[i].Length == 1)
                    ccArr[i] = "0" + ccArr[i];
                sb = sb.Append(ccArr[i]);
            }

            return sb.ToString();
        }

        //string轉byte  -- this is Jack's new method
        public static byte[] GetBytes(string HexString)
        {
            int byteLength = HexString.Length / 2;
            byte[] bytes = new byte[byteLength];
            string hex;
            int j = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                hex = new String(new Char[] { HexString[j], HexString[j + 1] });
                bytes[i] = HexToByte(hex);
                j = j + 2;
            }
            return bytes;
        }

        //-- this is Jack's new method
        public static byte HexToByte(string hex)
        {
            if (hex.Length > 2 || hex.Length <= 0)
                throw new ArgumentException("hex must be 1 or 2 characters in length");
            byte newByte = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            return newByte;
        }

        //16進位數字組成的字串轉換為Byte[] -- this is my old method
        public static byte[] HexToByte2(this string hexString)
        {
            //運算後的位元組長度:16進位數字字串長/2
            byte[] byteOUT = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i = i + 2)
            {
                //每2位16進位數字轉換為一個10進位整數
                byteOUT[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return byteOUT;  //the result will be hex to dec in byte format
        }

        //this is a method to check if tcpip is still active and stay connected
        //Since there may be multiple connections to your Local Endpoint
        public static TcpState GetState(this TcpClient tcpClient)
        {
            var foo = IPGlobalProperties.GetIPGlobalProperties()
              .GetActiveTcpConnections()
              .SingleOrDefault(x => x.LocalEndPoint.Equals(tcpClient.Client.LocalEndPoint)
                                 && x.RemoteEndPoint.Equals(tcpClient.Client.RemoteEndPoint)
              );
            return foo != null ? foo.State : TcpState.Unknown;
        }

        public static bool CheckJsonFile(string jsonFile)
        {
            //string BleJsonListFilePath = HttpContext.Current.Server.MapPath("~/Helper/");
            //string fullFileName = BleJsonListFilePath + jsonFile;
            if (File.Exists(jsonFile))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}