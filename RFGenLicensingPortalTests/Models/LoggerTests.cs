using Microsoft.VisualStudio.TestTools.UnitTesting;
using RFGenLicensingPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace RFGenLicensingPortal.Models.Tests
{
  [TestClass()]
  public class LoggerTests
  {
    private void CleanUnitTestData()
    {
      string sCommandText = "DELETE FROM Logs WHERE Description = " +
                            "'UNIT TEST';";
      try
      {
        using (SqlCommand dbCommand = new SqlCommand(sCommandText,
                                                     DBHandle.mdbHandle))
        {
          dbCommand.ExecuteNonQuery();
        }
      }
      catch (Exception e)
      {
        Assert.Fail("Clean up failed.");
      }
    }

    private void InsertTest() {
      try {
        for (int i = 0; i < 10; i++) {
          Logger.Log("UNIT TEST", Environment.StackTrace);
        }
        for (int i = 0; i < 10; i++) {
          Logger.Error("UNIT TEST", Environment.StackTrace);
        }
      }
      catch (Exception e)
      {
        Assert.Fail(e.Message);
      }
    }

    private void GetLogsTest() {
      try {
        List<LogItem> oLogs = Logger.GetLogs();
        if (oLogs.Count < 30) {
          Assert.Fail("Log count is too low.");
        }
        oLogs = Logger.GetLogs(new LogFilter(sUser:"UNIT TESTER"));
        if (oLogs.Count != 10) {
          Assert.Fail("Log count is incorrect with filter UNIT TESTER");
        }
        oLogs = Logger.GetLogs(new LogFilter(sUser:"UNIT TESTER1"));
        if (oLogs.Count != 5) {
          Assert.Fail("Log count is incorrect with filter UNIT TESTER1");
        }
        oLogs = Logger.GetLogs(new LogFilter(sUser:"UNIT TESTER",
                                             bErrorsOnly:true));
        if (oLogs.Count != 5) {
          Assert.Fail("Log count is incorrect with filter errors only");
        }
        oLogs = Logger.GetLogs(new LogFilter(sUser:"UNIT TESTER",
                                      oStartingDate:new DateTime(1776, 7, 4)));
        if (oLogs.Count != 4) {
          Assert.Fail("Log count is incorrect with filter start date");
        }
        oLogs = Logger.GetLogs(new LogFilter(sUser:"UNIT TESTER",
                                        oEndingDate:new DateTime(1776, 7, 4)));
        if (oLogs.Count != 8) {
          Assert.Fail("Log count is incorrect with filter end date");
        }
        oLogs = Logger.GetLogs(new LogFilter(sUser:"UNIT TESTER",
                                       oStartingDate: new DateTime(1776, 7, 4),
                                       oEndingDate: new DateTime(1776, 7, 4)));
        if (oLogs.Count != 2) {
          Assert.Fail("Log count is incorrect with filter start date and end " +
                      "date");
        }
      }
      catch (Exception e) {
        Assert.Fail(e.Message);
      }
    }

    private void CreateLog(string sUser, Boolean bIsError, DateTime oTimeStamp) {
      string sCommandText = "INSERT INTO Logs (UserId, IsError, TimeStamp, " +
                            "Description, StackTrace) VALUES (@UserID, " +
                            "@IsError, @TimeStamp, @Description, @StackTrace);";
      try {
        using (SqlCommand dbCommand = new SqlCommand(sCommandText,
                                                     DBHandle.mdbHandle)) {
          dbCommand.Parameters.AddWithValue("@UserId", sUser);
          dbCommand.Parameters.AddWithValue("@IsError", bIsError);
          dbCommand.Parameters.AddWithValue("@TimeStamp", oTimeStamp);
          dbCommand.Parameters.AddWithValue("@Description", "UNIT TEST");
          dbCommand.Parameters.AddWithValue("@StackTrace", "UNIT TEST");
          dbCommand.ExecuteNonQuery();
        }
      }
      catch (Exception e) {
        Assert.Fail("Create log failed.");
      }
    }

    private void GenerateCustomLogs() {
      CreateLog("UNIT TESTER1", false, new DateTime(1776, 7, 1, 0, 0, 0));
      CreateLog("UNIT TESTER1", false, new DateTime(1776, 7, 2, 0, 0, 0));
      CreateLog("UNIT TESTER1", false, new DateTime(1776, 7, 3, 0, 0, 0));
      CreateLog("UNIT TESTER1", false, new DateTime(1776, 7, 4, 0, 0, 0));
      CreateLog("UNIT TESTER1", false, new DateTime(1776, 7, 5, 0, 0, 0));
      CreateLog("UNIT TESTER2", true, new DateTime(1776, 7, 1, 0, 0, 0));
      CreateLog("UNIT TESTER2", true, new DateTime(1776, 7, 2, 0, 0, 0));
      CreateLog("UNIT TESTER2", true, new DateTime(1776, 7, 3, 0, 0, 0));
      CreateLog("UNIT TESTER2", true, new DateTime(1776, 7, 4, 0, 0, 0));
      CreateLog("UNIT TESTER2", true, new DateTime(1776, 7, 5, 0, 0, 0));
    }

    [TestMethod()]
    public void LoggerTest()
    {
      CleanUnitTestData();
      InsertTest();
      GenerateCustomLogs();
      GetLogsTest();
      CleanUnitTestData();
    }
  }
}