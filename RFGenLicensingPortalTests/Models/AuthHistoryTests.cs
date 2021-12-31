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
  public class AuthHistoryTests
  {
    private void ListTest()
    {
      try
      {
        List<AuthHistoryData> oShortPage1;
        List<AuthHistoryData> oShortPage2;
        List<AuthHistoryData> oLongPage;
        oShortPage1 = AuthHistory.List(5, 1, "UNIT TEST");
        oShortPage2 = AuthHistory.List(5, 2, "UNIT TEST");
        oLongPage = AuthHistory.List(10, 1, "UNIT TEST");

        if (oShortPage1.Count != 5)
        {
          Assert.Fail("Incorrect number of results.");
        }
        else if (oShortPage1[0].sRFUser != "UNIT TEST" ||
                 oShortPage1[4].sRFUser != "UNIT TEST")
        {
          Assert.Fail("Incorrect RFUser in List results");
        }

        if (oShortPage2.Count != 5)
        {
          Assert.Fail("Incorrect number of results.");
        }
        else if (oShortPage2[0].sRFUser != "UNIT TEST" ||
                 oShortPage2[4].sRFUser != "UNIT TEST")
        {
          Assert.Fail("Incorrect RFUser in List results");
        }

        if (oLongPage.Count != 10)
        {
          Assert.Fail("Incorrect number of results.");
        }
        else if (oLongPage[0].sRFUser != "UNIT TEST" ||
                 oLongPage[9].sRFUser != "UNIT TEST")
        {
          Assert.Fail("Incorrect RFUser in List results");
        }
      }
      catch (Exception e)
      {
        Assert.Fail(e.Message);
      }
    }

    private void GenerateUnitTestData() {
      string sCommandText = "INSERT INTO AuthHistory (RFUser) " +
                            "VALUES ('UNIT TEST');";
      try
      {
        using (SqlCommand dbCommand = new SqlCommand(sCommandText,
                                                     DBHandle.mdbHandle))
        {
          for (int i = 0; i < 10; i++) {
            dbCommand.ExecuteNonQuery();
          }
        }
      }
      catch (Exception e)
      {
        Assert.Fail("Generating AuthHistory data failed.");
      }
    }

    private void CleanUnitTestData() {
      string sCommandText = "DELETE FROM AuthHistory WHERE RFUser = " +
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

    [TestMethod()]
    public void AuthHistoryTest() {
      DBHandle.Init();
      CleanUnitTestData();
      GenerateUnitTestData();
      ListTest();
      CleanUnitTestData();
    }
  }
}