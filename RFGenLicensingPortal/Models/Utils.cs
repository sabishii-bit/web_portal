using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Configuration;
using Salesforce.Common;
using Salesforce.Force;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;
using Salesforce.Common.Models.Json;
using Microsoft.Extensions.Configuration;

namespace RFGenLicensingPortal.Models {

  public static class SF_FieldType {
    public const string RFUser = "Name";
    public const string Account_ID = "AcctID__c";
    public const string Users_Auth = "Users_Auth__c";
    public const string MobUsers_Auth = "Mobile_Auth__c";
    public const string BCSCnt = "BCSCnt__c";

    public const string ServerNotes = "ServerNotes__c";
    public const string Apps_Auth = "Apps_Auth__c";
    public const string Users_Allowed = "Users_Allowed__c";
    public const string Voice_Prod = "VoiceProd__c";
    public const string Install_Date = "Install_Date__c";
    public const string Support_Expiry = "Support_Expires__c";

    public const string Release_No = "ReleaseNo__c";
    public const string Service_Time = "Service_Time__c";
    public const string Trail_Time = "Trial_Time__c";
    public const string isSM = "SM_Enabled__c";
    public const string isXpress = "Xpress__c";

    public const string isJDE = "JDE_Enabled__c";
    public const string isSAP = "SAP_Enabled__c";
    public const string isEBS = "Oracle_Enabled__c";
    public const string isCostPoint = "Costpoint_Enabled__c";
    public const string isMS = "MS_Enabled__c";

    public const string isSAPB1 = "SAPB1_Enabled__c";
    public const string isVocollect = "Vocollect__c";
    public const string isBackup = "Backup__c";
    public const string isTest = "Test__c";
    public const string isLapse = "Lapse__c";

    public const string isService = "Service_Enabled__c";
    public const string isTrial = "Trial_Enabled__c";
    public const string Serial_ID = "SerialID__c";
    public const string Server_Cost = "Server_Cost__c";
    public const string User_Cost = "User_Cost__c";

    public const string Mobile_Cost = "Mobile_Cost__c";
    public const string Contract_Start = "Contract_Start__c";
    public const string Contract_End = "Contract_End__c";
    public const string Service_Expiry = "Service_Expire__c";
  }

  public class SF_FieldType_Account {
    public string Account_ID { get; set; }
    public string Account_Name { get; set; }
  }

  public static class Utils {

    public static int BoolToInt(bool bVal) {
      return bVal ? 1 : 0;
    }

    private static string msLastError;
    private static string msStackTrace;

    public static bool Authorize(CustomerInfoData oCS, string sParam) {
      //TODO: Move into Web.Config
      string sURL = "http://portal.rfgen.com/WebAuthXTest.asp";
      string sRsp = "";
      string sAuthCode = "";

      if (oCS == null) {
        Logger.Error("Empty CustomerInfoData object while authorizing.", Environment.StackTrace);
        return false;
      }

      try {
        sURL += "?custid=" + oCS + "&serno=" + oCS.sSerialID + "&sysid=" + sParam + "&HD=Y";

        HttpWebRequest oRequest = (HttpWebRequest)WebRequest.Create(sURL);
        HttpWebResponse oResponse = (HttpWebResponse)oRequest.GetResponse();

        System.IO.Stream oStream = oResponse.GetResponseStream();
        StreamReader oReader = new StreamReader(oStream, null); //Request.ContentEncoding

        sRsp = oReader.ReadToEnd();
        sRsp = sRsp.Replace("\r", "");
        string[] sLines = sRsp.Split('\n');

        foreach (string sLine in sLines) {
          System.Diagnostics.Debug.Print(sLine);
          if (sLine.Contains("AUTHCODE=")) {
            sAuthCode = sLine;
            ProcessAuthCode(oCS, sAuthCode);
            break;
          } else if (sLine.Contains("ERR=") || sLine.Contains("ERR:")) {
            //pass error back
            break;
          }
        }

        return true;
      } catch (Exception e) {
        //Log
        return false;
      }
    }

