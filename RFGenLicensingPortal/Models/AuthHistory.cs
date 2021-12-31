using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RFGenLicensingPortal.Models {
  public static class AuthHistory {
    //Returns single line from AuthHistory table
    public static AuthHistoryData Query(long lKey) //parameters MUST reference primary keys for searching purposes
    {
      AuthHistoryData oRecord;

      string sCommandText = "SELECT * FROM AUTHHISTORY WHERE [KEY] = @Key;";
      SqlCommand dbCommand = new SqlCommand(sCommandText, DBHandle.mdbHandle);
      dbCommand.Parameters.AddWithValue("@Key", lKey);
      SqlDataReader oReader = DBHandle.Query(dbCommand);
      if (oReader == null) return null;

      try {
        if (oReader.Read())
          oRecord = new AuthHistoryData(oReader);
        else
          oRecord = null;

        oReader.Close();
        return oRecord;
      } catch(Exception e) {
        Logger.Error("Query Error: " + e.Message, Environment.StackTrace);
        try { oReader.Close(); } catch(Exception f) { };
        return null;
      }
    }

    //returns list of up to 50 lines from AuthHistory table
    // param 1: how many record? param2: what page? EX: iCnt = 50, iPg = 2 gives you 50-100
    public static List<AuthHistoryData> List(int iCnt, int iPg, string sRFUser)
    {
      List<AuthHistoryData> oRecordList = new List<AuthHistoryData>();

      //Get records for database
      string sCommandText = "SELECT * FROM AUTHHISTORY WHERE RFUSER = " +
                            "@RFUser ORDER BY [KEY] OFFSET @Offset ROWS " +
                            "FETCH NEXT @Rows ROWS ONLY;";

      SqlCommand dbCommand = new SqlCommand(sCommandText, DBHandle.mdbHandle);
      dbCommand.Parameters.AddWithValue("@RFUser", sRFUser);
      dbCommand.Parameters.AddWithValue("@Offset", (iCnt * (iPg - 1)));
      dbCommand.Parameters.AddWithValue("@Rows", iCnt);
      SqlDataReader oReader = DBHandle.Query(dbCommand);

      //If no results return empty list
      if (oReader == null) return oRecordList;

      try {
        // add records to list
        while (oReader.Read()) oRecordList.Add(new AuthHistoryData(oReader));

        oReader.Close();
        return oRecordList;
      } catch (Exception e) {
        Logger.Error("Get List Error: " + e.Message, Environment.StackTrace);
        try { oReader.Close(); } catch (Exception f) { };
        return null;
      }
    }

    public static int Count() {
      string sCommandText = "SELECT COUNT(*) FROM AUTHHISTORY;";
      SqlCommand dbCommand = new SqlCommand(sCommandText, DBHandle.mdbHandle);
      SqlDataReader oReader = DBHandle.Query(dbCommand);

      if (oReader == null) return 0;

      try {
        oReader.Read();

        //giving count to int var since oReader needs to close before returning
        int iCount = oReader.GetInt32(0);
        oReader.Close();
        return iCount;
      } catch (Exception e) {
        Logger.Error("Count Error: " + e.Message, Environment.StackTrace);
        try { oReader.Close(); } catch (Exception f) { };
        return -1;
      }
    }
  }
}


