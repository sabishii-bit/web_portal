using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RFGenLicensingPortal.Models {
  public static class HDEvents {
    //made a page selector because of tons of possible columns in HDEvents
    public static List<HDEventsData> List(string sRFUser) {
      List<HDEventsData> oEventList = new List<HDEventsData>();

      // Ordering data retrieved from database by most recent date
      string sCommandText = "SELECT * FROM HDEvents WHERE RFUSER = @RFUser " +
                            "ORDER BY [TimeStamp] DESC;";

      SqlCommand dbCommand = new SqlCommand(sCommandText, DBHandle.mdbHandle);
      dbCommand.Parameters.AddWithValue("@RFUser", sRFUser);
      SqlDataReader oReader = DBHandle.Query(dbCommand);

      //If no results return empty list
      if (oReader == null) return oEventList;

      try {
        // add records to list
        while (oReader.Read()) oEventList.Add(new HDEventsData(oReader));

        oReader.Close();
        return oEventList;
      } catch (Exception e) {
        Logger.Error("List Error: " + e.Message, Environment.StackTrace);
        try { oReader.Close(); } catch (Exception f) { };
        return null;
      }
    }

    public static int Count(string sRFUser) {
      string sCommandText = "SELECT COUNT(*) FROM HDEvents WHERE RFUSER = @RFUser;";
      SqlCommand dbCommand = new SqlCommand(sCommandText, DBHandle.mdbHandle);
      dbCommand.Parameters.AddWithValue("@RFUser", sRFUser);
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

    private static void Insert(string sRFUser, string sEvent) {
      //TODO: we need to get the user Session for this - Cody T
      string sUserID = "000";
      string sUserPC = "000";
      DateTime oTimeStamp = DateTime.Now;
      string sCommandText = "INSERT INTO HDEvents " +
                            "(RFUser, Event, UserID, UserPC, TimeStamp) " +
                            "VALUES (@RFUser, @Event, @UserId, @UserPC, " +
                            "@TimeStamp);";
      SqlCommand dbCommand = new SqlCommand(sCommandText, DBHandle.mdbHandle);
      dbCommand.Parameters.AddWithValue("@RFUser", sRFUser);
      dbCommand.Parameters.AddWithValue("@Event", sEvent);
      dbCommand.Parameters.AddWithValue("@UserId", sUserID);
      dbCommand.Parameters.AddWithValue("@UserPC", sUserPC);
      dbCommand.Parameters.AddWithValue("@TimeStamp", oTimeStamp);

      try {
        DBHandle.NonQuery(dbCommand);
      } catch(Exception e) {
        Logger.Error("Insert Error: " + e.Message, Environment.StackTrace);
        return;
      }
    }

    public static void NewRecord(string sRFUser) {
      Insert(sRFUser, "New Record Added.");
    }

    public static void DeleteRecord(string sRFUser) {
      Insert(sRFUser, "Deleted the Record.");
    }

    public static void EditRecord(string sRFUser, string sField,
                                  string sOldValue, string sNewValue) {
      Insert(sRFUser, sField + ":  " + sOldValue + "-->" + sNewValue);
    }
  }
}