    private static void ProcessAuthCode(CustomerInfoData oCS, string sAuthCode) {
      string sNewAuthCode = "";
      string sMatchStr = "AUTHCODE=";
      string sPath = "~/App_Resource/Certs/";
      string sAuthFilePath = "certfile.cert";

      sNewAuthCode = sAuthCode.Substring(sAuthCode.LastIndexOf(sMatchStr) + sMatchStr.Length - 1).Trim();
      //TODO: store new authcode

      if (sNewAuthCode == "") {
        //trace log, not error
        //send back alert
      } else {
        if (/*StringUtils.getVersion(txtSysID.Text).ToSafeInt32() >= 50*/ true) {
          byte[] bCertContent = new byte[(sNewAuthCode.Length / 2) - 1];

          bCertContent = Enumerable.Range(0, sNewAuthCode.Length)
           .Where(x => x % 2 == 0)
           .Select(x => Convert.ToByte(sNewAuthCode.Substring(x, 2), 16))
           .ToArray();

          //AuthHistory.Insert(oCS);

          if (true /*Downloadable?*/) {
            try {
              //indicate download on front end

              //TODO: Do we want to deliver a message to the frontend?
              //txtRfgenKey.Text = "Certificate Downloaded";

              //TODO: Test on site
              //Response.Clear();
              //Response.Buffer = true;
              //Response.AppendHeader("Content-disposition", "attachment; filename=RFgen.cert");
              //Response.ContentType = "application/octet-stream";
              //Response.BinaryWrite(certContent);
              //Response.Flush();
              //Response.End();
            } catch (Exception e) {
              //log
            }
          } else {
            Directory.CreateDirectory(sPath);

            //create path
            try {
              using (FileStream fOut = File.OpenWrite(sAuthFilePath)) {

                fOut.Write(bCertContent, 0, bCertContent.Length);
                fOut.Close();
              }
            } catch (Exception e) {
              //log this
            }
          }
        } else {
          // version < 5.0
          //AuthHistory.Insert(oCS);
          //send back to front end
        }
      }
    }

    public static class SF {
      private static Dictionary<string, object> CreateDict(CustomerInfoData oCS) {
        Dictionary<string, object> oDict = new Dictionary<string, object>();

        if(oCS == null) {
          return null;
        }

        oDict.Add(SF_FieldType.RFUser, oCS.sRFUser);
        oDict.Add(SF_FieldType.Account_ID, oCS.sAcctID);
        oDict.Add(SF_FieldType.Users_Auth, oCS.iUsersAuth);
        oDict.Add(SF_FieldType.MobUsers_Auth, oCS.iMobileAuth);
        oDict.Add(SF_FieldType.BCSCnt, ""); //TODO: What is this?
        oDict.Add(SF_FieldType.ServerNotes, oCS.sServerNotes);
        oDict.Add(SF_FieldType.Apps_Auth, oCS.iAppsAuth);
        oDict.Add(SF_FieldType.Users_Allowed, oCS.iUsersAllowed);
        oDict.Add(SF_FieldType.Voice_Prod, oCS.iVoiceProd);
        oDict.Add(SF_FieldType.Install_Date, oCS.dtInstallDt);
        oDict.Add(SF_FieldType.Release_No, oCS.fReleaseNo);
        oDict.Add(SF_FieldType.Service_Time, oCS.iServiceTime); //TODO: Verify
        oDict.Add(SF_FieldType.Trail_Time, oCS.iTrialTime);
        oDict.Add(SF_FieldType.isSM, oCS.bSMEnabled);
        oDict.Add(SF_FieldType.isXpress, oCS.bXpress);
        oDict.Add(SF_FieldType.isJDE, oCS.bJDEEnabled);
        oDict.Add(SF_FieldType.isSAP, oCS.bSAPEnabled);
        oDict.Add(SF_FieldType.isEBS, oCS.bOracleEnabled);
        oDict.Add(SF_FieldType.isCostPoint, oCS.bCostpointEnabled);
        oDict.Add(SF_FieldType.isMS, oCS.sRFUser);
        oDict.Add(SF_FieldType.isSAPB1, oCS.bSAPB1Enabled);
        oDict.Add(SF_FieldType.isVocollect, oCS.bVocollect);
        oDict.Add(SF_FieldType.isBackup, oCS.bBackup);
        oDict.Add(SF_FieldType.isTest, oCS.bTest);
        oDict.Add(SF_FieldType.isLapse, oCS.bLapse);
        oDict.Add(SF_FieldType.isService, oCS.bServiceEnabled);
        oDict.Add(SF_FieldType.isTrial, oCS.bTrialEnabled);
        oDict.Add(SF_FieldType.Serial_ID, oCS.sSerialID);
        oDict.Add(SF_FieldType.Server_Cost, oCS.sServerCost);
        oDict.Add(SF_FieldType.User_Cost, oCS.sUserCost);
        oDict.Add(SF_FieldType.Mobile_Cost, oCS.sMobileCost);
        oDict.Add(SF_FieldType.Contract_Start, oCS.dtContractSt);
        oDict.Add(SF_FieldType.Contract_End, oCS.dtContractEd);
        oDict.Add(SF_FieldType.Service_Expiry, oCS.dtSvcExp);

        return oDict;
      }

