using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Reflection;

namespace RFGenLicensingPortal.Models {
  public class CustomerInfoData : IEnumerable<string> {
    private static DateTime dtSQLMinDateTime = new DateTime(1753, 01, 01);
    private static DateTime dtSQLMaxDateTime = new DateTime(9999, 12, 31);
    public string sRFUser { get; set; }
    public string sAllocGrp { get; set; }
    public string sVAR { get; set; }
    public string sCustomer { get; set; }
    public int iUsersAuth { get; set; }
    public int iMobileAuth { get; set; }
    public int iVoiceProd { get; set; }
    public string sSysID { get; set; }
    public DateTime dtInstallDt { get; set; }
    public DateTime dtSupportExp { get; set; }
    public float fReleaseNo { get; set; }
    public string sSales { get; set; }
    public string sComments { get; set; }
    public string sInstallCnt { get; set; }
    public int iServiceTime { get; set; }
    public int iTrialTime { get; set; }
    public bool bSMEnabled { get; set; }
    public bool bCreditHold { get; set; }
    public bool bXpress { get; set; }
    public bool bJDEEnabled { get; set; }
    public bool bSAPEnabled { get; set; }
    public bool bOracleEnabled { get; set; }
    public bool bCostpointEnabled { get; set; }
    public bool bMSEnabled { get; set; }
    public bool bBackup { get; set; }
    public bool bTest { get; set; }
    public bool bLapse { get; set; }
    public bool bServiceEnabled { get; set; }
    public bool bTrialEnabled { get; set; }
    public string sSerialID { get; set; }
    public bool bVocollect { get; set; }
    public int iServerAuth { get; set; }
    public bool bSAPB1Enabled { get; set; }
    public int iAppsAuth { get; set; }
    public string sAcctID { get; set; }
    public string sServerCost { get; set; }
    public string sUserCost { get; set; }
    public string sMobileCost { get; set; }
    public int iUsersAllowed { get; set; }
    public string sServerNotes { get; set; }
    public bool bObsolete { get; set; }
    public DateTime dtContractSt { get; set; }
    public DateTime dtContractEd { get; set; }
    public DateTime dtSvcExp { get; set; }
    public DateTime dtHeartbeat { get; set; }
    public int iMinUsers { get; set; }
    public bool bSCMEnabled { get; set; }
    public bool bHCPEnabled { get; set; }
    public int iBCSCount { get; set; }

    public CustomerInfoData() { }

