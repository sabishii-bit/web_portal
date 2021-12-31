using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RFGenLicensingPortal.Models.Tests
{
  [TestClass()]
  public class CustomerInfoTests
  {
    private void InsertTest() {
      List<CustomerInfoData> oCustomers = new List<CustomerInfoData>();
      for (int i = 0; i < 10; i++) {
        CustomerInfoData oTestCustomer = new();
        oTestCustomer.sRFUser = "UNIT TEST " + i.ToString();
        oTestCustomer.sAllocGrp = "JimGroup";
        oTestCustomer.sVAR = "TestSvar";
        oTestCustomer.sCustomer = "Fred";
        oTestCustomer.iUsersAuth = 15;
        oTestCustomer.iMobileAuth = 10;
        oTestCustomer.iVoiceProd = 11;
        oTestCustomer.sSysID = "abcde";
        oTestCustomer.dtInstallDt = new DateTime(2020, 05, 15);
        oTestCustomer.dtSupportExp = new DateTime(2020, 05, 15);
        oTestCustomer.fReleaseNo = 4.17f;
        oTestCustomer.sSales = "Linda Thomas";
        oTestCustomer.sComments = "This is a sentense containing words. This is another sentense.";
        oTestCustomer.sInstallCnt = "Why is this a string?";
        oTestCustomer.iServiceTime = 19;
        oTestCustomer.iTrialTime = 12;
        oTestCustomer.bSMEnabled = true;
        oTestCustomer.bCreditHold = false;
        oTestCustomer.bXpress = true;
        oTestCustomer.bJDEEnabled = false;
        oTestCustomer.bSAPEnabled = true;
        oTestCustomer.bOracleEnabled = false;
        oTestCustomer.bCostpointEnabled = true;
        oTestCustomer.bMSEnabled = false;
        oTestCustomer.bBackup = true;
        oTestCustomer.bTest = false;
        oTestCustomer.bLapse = true;
        oTestCustomer.bServiceEnabled = false;
        oTestCustomer.bTrialEnabled = true;
        oTestCustomer.sSerialID = "defg";
        oTestCustomer.bVocollect = true;
        oTestCustomer.iServerAuth = 10;
        oTestCustomer.bSAPB1Enabled = false;
        oTestCustomer.iAppsAuth = 21;
        oTestCustomer.sAcctID = "efgh";
        oTestCustomer.sServerCost = "fghi";
        oTestCustomer.sUserCost = "ghij";
        oTestCustomer.sMobileCost = "hijk";
        oTestCustomer.iUsersAllowed = 22;
        oTestCustomer.sServerNotes = "I am typing more words";
        oTestCustomer.bObsolete = false;
        oTestCustomer.dtContractSt = new DateTime(2020, 05, 15);
        oTestCustomer.dtContractEd = new DateTime(2020, 05, 15);
        oTestCustomer.dtSvcExp = new DateTime(2020, 05, 15);
        oTestCustomer.dtHeartbeat = new DateTime(2020, 05, 15);
        oTestCustomer.iMinUsers = 23;
        oTestCustomer.bSCMEnabled = false;
        oTestCustomer.bHCPEnabled = true;
        oTestCustomer.iBCSCount = 24;
        oCustomers.Add(oTestCustomer);
      }

      try
      {
        foreach (CustomerInfoData oCustomer in oCustomers) {
          if (CustomerInfo.Insert(oCustomer) == false) {
            Assert.Fail("Failed to insert into CustomerInfo.");
          }
        }
      } catch (Exception e)
      {
          Assert.Fail(e.Message);
      }
    }

    private void UpdateTest() {
      List<CustomerInfoData> oCustomers = new List<CustomerInfoData>();
      for (int i = 0; i < 10; i++) {
        CustomerInfoData oTestCustomer = new();
        oTestCustomer.sRFUser = "UNIT TEST " + i.ToString();
        oTestCustomer.sAllocGrp = "JimGroup";
        oTestCustomer.sVAR = "TestSvar";
        oTestCustomer.sCustomer = "Fred";
        oTestCustomer.iUsersAuth = 15;
        oTestCustomer.iMobileAuth = 10;
        oTestCustomer.iVoiceProd = 11;
        oTestCustomer.sSysID = "abcde";
        oTestCustomer.dtInstallDt = new DateTime(2020, 05, 15);
        oTestCustomer.dtSupportExp = new DateTime(2020, 05, 15);
        oTestCustomer.fReleaseNo = 4.17f;
        oTestCustomer.sSales = "Linda Thomas";
        oTestCustomer.sComments = "This is a sentense containing words. This is another sentense.";
        oTestCustomer.sInstallCnt = "Why is this a string?";
        oTestCustomer.iServiceTime = 19;
        oTestCustomer.iTrialTime = 12;
        oTestCustomer.bSMEnabled = true;
        oTestCustomer.bCreditHold = false;
        oTestCustomer.bXpress = true;
        oTestCustomer.bJDEEnabled = false;
        oTestCustomer.bSAPEnabled = true;
        oTestCustomer.bOracleEnabled = false;
        oTestCustomer.bCostpointEnabled = true;
        oTestCustomer.bMSEnabled = false;
        oTestCustomer.bBackup = true;
        oTestCustomer.bTest = false;
        oTestCustomer.bLapse = true;
        oTestCustomer.bServiceEnabled = false;
        oTestCustomer.bTrialEnabled = true;
        oTestCustomer.sSerialID = "defg";
        oTestCustomer.bVocollect = true;
        oTestCustomer.iServerAuth = 10;
        oTestCustomer.bSAPB1Enabled = false;
        oTestCustomer.iAppsAuth = 21;
        oTestCustomer.sAcctID = "efgh";
        oTestCustomer.sServerCost = "fghi";
        oTestCustomer.sUserCost = "ghij";
        oTestCustomer.sMobileCost = "hijk";
        oTestCustomer.iUsersAllowed = 22;
        oTestCustomer.sServerNotes = "I am typing more words";
        oTestCustomer.bObsolete = false;
        oTestCustomer.dtContractSt = new DateTime(2020, 05, 15);
        oTestCustomer.dtContractEd = new DateTime(2020, 05, 15);
        oTestCustomer.dtSvcExp = new DateTime(2020, 05, 15);
        oTestCustomer.dtHeartbeat = new DateTime(2020, 05, 15);
        oTestCustomer.iMinUsers = 23;
        oTestCustomer.bSCMEnabled = false;
        oTestCustomer.bHCPEnabled = true;
        oTestCustomer.iBCSCount = 24;
        oCustomers.Add(oTestCustomer);
      }

      try {
        foreach (CustomerInfoData oCustomer in oCustomers) {
          if (CustomerInfo.Update(oCustomer) == false) {
            Assert.Fail("Failed to update CustomerInfo.");
          }
          else {
            CustomerInfoData oCurr = CustomerInfo.GetRecord(oCustomer.sRFUser);
            if(oCurr.sCustomer != "Fred") {
              Assert.Fail("Value changed when Update called with same " +
                          "values.");
            }
          }
          oCustomer.sCustomer = "Bob";
          if (CustomerInfo.Update(oCustomer) == false) {
            Assert.Fail("Failed to update CustomerInfo.");
          }
          else {
            CustomerInfoData oCurr = CustomerInfo.GetRecord(oCustomer.sRFUser);
            if(oCurr.sCustomer != "Bob") {
              Assert.Fail("Value not changed correctly when Update called " +
                          "with different values");
            }
          }
        }
      }
      catch (Exception e) {
        Assert.Fail(e.Message);
      }
    }

    private void CountTest() {
      int iCount = 0;
      try {
        iCount = CustomerInfo.Count("UNIT TEST");
      } catch (Exception e) {
        Assert.Fail(e.Message);
      }

      if (iCount != 10) {
        Assert.Fail("CustomerInfo.Count returned " + iCount.ToString());
      }
    }

    private void GetRecordTest() {
      try {
        for (int i = 0; i < 10; i++) {
          String sRFUser = "UNIT TEST " + i.ToString();
          CustomerInfoData oData = CustomerInfo.GetRecord(sRFUser);
          if (oData.sRFUser != sRFUser) {
            Assert.Fail("Failed to get record: " + sRFUser);
          }
        }
      } catch (Exception e) {
        Assert.Fail(e.Message);
      }
    }

    private void ListTest() {
      try {
        List<CustomerInfoData> oShortPage1;
        List<CustomerInfoData> oShortPage2;
        List<CustomerInfoData> oLongPage;
        oShortPage1 = CustomerInfo.List(5, 1, "UNIT TEST");
        oShortPage2 = CustomerInfo.List(5, 2, "UNIT TEST");
        oLongPage = CustomerInfo.List(10, 1, "UNIT TEST");

        if (oShortPage1.Count != 5) {
          Assert.Fail("Incorrect number of results.");
        } else if (oShortPage1[0].sRFUser != "UNIT TEST 0" ||
                   oShortPage1[4].sRFUser != "UNIT TEST 4") {
          Assert.Fail("Incorrect RFUser in List results");
        }

        if (oShortPage2.Count != 5) {
          Assert.Fail("Incorrect number of results.");
        }  else if (oShortPage2[0].sRFUser != "UNIT TEST 5" ||
                    oShortPage2[4].sRFUser != "UNIT TEST 9") {
          Assert.Fail("Incorrect RFUser in List results");
        }

        if (oLongPage.Count != 10){
          Assert.Fail("Incorrect number of results.");
        } else if (oLongPage[0].sRFUser != "UNIT TEST 0" ||
                   oLongPage[9].sRFUser != "UNIT TEST 9") {
          Assert.Fail("Incorrect RFUser in List results");
        }
      } catch (Exception e) {
        Assert.Fail(e.Message);
      }
    }

    private void CleanUnitTestData() {
      string sCommandText = "DELETE FROM CustomerInfo WHERE RFUser LIKE " +
                            "'UNIT TEST%';";
      try {
        using (SqlCommand dbCommand = new SqlCommand(sCommandText,
                                                     DBHandle.mdbHandle)) {
          dbCommand.ExecuteNonQuery();
        }
      } catch (Exception e) {
        Assert.Fail("Clean up failed.");
      }
    }

    [TestMethod()]
    public void CustomerInfoTest()
    {
      CleanUnitTestData();
      InsertTest();
      CountTest();
      GetRecordTest();
      UpdateTest();
      ListTest();
      CleanUnitTestData();
    }
  }
}