      public static void CreateSF(string sRFUser, CustomerInfoData oCS) {
        //test env check
        try {
          ForceClient oClient = getSFInstance();
          Dictionary<string, object> oHD = CreateDict(oCS);

          Task<SuccessResponse> resp = oClient.CreateAsync("HelpDesk__c", oHD);
          resp.Wait();
        } catch(Exception e) {
          //log
        }

      }

      public static ForceClient getSFInstance() {
        ForceClient forceClient = null;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

        try {
          //string Username = ConfigurationManager.AppSettings["username"].ToSafeString();
          //string Password = ConfigurationManager.AppSettings["password"].ToSafeString();
          //string Token = ConfigurationManager.AppSettings["token"].ToSafeString();
          //string ClientId = ConfigurationManager.AppSettings["clientId"].ToSafeString();
          //string ClientSecret = ConfigurationManager.AppSettings["clientSecret"].ToSafeString();
          var oAuth = new AuthenticationClient();

          oAuth.UsernamePasswordAsync("ID", "Secret", "Username", "Password").Wait();

          forceClient = new ForceClient(oAuth.InstanceUrl, oAuth.AccessToken, oAuth.ApiVersion);
          return forceClient;
        } catch(Exception e) {
          //log
          return null;
        }
      }

      public static ArrayList getAllSalesmanList() {
        ArrayList salesPersonList = null;
        try {
          ForceClient oClient = getSFInstance();

          string sSQL = "select Name from User where IsActive = True and ProfileID IN" +
            "(select Id from Profile where (Name = 'Sales' or Name = 'Sales Manager' " +
            "or Name = 'Sales_Close_Opp'))";

          Task<QueryResult<JObject>> results = oClient.QueryAsync<JObject>(sSQL);

          List<JObject> jObjList = results.Result.Records;
          //JObject jObj;
          salesPersonList = new ArrayList();
          //for (int i = 0; i < jObjList.Count; i++){
          foreach (JObject jObj in results.Result.Records) {
            //jObj = jObjList[i];
            salesPersonList.Add(jObj["Name"].ToString());
          }

          salesPersonList.Add("House");

        } catch (Exception ex) {
          //Log
        }
        return salesPersonList;
      }

      public static string getSalesmanEmail(string sSalesman) {
        try {

          ForceClient client = getSFInstance();


          string sSQL = string.Format("select Email from User where Name = '{0}' and IsActive = True " +
            "and ProfileID IN(select Id from Profile where (Name = 'Sales' or Name = 'Sales" +
            " Manager'))", sSalesman);

          Task<QueryResult<dynamic>> results = client.QueryAsync<dynamic>(sSQL);

          var found = results.Result.Records.FirstOrDefault();

          if (found != null) {
            return (string)found.Email;

          } else {
            //Log
          }

        } catch (Exception ex) {
          //Log
        }
        return string.Empty;
      }

      //TODO: consider a bool return
      public static void updateSalesForce(string sRFUser, CustomerInfoData oCS) {
        try {
          //test env check

          ForceClient client = getSFInstance();

          Dictionary<string, object> oHD = CreateDict(oCS);

          Task<QueryResult<dynamic>> results = client.QueryAsync<dynamic>("SELECT  Id  FROM HelpDesk__c WHERE Name = '" + sRFUser + "'");

          var oFound = results.Result.Records.FirstOrDefault();

          if (oFound != null) {
            Task<SuccessResponse> rsp = client.UpdateAsync("HelpDesk__c", (string)oFound.Id, oHD);
            rsp.Wait();
          } else {
            //Log
          }

        } catch (Exception ex) {
          //log
        }
      }
    }
  }
}