    public IEnumerator<string> GetEnumerator() {
      return (new List<string>() {
            sRFUser, sAllocGrp, sVAR, sCustomer, iUsersAuth.ToString(), iMobileAuth.ToString(),
            iVoiceProd.ToString(), sSysID, dtInstallDt.ToString(), dtSupportExp.ToString(),
            fReleaseNo.ToString(), sSales, sComments, sInstallCnt, iServiceTime.ToString(),
            iTrialTime.ToString(), bSMEnabled.ToString(), bCreditHold.ToString(), bXpress.ToString(),
            bJDEEnabled.ToString(), bSAPEnabled.ToString(), bOracleEnabled.ToString(),
            bCostpointEnabled.ToString(), bMSEnabled.ToString(), bBackup.ToString(),
            bTest.ToString(), bLapse.ToString(), bServiceEnabled.ToString(), bTrialEnabled.ToString(),
            sSerialID, bVocollect.ToString(), iServerAuth.ToString(), bSAPB1Enabled.ToString(),
            iAppsAuth.ToString(), sAcctID, sUserCost, sMobileCost, iUsersAllowed.ToString(),
            sServerNotes, bObsolete.ToString(), dtContractSt.ToString(), dtContractEd.ToString(),
            dtSvcExp.ToString(), dtHeartbeat.ToString(), iMinUsers.ToString(), bSCMEnabled.ToString(),
            bHCPEnabled.ToString(), iBCSCount.ToString()
        }).GetEnumerator();
    }
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    public CustomerInfoData(SqlDataReader oReader) {
      try {
        if(oReader == null) {
          Logger.Error("Record object set to null.", Environment.StackTrace);
          return;
        }

        if (!oReader.HasRows) {
          Logger.Error("No records supplied in object creation.", Environment.StackTrace);
          return;
        }

        if (!oReader.IsDBNull(0)) this.sRFUser = oReader.GetString(0);
        if (!oReader.IsDBNull(1)) this.sAllocGrp = oReader.GetString(1);
        if (!oReader.IsDBNull(2)) this.sVAR = oReader.GetString(2);
        if (!oReader.IsDBNull(3)) this.sCustomer = oReader.GetString(3);
        if (!oReader.IsDBNull(4)) this.iUsersAuth = oReader.GetInt32(4);
        if (!oReader.IsDBNull(5)) this.iMobileAuth = oReader.GetInt32(5);
        if (!oReader.IsDBNull(6)) this.iVoiceProd = oReader.GetInt32(6);
        if (!oReader.IsDBNull(7)) this.sSysID = oReader.GetString(7);
        if (!oReader.IsDBNull(8)) this.dtInstallDt = oReader.GetDateTime(8);
        if (!oReader.IsDBNull(9)) this.dtSupportExp = oReader.GetDateTime(9);
        if (!oReader.IsDBNull(10)) this.fReleaseNo = oReader.GetFloat(10);
        if (!oReader.IsDBNull(11)) this.sSales = oReader.GetString(11);
        if (!oReader.IsDBNull(12)) this.sComments = oReader.GetString(12);
        if (!oReader.IsDBNull(13)) this.sInstallCnt = oReader.GetString(13);
        if (!oReader.IsDBNull(14)) this.iServiceTime = oReader.GetInt32(14);
        if (!oReader.IsDBNull(15)) this.iTrialTime = oReader.GetInt32(15);
        if (!oReader.IsDBNull(16)) this.bSMEnabled = oReader.GetBoolean(16);
        if (!oReader.IsDBNull(17)) this.bCreditHold = oReader.GetBoolean(17);
        if (!oReader.IsDBNull(18)) this.bXpress = oReader.GetBoolean(18);
        if (!oReader.IsDBNull(19)) this.bJDEEnabled = oReader.GetBoolean(19);
        if (!oReader.IsDBNull(20)) this.bSAPEnabled = oReader.GetBoolean(20);
        if (!oReader.IsDBNull(21)) this.bOracleEnabled = oReader.GetBoolean(21);
        if (!oReader.IsDBNull(22)) this.bCostpointEnabled = oReader.GetBoolean(22);
        if (!oReader.IsDBNull(23)) this.bMSEnabled = oReader.GetBoolean(23);
        if (!oReader.IsDBNull(24)) this.bBackup = oReader.GetBoolean(24);
        if (!oReader.IsDBNull(25)) this.bTest = oReader.GetBoolean(25);
        if (!oReader.IsDBNull(26)) this.bLapse = oReader.GetBoolean(26);
        if (!oReader.IsDBNull(27)) this.bServiceEnabled = oReader.GetBoolean(27);
        if (!oReader.IsDBNull(28)) this.bTrialEnabled = oReader.GetBoolean(28);
        if (!oReader.IsDBNull(29)) this.sSerialID = oReader.GetString(29);
        if (!oReader.IsDBNull(30)) this.bVocollect = oReader.GetBoolean(30);
        if (!oReader.IsDBNull(31)) this.iServerAuth = oReader.GetInt32(31);
        if (!oReader.IsDBNull(32)) this.bSAPB1Enabled = oReader.GetBoolean(32);
        if (!oReader.IsDBNull(33)) this.iAppsAuth = oReader.GetInt32(33);
        if (!oReader.IsDBNull(34)) this.sAcctID = oReader.GetString(34);
        if (!oReader.IsDBNull(35)) this.sServerCost = oReader.GetString(35);
        if (!oReader.IsDBNull(36)) this.sUserCost = oReader.GetString(36);
        if (!oReader.IsDBNull(37)) this.sMobileCost = oReader.GetString(37);
        if (!oReader.IsDBNull(38)) this.iUsersAllowed = oReader.GetInt32(38);
        if (!oReader.IsDBNull(39)) this.sServerNotes = oReader.GetString(39);
        if (!oReader.IsDBNull(40)) this.bObsolete = oReader.GetBoolean(40);
        if (!oReader.IsDBNull(41)) this.dtContractSt = oReader.GetDateTime(41);
        if (!oReader.IsDBNull(42)) this.dtContractEd = oReader.GetDateTime(42);
        if (!oReader.IsDBNull(43)) this.dtSvcExp = oReader.GetDateTime(43);
        if (!oReader.IsDBNull(44)) this.dtHeartbeat = oReader.GetDateTime(44);
        if (!oReader.IsDBNull(45)) this.iMinUsers = oReader.GetInt32(45);
        if (!oReader.IsDBNull(46)) this.bSCMEnabled = oReader.GetBoolean(46);
        if (!oReader.IsDBNull(47)) this.bHCPEnabled = oReader.GetBoolean(47);
        if (!oReader.IsDBNull(48)) this.iBCSCount = oReader.GetInt32(48);
      } catch (Exception e) {
        Logger.Error("Object Creation Error: ", Environment.StackTrace);
        return;
      }
    }

    public Dictionary<string, string> GetChangedColumns(CustomerInfoData oCustomerInfoData) {
      Dictionary<string, string> oDict = new Dictionary<string, string>();

      PropertyInfo[] oCustInfoProps = oCustomerInfoData.GetType().GetProperties();
      PropertyInfo[] oThisProps = this.GetType().GetProperties();

      //they will both be the same size
      for(int i = 0; i < oCustInfoProps.Length; i++) {
        var oPropCust = oCustInfoProps[i].GetValue(oCustomerInfoData);
        var oPropThis = oThisProps[i].GetValue(this);

        //checking if they are not equal, if they are not equal that property
        //has been changed, and we ignore null values
        if (oPropThis != null && oPropCust != null && !oPropCust.Equals(oPropThis))
          oDict.Add(oThisProps[i].Name, oPropThis.ToString());
      }

      return oDict;
    }

    // SQL only allows dates within a specific range for the DateTime type. Trying to call an insert function with a
    // date outside of that range causes an exception to be thrown. This function clamps dates to the safe range.
    private string getSQLAppropriateDateTime(DateTime dtInput) {
      if (DateTime.Compare(dtInput, dtSQLMinDateTime) < 0) {
        return dtSQLMinDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
      } else if (DateTime.Compare(dtInput, dtSQLMaxDateTime) > 0) {
        return dtSQLMaxDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
      } else {
        return dtInput.ToString("yyyy-MM-dd HH:mm:ss.fff");
      }
    }
  }
}
