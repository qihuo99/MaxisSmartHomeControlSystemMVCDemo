using MaxisSmartHomeControlSystemMVCDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MaxisSmartHomeControlSystemMVCDemo.Helper
{
    public class RexliteDBFunctions
    {
        private readonly RexliteDBEntities rexliteDBEntities = new RexliteDBEntities();

        public int GetDeviceRecordCount()
        {
            //Get total record count from device table
            int cnt = rexliteDBEntities.Devices.Count();

            return cnt;
        }


    }
}