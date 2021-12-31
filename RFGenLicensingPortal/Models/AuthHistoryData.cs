using System;
using System.Data.SqlClient;

namespace RFGenLicensingPortal.Models {
  public class AuthHistoryData {
    public long lKey;
    public string sRFUser;
    public string sOldID;
    public int iUsers_Auth;
    public DateTime dInstall_Date;
    public string sEmployee;
    public string sComments;
    public int iApps_Auth;
    public int iUsersAllowed;
    public DateTime dtSupportExpires;
    public string sAuth_Mode;
    public string sAuth_Type;
    public DateTime dAuthCode_Expiry;
    public bool bSM_Enabled;
    public bool bJDE_Enabled;
    public bool bSAP_Enabled;
    public bool bMS_Enabled;
    public bool bXpress_Enabled;
    public int iServerAuth_Count;
    public string sSerial_ID;
    public int iVocollect_Count;
    public int iSaas_Count;
    public string sAuth_Code;
    public int iBatch_Count;
    public bool bTrial_Enabled;
    public int iTrial_Time;
    public bool bSaas_Enabled;
    public DateTime dSaasContract_Start;
    public DateTime dSaasContract_End;
    public bool bOracle_enabled;
    public bool bSAPB1_Enabled;
    public bool bCostPoint_Enabled;
    public bool bVocollect_Enabled;
    public bool bBackup_Enabled;
    public bool bTest_Enabled;
    public bool bSCM_Enabled;
    public bool bHCP_Enabled;

    public AuthHistoryData(SqlDataReader oReader) {
      try {
        if (oReader == null) {
          Logger.Error("Record object set to null.", Environment.StackTrace);
          return;
        }

        if (!oReader.HasRows) {
          Logger.Error("No records supplied in object creation.", Environment.StackTrace);
          return;
        }

        if (!oReader.IsDBNull(0)) this.lKey = oReader.GetInt64(0);
        if (!oReader.IsDBNull(1)) this.sRFUser = oReader.GetString(1);
        if (!oReader.IsDBNull(2)) this.sOldID = oReader.GetString(2);
        if (!oReader.IsDBNull(3)) this.iUsers_Auth = oReader.GetInt32(3);
        if (!oReader.IsDBNull(4)) this.dInstall_Date = oReader.GetDateTime(4);
        if (!oReader.IsDBNull(5)) this.sEmployee = oReader.GetString(5);
        if (!oReader.IsDBNull(6)) this.sComments = oReader.GetString(6);
        if (!oReader.IsDBNull(7)) this.iApps_Auth = oReader.GetInt32(7);
        if (!oReader.IsDBNull(8)) this.iUsersAllowed = oReader.GetInt32(8);
        if (!oReader.IsDBNull(9)) this.dtSupportExpires = oReader.GetDateTime(9);
        if (!oReader.IsDBNull(10)) this.sAuth_Mode = oReader.GetString(10);
        if (!oReader.IsDBNull(11)) this.sAuth_Type = oReader.GetString(11);
        if (!oReader.IsDBNull(12)) this.dAuthCode_Expiry = oReader.GetDateTime(12);
        if (!oReader.IsDBNull(13)) this.bSM_Enabled = oReader.GetBoolean(13);
        if (!oReader.IsDBNull(14)) this.bJDE_Enabled = oReader.GetBoolean(14);
        if (!oReader.IsDBNull(15)) this.bSAP_Enabled = oReader.GetBoolean(15);
        if (!oReader.IsDBNull(16)) this.bMS_Enabled = oReader.GetBoolean(16);
        if (!oReader.IsDBNull(17)) this.bXpress_Enabled = oReader.GetBoolean(17);
        if (!oReader.IsDBNull(18)) this.iServerAuth_Count = oReader.GetInt32(18);
        if (!oReader.IsDBNull(19)) this.sSerial_ID = oReader.GetString(19);
        if (!oReader.IsDBNull(20)) this.iVocollect_Count = oReader.GetInt32(20);
        if (!oReader.IsDBNull(21)) this.iSaas_Count = oReader.GetInt32(21);
        if (!oReader.IsDBNull(22)) this.sAuth_Code = oReader.GetString(22);
        if (!oReader.IsDBNull(23)) this.iBatch_Count = oReader.GetInt32(23);
        if (!oReader.IsDBNull(24)) this.bTrial_Enabled = oReader.GetBoolean(24);
        if (!oReader.IsDBNull(25)) this.iTrial_Time = oReader.GetInt32(25);
        if (!oReader.IsDBNull(26)) this.bSaas_Enabled = oReader.GetBoolean(26);
        if (!oReader.IsDBNull(27)) this.dSaasContract_Start = oReader.GetDateTime(27);
        if (!oReader.IsDBNull(28)) this.dSaasContract_End = oReader.GetDateTime(28);
        if (!oReader.IsDBNull(29)) this.bOracle_enabled = oReader.GetBoolean(29);
        if (!oReader.IsDBNull(30)) this.bSAPB1_Enabled = oReader.GetBoolean(30);
        if (!oReader.IsDBNull(31)) this.bCostPoint_Enabled = oReader.GetBoolean(31);
        if (!oReader.IsDBNull(32)) this.bVocollect_Enabled = oReader.GetBoolean(32);
        if (!oReader.IsDBNull(33)) this.bBackup_Enabled = oReader.GetBoolean(33);
        if (!oReader.IsDBNull(34)) this.bTest_Enabled = oReader.GetBoolean(34);
        if (!oReader.IsDBNull(35)) this.bSCM_Enabled = oReader.GetBoolean(35);
        if (!oReader.IsDBNull(36)) this.bHCP_Enabled = oReader.GetBoolean(36);
      } catch(Exception e) {
        Logger.Error("Object Creation Error: ", Environment.StackTrace);
        return;
      }
    }
  }
}
