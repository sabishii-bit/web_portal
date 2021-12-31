using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RFGenLicensingPortal.Models {
  public static class CustomerInfo {
    private static DateTime dtSQLMinDateTime = new DateTime(1753, 01, 01);
    private static DateTime dtSQLMaxDateTime = new DateTime(9999, 12, 31);
    public static CustomerInfoData GetRecord(string sRFUser) {
      CustomerInfoData oRecord;
      string sCommandText = "SELECT * FROM CUSTOMERINFO WHERE RFUser = @RFUser;";

      SqlCommand dbCommand = new SqlCommand(sCommandText, DBHandle.mdbHandle);
      dbCommand.Parameters.AddWithValue("@RFUser", sRFUser);
      SqlDataReader oReader = DBHandle.Query(dbCommand);
      if (oReader == null) return null;

      try {
        if (oReader.Read())
          oRecord = new CustomerInfoData(oReader);
        else
          oRecord = null;

        oReader.Close();
        return oRecord;
      } catch(Exception e) {
        Logger.Error("Get Record Error: " + e.Message, Environment.StackTrace);
        try { oReader.Close(); } catch (Exception f) { };
        return null;
      }
    }

    //TODO merge list and searchresults
    public static List<CustomerInfoData> List(int iCnt, int iPg, string sSearch) {
      List<CustomerInfoData> oRecordList = new List<CustomerInfoData>();

      SqlDataReader oReader;
      if (sSearch == null) sSearch = "";
      //Get records for database
      string sCommandText = "SELECT * FROM CUSTOMERINFO " +
                            "WHERE (RFUSER LIKE @Search OR " +
                            "Customer LIKE @Search OR " +
                            "VAR LIKE @Search OR " +
                            "ServerNotes LIKE @Search OR " +
                            "FORMAT(Support_Expires, 'M/d/yyyy') " +
                            "LIKE @Search) AND " +
                            "OBSOLETE = 0 " +
                            "ORDER BY RFUSER OFFSET @Offset ROWS " +
                            "FETCH NEXT @Rows ROWS ONLY;";
      SqlCommand dbCommand = new SqlCommand(sCommandText, DBHandle.mdbHandle);
      dbCommand.Parameters.AddWithValue("@Search", "%" + sSearch + "%");
      dbCommand.Parameters.AddWithValue("@Offset", (iCnt * (iPg - 1)));
      dbCommand.Parameters.AddWithValue("@Rows", iCnt);
      oReader = DBHandle.Query(dbCommand);
      
      //If no results return empty list
      if (oReader == null) return oRecordList;

      try {
        // add records to list
        while (oReader.Read()) oRecordList.Add(new CustomerInfoData(oReader));

        oReader.Close();
        return oRecordList;
      } catch(Exception e) {
        Logger.Error("Get List Error: " + e.Message, Environment.StackTrace);
        try { oReader.Close(); } catch (Exception f) { };
        return null;
      }
    }

    public static int Count(string sSearch) {
      int iVal;
      SqlDataReader oReader;
      if (sSearch == null) sSearch = "";
      string sCommandText = "SELECT COUNT(RFUSER) FROM CUSTOMERINFO " +
                            "WHERE (RFUSER LIKE @Search OR " +
                            "Customer LIKE @Search OR " +
                            "VAR LIKE @Search OR " +
                            "ServerNotes LIKE @Search OR " +
                            "FORMAT(Support_Expires, 'M/d/yyyy') " +
                            "LIKE @Search) AND " +
                            "OBSOLETE = 0;";
      SqlCommand dbCommand = new SqlCommand(sCommandText, DBHandle.mdbHandle);
      dbCommand.Parameters.AddWithValue("@Search", "%" + sSearch + "%");
      oReader = DBHandle.Query(dbCommand);

      if (oReader == null) return 0;

      try {
        oReader.Read();
        iVal = oReader.GetInt32(0);
        oReader.Close();
        return iVal;
      } catch(Exception e) {
        Logger.Error("Count Error: " + e.Message, Environment.StackTrace);
        try { oReader.Close(); } catch (Exception f) { };
        return -1;
      }
    }

    public static bool Insert(CustomerInfoData oCS) {
      string sCommandText = "INSERT INTO CustomerInfo " +
                            "(RFUser, Alloc_Group, VAR, Customer, " +
                            "Users_Auth, Mobile_Auth, VoiceProd, SysID, " +
                            "Install_Date, Support_Expires, ReleaseNo, " +
                            "Sales, Comments, Install_Cnt, Service_Time, " +
                            "Trial_Time, SM_Enabled, CreditHold, Xpress, " +
                            "JDE_Enabled, SAP_Enabled, Oracle_Enabled, " +
                            "Costpoint_Enabled, MS_Enabled, [Backup], Test, " +
                            "Lapse, Service_Enabled, Trial_Enabled, " +
                            "SerialID, Vocollect, Server_Auth, " +
                            "SAPB1_Enabled, Apps_Auth, AcctID, Server_Cost, " +
                            "User_Cost, Mobile_Cost, UsersAllowed, " +
                            "ServerNotes, Obsolete, Contract_Start, " +
                            "Contract_End, Service_Expire, HeartBeat, " +
                            "MinUsers, SCM_Enabled, HCP_Enabled, BCS_Cnt) " +
                            "VALUES (@RFUser, @Alloc_Group, @VAR, " +
                            "@Customer, @Users_Auth, @Mobile_Auth, " +
                            "@VoiceProd, @SysID, @Install_Date, " +
                            "@Support_Expires, @ReleaseNo, @Sales, " +
                            "@Comments, @Install_Cnt, @Service_Time, " +
                            "@Trial_Time, @SM_Enabled, @CreditHold, " +
                            "@Xpress, @JDE_Enabled, @SAP_Enabled, " +
                            "@Oracle_Enabled, @Costpoint_Enabled, " +
                            "@MS_Enabled, @Backup, @Test, @Lapse, " +
                            "@Service_Enabled, @Trial_Enabled, @SerialID, " +
                            "@Vocollect, @Server_Auth, @SAPB1_Enabled, " +
                            "@Apps_Auth, @AcctID, @Server_Cost, @User_Cost, " +
                            "@Mobile_Cost, @UsersAllowed, @ServerNotes, " +
                            "@Obsolete, @Contract_Start, @Contract_End, " +
                            "@Service_Expire, @HeartBeat, @MinUsers, " +
                            "@SCM_Enabled, @HCP_Enabled, @BCS_Cnt);";

      SqlCommand dbCommand = new SqlCommand(sCommandText, DBHandle.mdbHandle);
      dbCommand.Parameters.AddWithValue("@RFUser", oCS.sRFUser);
      dbCommand.Parameters.AddWithValue("@Alloc_Group", oCS.sAllocGrp);
      dbCommand.Parameters.AddWithValue("@VAR", oCS.sVAR);
      dbCommand.Parameters.AddWithValue("@Customer", oCS.sCustomer);
      dbCommand.Parameters.AddWithValue("@Users_Auth", oCS.iUsersAuth);
      dbCommand.Parameters.AddWithValue("@Mobile_Auth", oCS.iMobileAuth);
      dbCommand.Parameters.AddWithValue("@VoiceProd", oCS.iVoiceProd);
      dbCommand.Parameters.AddWithValue("@SysID", oCS.sSysID);
      dbCommand.Parameters.AddWithValue("@Install_Date",
                                   getSQLAppropriateDateTime(oCS.dtInstallDt));
      dbCommand.Parameters.AddWithValue("@Support_Expires",
                                   getSQLAppropriateDateTime(oCS.dtSupportExp));
      dbCommand.Parameters.AddWithValue("@ReleaseNo", oCS.fReleaseNo);
      dbCommand.Parameters.AddWithValue("@Sales", oCS.sSales);
      dbCommand.Parameters.AddWithValue("@Comments", oCS.sComments);
      dbCommand.Parameters.AddWithValue("@Install_Cnt", oCS.sInstallCnt);
      dbCommand.Parameters.AddWithValue("@Service_Time", oCS.iServiceTime);
      dbCommand.Parameters.AddWithValue("@Trial_Time", oCS.iTrialTime);
      dbCommand.Parameters.AddWithValue("@SM_Enabled", oCS.bSMEnabled);
      dbCommand.Parameters.AddWithValue("@CreditHold", oCS.bCreditHold);
      dbCommand.Parameters.AddWithValue("@Xpress", oCS.bXpress);
      dbCommand.Parameters.AddWithValue("@JDE_Enabled", oCS.bJDEEnabled);
      dbCommand.Parameters.AddWithValue("@SAP_Enabled", oCS.bSAPEnabled);
      dbCommand.Parameters.AddWithValue("@Oracle_Enabled", oCS.bOracleEnabled);
      dbCommand.Parameters.AddWithValue("@Costpoint_Enabled",
                                        oCS.bCostpointEnabled);
      dbCommand.Parameters.AddWithValue("@MS_Enabled", oCS.bMSEnabled);
      dbCommand.Parameters.AddWithValue("@Backup", oCS.bBackup);
      dbCommand.Parameters.AddWithValue("@Test", oCS.bTest);
      dbCommand.Parameters.AddWithValue("@Lapse", oCS.bLapse);
      dbCommand.Parameters.AddWithValue("@Service_Enabled",
                                        oCS.bServiceEnabled);
      dbCommand.Parameters.AddWithValue("@Trial_Enabled", oCS.bTrialEnabled);
      dbCommand.Parameters.AddWithValue("@SerialID", oCS.sSerialID);
      dbCommand.Parameters.AddWithValue("@Vocollect", oCS.bVocollect);
      dbCommand.Parameters.AddWithValue("@Server_Auth", oCS.iServerAuth);
      dbCommand.Parameters.AddWithValue("@SAPB1_Enabled", oCS.bSAPB1Enabled);
      dbCommand.Parameters.AddWithValue("@Apps_Auth", oCS.iAppsAuth);
      dbCommand.Parameters.AddWithValue("@AcctID", oCS.sAcctID);
      dbCommand.Parameters.AddWithValue("@Server_Cost", oCS.sServerCost);
      dbCommand.Parameters.AddWithValue("@User_Cost", "");
      dbCommand.Parameters.AddWithValue("@Mobile_Cost", "");
      dbCommand.Parameters.AddWithValue("@UsersAllowed", oCS.iUsersAllowed);
      dbCommand.Parameters.AddWithValue("@ServerNotes", oCS.sServerNotes);
      dbCommand.Parameters.AddWithValue("@Obsolete", oCS.bObsolete);
      dbCommand.Parameters.AddWithValue("@Contract_Start",
                                 getSQLAppropriateDateTime(oCS.dtContractSt));
      dbCommand.Parameters.AddWithValue("@Contract_End",
                                 getSQLAppropriateDateTime(oCS.dtContractEd));
      dbCommand.Parameters.AddWithValue("@Service_Expire",
                                     getSQLAppropriateDateTime(oCS.dtSvcExp));
      dbCommand.Parameters.AddWithValue("@HeartBeat",
                                  getSQLAppropriateDateTime(oCS.dtHeartbeat));
      dbCommand.Parameters.AddWithValue("@MinUsers", oCS.iMinUsers);
      dbCommand.Parameters.AddWithValue("@SCM_Enabled", oCS.bSCMEnabled);
      dbCommand.Parameters.AddWithValue("@HCP_Enabled", oCS.bHCPEnabled);
      dbCommand.Parameters.AddWithValue("@BCS_Cnt", oCS.iBCSCount);

      if (DBHandle.NonQuery(dbCommand) == -1) {
        Logger.Error("DB Insert Error.", Environment.StackTrace);
        return false;
      }

      return true;
    }

    public static bool Update(CustomerInfoData oCS) {
      string sCommandText = "UPDATE CustomerInfo " +
                            "SET Alloc_Group=@Alloc_Group, " +
                            "VAR=@VAR, " +
                            "Customer=@Customer, " +
                            "Users_Auth=@Users_Auth, " +
                            "Mobile_Auth=@Mobile_Auth, " +
                            "VoiceProd=@VoiceProd, " +
                            "SysID=@SysID, " +
                            "Install_Date=@Install_Date, " +
                            "Support_Expires=@Support_Expires, " +
                            "ReleaseNo=@ReleaseNo, " +
                            "Sales=@Sales, " +
                            "Comments=@Comments, " +
                            "Install_Cnt=@Install_Cnt, " +
                            "Service_Time=@Service_Time, " +
                            "Trial_Time=@Trial_Time, " +
                            "SM_Enabled=@SM_Enabled, " +
                            "CreditHold=@CreditHold, " +
                            "Xpress=@Xpress, " +
                            "JDE_Enabled=@JDE_Enabled, " +
                            "SAP_Enabled=@SAP_Enabled, " +
                            "Oracle_Enabled=@Oracle_Enabled, " +
                            "Costpoint_Enabled=@Costpoint_Enabled, " +
                            "MS_Enabled=@MS_Enabled, " +
                            "[Backup]=@Backup, " +
                            "Test=@Test, " +
                            "Lapse=@Lapse, " +
                            "Service_Enabled=@Service_Enabled, " +
                            "Trial_Enabled=@Trial_Enabled, " +
                            "SerialID=@SerialID, " +
                            "Vocollect=@Vocollect, " +
                            "Server_Auth=@Server_Auth, " +
                            "SAPB1_Enabled=@SAPB1_Enabled, " +
                            "Apps_Auth=@Apps_Auth, " +
                            "Server_Cost=@Server_Cost, " +
                            "UsersAllowed=@UsersAllowed, " +
                            "ServerNotes=@ServerNotes, " +
                            "Obsolete=@Obsolete, " +
                            "Contract_Start=@Contract_Start, " +
                            "Contract_End=@Contract_End, " +
                            "Service_Expire=@Service_Expire, " +
                            "HeartBeat=@HeartBeat, " +
                            "MinUsers=@MinUsers, " +
                            "SCM_Enabled=@SCM_Enabled, " +
                            "HCP_Enabled=@HCP_Enabled, " +
                            "BCS_Cnt=@BCS_Cnt " +
                            "WHERE (RFUser=@RFUser);";

      SqlCommand dbCommand = new SqlCommand(sCommandText, DBHandle.mdbHandle);
      dbCommand.Parameters.AddWithValue("@Alloc_Group", oCS.sAllocGrp);
      dbCommand.Parameters.AddWithValue("@VAR", oCS.sVAR);
      dbCommand.Parameters.AddWithValue("@Customer", oCS.sCustomer);
      dbCommand.Parameters.AddWithValue("@Users_Auth", oCS.iUsersAuth);
      dbCommand.Parameters.AddWithValue("@Mobile_Auth", oCS.iMobileAuth);
      dbCommand.Parameters.AddWithValue("@VoiceProd", oCS.iVoiceProd);
      dbCommand.Parameters.AddWithValue("@SysID", oCS.sSysID);
      dbCommand.Parameters.AddWithValue("@Install_Date",
                                   getSQLAppropriateDateTime(oCS.dtInstallDt));
      dbCommand.Parameters.AddWithValue("@Support_Expires",
                                   getSQLAppropriateDateTime(oCS.dtSupportExp));
      dbCommand.Parameters.AddWithValue("@ReleaseNo", oCS.fReleaseNo);
      dbCommand.Parameters.AddWithValue("@Sales", oCS.sSales);
      dbCommand.Parameters.AddWithValue("@Comments", oCS.sComments);
      dbCommand.Parameters.AddWithValue("@Install_Cnt", oCS.sInstallCnt);
      dbCommand.Parameters.AddWithValue("@Service_Time", oCS.iServiceTime);
      dbCommand.Parameters.AddWithValue("@Trial_Time", oCS.iTrialTime);
      dbCommand.Parameters.AddWithValue("@SM_Enabled", oCS.bSMEnabled);
      dbCommand.Parameters.AddWithValue("@CreditHold", oCS.bCreditHold);
      dbCommand.Parameters.AddWithValue("@Xpress", oCS.bXpress);
      dbCommand.Parameters.AddWithValue("@JDE_Enabled", oCS.bJDEEnabled);
      dbCommand.Parameters.AddWithValue("@SAP_Enabled", oCS.bSAPEnabled);
      dbCommand.Parameters.AddWithValue("@Oracle_Enabled", oCS.bOracleEnabled);
      dbCommand.Parameters.AddWithValue("@Costpoint_Enabled",
                                        oCS.bCostpointEnabled);
      dbCommand.Parameters.AddWithValue("@MS_Enabled", oCS.bMSEnabled);
      dbCommand.Parameters.AddWithValue("@Backup", oCS.bBackup);
      dbCommand.Parameters.AddWithValue("@Test", oCS.bTest);
      dbCommand.Parameters.AddWithValue("@Lapse", oCS.bLapse);
      dbCommand.Parameters.AddWithValue("@Service_Enabled",
                                        oCS.bServiceEnabled);
      dbCommand.Parameters.AddWithValue("@Trial_Enabled", oCS.bTrialEnabled);
      dbCommand.Parameters.AddWithValue("@SerialID", oCS.sSerialID);
      dbCommand.Parameters.AddWithValue("@Vocollect", oCS.bVocollect);
      dbCommand.Parameters.AddWithValue("@Server_Auth", oCS.iServerAuth);
      dbCommand.Parameters.AddWithValue("@SAPB1_Enabled", oCS.bSAPB1Enabled);
      dbCommand.Parameters.AddWithValue("@Apps_Auth", oCS.iAppsAuth);
      dbCommand.Parameters.AddWithValue("@Server_Cost", oCS.sServerCost);
      dbCommand.Parameters.AddWithValue("@UsersAllowed", oCS.iUsersAllowed);
      dbCommand.Parameters.AddWithValue("@ServerNotes", oCS.sServerNotes);
      dbCommand.Parameters.AddWithValue("@Obsolete", oCS.bObsolete);
      dbCommand.Parameters.AddWithValue("@Contract_Start",
                                 getSQLAppropriateDateTime(oCS.dtContractSt));
      dbCommand.Parameters.AddWithValue("@Contract_End",
                                 getSQLAppropriateDateTime(oCS.dtContractEd));
      dbCommand.Parameters.AddWithValue("@Service_Expire",
                                     getSQLAppropriateDateTime(oCS.dtSvcExp));
      dbCommand.Parameters.AddWithValue("@HeartBeat",
                                  getSQLAppropriateDateTime(oCS.dtHeartbeat));
      dbCommand.Parameters.AddWithValue("@MinUsers", oCS.iMinUsers);
      dbCommand.Parameters.AddWithValue("@SCM_Enabled", oCS.bSCMEnabled);
      dbCommand.Parameters.AddWithValue("@HCP_Enabled", oCS.bHCPEnabled);
      dbCommand.Parameters.AddWithValue("@BCS_Cnt", oCS.iBCSCount);
      dbCommand.Parameters.AddWithValue("@RFUser", oCS.sRFUser);

      if (DBHandle.NonQuery(dbCommand) == -1) {
        Logger.Error("DB Update Error.", Environment.StackTrace);
        return false;
      }

      return true;
    }

    public static bool Delete(string sRFUser) { //update the obsolete column to true anytime a customer tuple is "deleted".
      string sUpdateText = "UPDATE CustomerInfo " +              
                           "SET OBSOLETE = 1" +
                           "WHERE (RFUser=@RFUser);";
      SqlCommand dbCommand = new SqlCommand(sUpdateText, DBHandle.mdbHandle); 
      dbCommand.Parameters.AddWithValue("@RFUser", sRFUser);

      if (DBHandle.NonQuery(dbCommand) != 0) {
        Logger.Error("DB Update Error on Obsolete val.", Environment.StackTrace);
        return false;
      }
      return true;
    }
    
    private static string getSQLAppropriateDateTime(DateTime dtInput) {
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
