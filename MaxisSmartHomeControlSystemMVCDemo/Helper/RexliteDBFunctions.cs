using MaxisSmartHomeControlSystemMVCDemo.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
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

        //public List<GetDeviceListWithUpdatesByBleId_Result> GetDeviceDataInListArrayByBleID(string BleID)
        //{
        //    //Execute stored procedure as a function
        //    List<GetDeviceListWithUpdatesByBleId_Result> deviceList = rexliteDBEntities.GetDeviceListWithUpdatesByBleId(BleID).ToList();

        //    return deviceList;
        //}

        public List<GetUserRole_Result> GetUserRole()
        {
            //Execute stored procedure as a function
            List<GetUserRole_Result> UserRoleList = rexliteDBEntities.GetUserRole().ToList();

            return UserRoleList;
        }



        //public List<GetUserDataByUserNameAndUserEmailAndUserPwd_Result> GetSingleUserDataByUserNameEmailPwd(string UserName, string UserPwd, string UserEmail)
        //{
        //    //Execute stored procedure as a function
        //    List<GetUserDataByUserNameAndUserEmailAndUserPwd_Result> userDataList = rexliteDBEntities.GetUserDataByUserNameAndUserEmailAndUserPwd(UserName, UserPwd, UserEmail).ToList();


        //    return userDataList;
        //}

        public int TruncateDeviceTmpTableData()
        {
            //This command is to truncate Device Table and clear all data
            int output = rexliteDBEntities.Database.ExecuteSqlCommand("TRUNCATE TABLE DeviceTmp");

            return output;  //if return -1 it means success on truncating the designated table
        }

        public List<GetUserByUserEmail_Result> GetUserDataByUserEmail(string userEmail)
        {
            //Execute stored procedure as a function
            List<GetUserByUserEmail_Result> userDataList = rexliteDBEntities.GetUserByUserEmail(userEmail).ToList();

            return userDataList;
        }

        public int CheckIfDeviceDataNeedToUpdate()
        {
            //Execute stored procedure as a function
            ObjectParameter HasNoMatchCount = new ObjectParameter("HasNoMatchCount", typeof(Int32));
           // rexliteDBEntities.CheckIfDeviceNeedUpdate(HasNoMatchCount);
            int getDeviceCheckStatus = Convert.ToInt32(HasNoMatchCount.Value);

            return getDeviceCheckStatus;
        }

        public int UpdateDeviceBleListData()
        {
            //Execute stored procedure as a function
            ObjectParameter NotMatchingRowCountInt = new ObjectParameter("NotMatchingRowCount", typeof(Int32));
            //rexliteDBEntities.UpdateDeviceBleList(NotMatchingRowCountInt);
            int getDeviceUpdateStatus = Convert.ToInt32(NotMatchingRowCountInt.Value);

            return getDeviceUpdateStatus;
        }







    }
}