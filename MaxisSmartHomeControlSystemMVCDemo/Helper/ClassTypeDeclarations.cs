using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MaxisSmartHomeControlSystemMVCDemo.Helper
{
    public class ClassTypeDeclarations
    {
    }

    public class BleJsonDevice
    {
        public string BleID { get; set; }
        public string BleSN { get; set; }
        public string BleName { get; set; }
    }

    public enum CallCommandSend : int
    {
        ConnectToMAXBook = 13,                  //連線哪台設備
        ActivateSendBackFunction = 06,          //開通並讀取設備狀態資料
        BlinkingPanel = 06,                     //閃爍面板
        Search_1_2_3_Panel_BlueTooth = 12,      //搜尋1.2.3級面板(藍芽用)
        Search_1_2_Panel_Only_BlueTooth = 12,   //搜尋1.2級面板(藍芽用)
        Search_1_2_3_Panel_Ethernet = 12,       //搜尋1.2級面板(網路用)
        MAXScene = 06,                          //情境面板
        MAXLite = 06                            //切面板
    }

}