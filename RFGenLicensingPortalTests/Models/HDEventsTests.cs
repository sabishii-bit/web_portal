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
  public class HDEventsTests
  {
    private void CleanUnitTestData()
    {
      string sCommandText = "DELETE FROM HDEvents WHERE RFUser = " +
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
          HDEvents.NewRecord("UNIT TEST");
        }
        for (int i = 0; i < 10; i++) {
          HDEvents.DeleteRecord("UNIT TEST");
        }
        for (int i = 0; i < 10; i++) {
          HDEvents.EditRecord("UNIT TEST", "UNIT TEST", "UNIT", "TEST");
        }
      } catch (Exception e) {
        Assert.Fail(e.Message);
      }
    }

    private void CountTest() {
      try {
        if (HDEvents.Count("UNIT TEST") != 30) {
          Assert.Fail("Incorrect count from HDEvents.");
        }
      } catch (Exception e) {
        Assert.Fail(e.Message);
      }
    }

    private void ListTest()
    {
      try
      {
        List<HDEventsData> oList;
        oList = HDEvents.List("UNIT TEST");

        if (oList.Count != 30)
        {
          Assert.Fail("Incorrect number of results.");
        }
        else if (oList[0].sRFUser != "UNIT TEST" ||
                 oList[29].sRFUser != "UNIT TEST")
        {
          Assert.Fail("Incorrect RFUser in List results");
        }
      }
      catch (Exception e)
      {
        Assert.Fail(e.Message);
      }
    }

    [TestMethod()]
    public void HDEventsTest()
    {
      CleanUnitTestData();
      InsertTest();
      ListTest();
      CleanUnitTestData();
    }
  }
}