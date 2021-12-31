using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RFGenLicensingPortal.Models {
  public static class Logger {
    public static string msUser { get; set;} 

    static Logger() { }
    private static void Insert(string sDescription, string sStackTrace,
                               Boolean bIsError) {
      string sCommandText = "INSERT INTO Logs (UserId, IsError, TimeStamp, " +
                            "Description, StackTrace) VALUES (@UserID, " +
                            "@IsError, @TimeStamp, @Description, @StackTrace);";

      SqlCommand dbCommand = new SqlCommand(sCommandText, DBHandle.mdbHandle);
      if (msUser == null) msUser = "N/A";
      dbCommand.Parameters.AddWithValue("@UserId", msUser);
      dbCommand.Parameters.AddWithValue("@IsError", bIsError);
      dbCommand.Parameters.AddWithValue("@TimeStamp", DateTime.Now);
      dbCommand.Parameters.AddWithValue("@Description", sDescription);
      dbCommand.Parameters.AddWithValue("@StackTrace", sStackTrace);

      DBHandle.NonQuery(dbCommand);
    }

    public static void Log(string sDescription, string sStackTrace) {
      Insert(sDescription, sStackTrace, false);
    }

    public static void Error(string sDescription, string sStackTrace) {
      Insert(sDescription, sStackTrace, true);
    }

    public static List<LogItem> GetLogs(LogFilter oFilter = null) {
      if (oFilter == null) oFilter = new LogFilter();
      List<LogItem> oLogs = new List<LogItem>();

      //retrieve logs from db
      string sCommandText = "SELECT * FROM Logs WHERE TimeStamp BETWEEN " +
                            "@StartingDate AND @EndingDate AND " +
                            "UserID LIKE @UserID AND (IsError = @IsError " +
                            "OR IsError = 1);";
      SqlCommand dbCommand = new SqlCommand(sCommandText, DBHandle.mdbHandle);
      dbCommand.Parameters.AddWithValue("@StartingDate",
                                        oFilter.moStartingDate);
      dbCommand.Parameters.AddWithValue("@EndingDate", oFilter.moEndingDate);
      dbCommand.Parameters.AddWithValue("@UserID", "%" + oFilter.msUser + "%");
      dbCommand.Parameters.AddWithValue("@IsError", oFilter.mbErrorsOnly);
      SqlDataReader oReader = DBHandle.Query(dbCommand);

      //If no results return empty list
      if (oReader == null) return oLogs;

      // add LogItems to list
      while (oReader.Read()) {
        oLogs.Add(new LogItem(oReader));
      }

      oReader.Close();
      return oLogs;
    }
  }
}
