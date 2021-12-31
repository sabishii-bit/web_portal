using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RFGenLicensingPortal.Models;
using System;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Collections.Generic;

namespace RFGenLicensingPortal.Controllers {
  public class HomeController : Controller {
    private readonly ILogger<HomeController> _logger;
    private bool bIsInit = false;

    public void Validate_Creds(string sUser, string sPassword) {
      PrincipalContext oPC;
      UserPrincipal oUP;
      PrincipalSearchResult<Principal> oResult;

      try {
        oPC = new PrincipalContext(ContextType.Machine); //check for windows OS? - Dmitriy

        bIsInit = oPC.ValidateCredentials(sUser, sPassword);

        if (bIsInit) {
          oUP = UserPrincipal.FindByIdentity(oPC, sUser);
          HttpContext.Session.SetString("userName", oUP.Name);
          HttpContext.Session.SetString("userEmail", sUser);
          oResult = oUP.GetGroups();

          //TODO: clear cookies

          foreach (GroupPrincipal oGP in oResult) {
            switch (oGP.Name) {
              case "Administrators":
                HttpContext.Session.SetString("permHDOwner", "true");
                HttpContext.Session.SetString("permHDPowerUser", "true");
                HttpContext.Session.SetString("permHDGiveCodes", "false");
                HttpContext.Session.SetString("permHDSales", "false");
                break;
              case "hdPowerUser":
                HttpContext.Session.SetString("permHDOwner", "false");
                HttpContext.Session.SetString("permHDPowerUser", "true");
                HttpContext.Session.SetString("permHDGiveCodes", "false");
                HttpContext.Session.SetString("permHDSales", "false");
                break;
              case "hdGiveCodes":
                HttpContext.Session.SetString("permHDOwner", "false");
                HttpContext.Session.SetString("permHDPowerUser", "false");
                HttpContext.Session.SetString("permHDGiveCodes", "true");
                HttpContext.Session.SetString("permHDSales", "false");
                break;
              case "hdSales":
                HttpContext.Session.SetString("permHDOwner", "false");
                HttpContext.Session.SetString("permHDPowerUser", "false");
                HttpContext.Session.SetString("permHDGiveCodes", "false");
                HttpContext.Session.SetString("permHDSales", "true");
                break;
            }
          }          
          //indicate that the user is unauthorized         
        } else {
          //indicate wrong creds

          /////////////////////////////////
          ///////////TEST CODE/////////////
          /////////////////////////////////
          bIsInit = true;
          HttpContext.Session.SetString("permHDOwner", "false");    // false for permission test on client details
          HttpContext.Session.SetString("permHDPowerUser", "false");
          HttpContext.Session.SetString("permHDGiveCodes", "true");
          HttpContext.Session.SetString("permHDSales", "true");
          HttpContext.Session.SetString("userName", sUser);
          /////////////////////////////////
          /////////END TEST CODE///////////
          /////////////////////////////////
        }
      } catch (Exception e) {
        Logger.Error(e.Message, Environment.StackTrace);
      }
    }

    public HomeController(ILogger<HomeController> logger) {
      _logger = logger;
    }

    //Add a IActionresult function for every page we add
    public IActionResult Index() {
      if(HttpContext.Session.GetString("userName") != null) {
        return RedirectToAction("Home");
      } else {
        return RedirectToAction("Login");
      }
    }

    [HttpPost]
    public IActionResult BulkChangeSupportExp(IFormCollection keyValues) {
      if (HttpContext.Session.GetString("userName") != null) {
        string sLoggingString = "Home controller recieved request to perform bulk update of support expiration dates:\n";
        foreach (string key in keyValues.Keys) {
          if (key.Equals("supportExpDate")) {
            sLoggingString += "New Date: " + keyValues["supportExpDate"] + "\nRecords to change:";
          } else if (!key.Equals("__RequestVerificationToken")) {
            sLoggingString += " " + key;
          }
        }
        Logger.Log(sLoggingString, Environment.StackTrace);
        string sUpdatedDate = keyValues["supportExpDate"];
        foreach (string key in keyValues.Keys) {
          if (!key.Equals("supportExpDate") && !key.Equals("__RequestVerificationToken")) {
            CustomerInfoData oRecordToModify = CustomerInfo.GetRecord(key);

            DateTime oDt = new DateTime();
            if (DateTime.TryParse(sUpdatedDate, out oDt)) {
              oRecordToModify.dtSupportExp = oDt;
              CustomerInfo.Update(oRecordToModify);
            } else {
              Logger.Log("Invalid date time entered: " + sUpdatedDate, Environment.StackTrace);
              break;
            }
          }
        }
        return RedirectToAction("Home");
      } else {
        return RedirectToAction("Login");
      }
    }

    public IActionResult Privacy() {
      return View();
    }

    [HttpGet]
    public IActionResult Logs(DateTime iStartDate, DateTime iEndDate, String iUser, Boolean iErrors) {
      if (HttpContext.Session.GetString("userName") != null) {
        LogFilter iLogs = new LogFilter(sUser: iUser,
                                        oStartingDate: iStartDate,
                                        oEndingDate: iEndDate,
                                        bErrorsOnly: iErrors);
        List<LogItem> oLogs = Logger.GetLogs(iLogs);

        return View(oLogs);
      } else {
        return RedirectToAction("Login");
      }
    }
    public IActionResult Login(bool failed) {
      if (HttpContext.Session.GetString("userName") != null) {
        return RedirectToAction("Home");
      }else if (failed != null && failed) {
        ViewBag.error = failed;
        return View();
      } else {
        return View();
      }
    }

    [HttpPost]
    public IActionResult Login(string username, string password) {
      Validate_Creds(username, password);
      if (bIsInit) {
        try {
          HttpContext.Session.SetString("userEmail", username);
        } catch (ArgumentNullException) {
          return RedirectToAction("Login", true);
        }
        Logger.msUser = HttpContext.Session.GetString("userName");
        return RedirectToAction("Home");
      } else {
        return RedirectToAction("Login",true);
      }
    }

    [HttpPost]
    public IActionResult ClientDetails(ClientDetailsViewModel viewModel) {
      if (HttpContext.Session.GetString("userName") != null) {
        CustomerInfoData oCustDataBefore =
        CustomerInfo.GetRecord(viewModel.oCustomerInfoData.sRFUser);

        if (viewModel != null && viewModel.oCustomerInfoData != null 
          && (HttpContext.Session.GetString("permHDOwner").Equals("true") || HttpContext.Session.GetString("permHDPowerUser").Equals("true"))) {
          //if update did not work, there is nothing to add to HDEvents
          if (CustomerInfo.Update(viewModel.oCustomerInfoData)) {
            //getting all changed columns before and after the changes
            Dictionary<string, string> oPreCustPairs =
              oCustDataBefore.GetChangedColumns(viewModel.oCustomerInfoData);
            Dictionary<string, string> oPostCustPairs =
              viewModel.oCustomerInfoData.GetChangedColumns(oCustDataBefore);

            //adding all of the columns that were changed to the HDEvents table
            foreach (KeyValuePair<string, string> oCustPair in oPostCustPairs) {
              string sOldData;
              if (!oPreCustPairs.TryGetValue(oCustPair.Key, out sOldData)) {
                Logger.Log("Cannot retrieve " + oCustPair.Key + " from " +
                  oCustDataBefore.ToString(), Environment.StackTrace);
                continue;
              }
              HDEvents.EditRecord(viewModel.oCustomerInfoData.sRFUser,
                oCustPair.Key, sOldData, oCustPair.Value);
            }
          }
        }

        return RedirectToAction("home");
      } else {
        return RedirectToAction("Login");
      }
    }

    [HttpGet]
    public IActionResult ClientDetails(String userKey) {
      if (HttpContext.Session.GetString("userName") != null) {
        var oCustomerInfoData = CustomerInfo.GetRecord(userKey);
        var oPreCustomerInfoData = CustomerInfo.GetRecord(userKey);
        var oAuthList = AuthHistory.List(20, 1, userKey);
        var oHDEventsList = HDEvents.List(userKey);
        var delete = false;
        var edit = false;

        if (HttpContext.Session.GetString("permHDOwner").Equals("true") || HttpContext.Session.GetString("permHDPowerUser").Equals("true")) {
          delete = true;
          edit = true;
        }

        var viewModel = new ClientDetailsViewModel {
          oCustomerInfoData = oCustomerInfoData,
          oPreCustomerInfoData = oPreCustomerInfoData,
          oAuthList = oAuthList,
          oHDEventsList = oHDEventsList,
          bDelete = delete,
          bEdit = edit
        };

        return View(viewModel);
      } else {
        return RedirectToAction("Login");
      }
    }

    [HttpPost]
    public IActionResult NewRecord(CustomerInfoData newCustomerData) {
      if (HttpContext.Session.GetString("userName") != null) {
        //TODO when bad input return user to exact location of error
        String inputData = InputValidation(newCustomerData);
        if (inputData != "") {
          TempData["msg"] = "<script>alert('Incorrect input into " + inputData + ", retry');</script>";
          //TODO not returning comment section info, comes back empty
          return View(newCustomerData);
        }

        CustomerInfo.Insert(newCustomerData);
        return RedirectToAction("home");
      } else {
        return RedirectToAction("Login");
      }
    }

    [HttpGet]
    public IActionResult NewRecord() {
      if (HttpContext.Session.GetString("userName") != null) {
        if (HttpContext.Session.GetString("permHDOwner").Equals("true") || HttpContext.Session.GetString("permHDPowerUser").Equals("true"))
          return View();
        return RedirectToAction("Home");
      } else {
        return RedirectToAction("Login");
      }
    }

    [HttpGet]
    public IActionResult Home(int? page, string q) {
      if (HttpContext.Session.GetString("userName") != null) {
        if (page == null)
          page = 1;

        var sCookie = Request.Cookies["userResultsAmount"];

        if (sCookie == null) {
          CookieOptions oCookieOptions = new CookieOptions();
          oCookieOptions.Expires = DateTime.Now.AddDays(3650);

          Response.Cookies.Append("userResultsAmount", "10", oCookieOptions);
          sCookie = "10";
        }

        var iAmountPerPage = Convert.ToInt32(sCookie);

        var viewModel = new HomePageViewModel();

        if (q == null) {                                                           // need to be same values 
          var oCustomers = CustomerInfo.List(iAmountPerPage, (int)page, q); // 1st param sets how many per page //Last param is the search qurey  
          var oPager = new Pager(CustomerInfo.Count(q), page, iAmountPerPage, ""); // 3rd param would be how many per page
          viewModel.oCustomerList = oCustomers;
          viewModel.oPaging = oPager;

        } else {
          var oCustomers = CustomerInfo.List(iAmountPerPage, (int)page, q); // 1st param sets how many per page //Last param is the search qurey                                                                       
          var oPager = new Pager(CustomerInfo.Count(q), page, iAmountPerPage, q); // 3rd param would be how many per page
          viewModel.oCustomerList = oCustomers;
          viewModel.oPaging = oPager;
        }
        return View(viewModel);
      } else {
        return RedirectToAction("Login");
      }
    }

    [HttpPost]
    public IActionResult Profile(string sResultsAmount){
      if (HttpContext.Session.GetString("userName") != null) {
        if (sResultsAmount != null && Convert.ToInt32(sResultsAmount) > 0) {
          CookieOptions oCookieOptions = new CookieOptions();
          oCookieOptions.Expires = DateTime.Now.AddDays(3650);
          Response.Cookies.Append("userResultsAmount", sResultsAmount, oCookieOptions);
        }
        return View();
      } else {
        return RedirectToAction("Login");
      }
    }

    [HttpGet]
    public IActionResult Profile() {
      if (HttpContext.Session.GetString("userName") != null) {
        return View();
      } else {
        return RedirectToAction("Login");
      }
    }

    public IActionResult Admin() {
      if (HttpContext.Session.GetString("userName") != null) {
        return View();
      } else {
        return RedirectToAction("Login");
      }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    public IActionResult Error() {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private String InputValidation(CustomerInfoData newCustomer) {    //This function in takes the client details list
      string badCharacters = "\\$!@#%^&*()/[]{}<>?|_+\"-;:.,"; //and veryifies that correct input has been inputed.
      string comentsBadCharacters = ";\\#%^&*()<>|_+-";

      if (InputHandling.SRFUser(newCustomer.sRFUser) == false) {
        return "Company ID, these characters are not allowed:   " + badCharacters;
      } else if(InputHandling.SCustomer(newCustomer.sCustomer) == false) {
          return "Company Name, these characters are not allowed:   " + badCharacters;
      } else if (InputHandling.sAcctID(newCustomer.sAcctID) == false) {
        return "Salesforce ID, these characters are not allowed:   " + badCharacters;
      } else if (InputHandling.SAllocGrp(newCustomer.sAllocGrp) == false) {
        return "Allocation Group, these characters are not allowed:   " + badCharacters;
      } else if (InputHandling.SVAR(newCustomer.sVAR) == false) {
        return "VAR Name, these characters are not allowed:   " + badCharacters;
      } else if (InputHandling.SSerialID(newCustomer.sSerialID) == false) {
        return "Serial Number, these characters are not allowed:   " + badCharacters;
      } else if (InputHandling.SComments(newCustomer.sComments) == false) {
        return "Comments, these characters are not allowed:   " + comentsBadCharacters;
      } else if (InputHandling.SServerCost(newCustomer.sServerCost) == false) {
        return "Server Renewal Cost, these characters are not allowed:   " + badCharacters;
      } else if (InputHandling.SSysID(newCustomer.sSysID) == false) {
        return "System ID, these characters are not allowed:   " + badCharacters;
      } else if (InputHandling.SInstallCnt(newCustomer.sInstallCnt) == false) {
        return "Installation Count, these characters are not allowed:   " + badCharacters;
      } else if (InputHandling.SServerNotes(newCustomer.sServerNotes) == false) {
        return "Server Notes, these characters are not allowed: " + comentsBadCharacters;
      } /*else if (InputHandling.Duration(newCustomer.duration) == false) {
        return "Duration, these characters are not allowed:   " + badCharacters;
      }*/ else if (InputHandling.FReleaseNo(newCustomer.fReleaseNo) == false) {
        return "Release Number, these characters are not allowed:   " + badCharacters;
      } else if (InputHandling.IMinUsers(newCustomer.iMinUsers) == false) {
        return "Minimum User Count, these characters are not allowed:   " + badCharacters;
      } else if (InputHandling.IUsersAuth(newCustomer.iUsersAuth) == false) {
        return "Thin Clients, these characters are not allowed:   " + badCharacters;
      } else if (InputHandling.IAppsAuth(newCustomer.iAppsAuth) == false) {
        return "Application Count, these characters are not allowed:   " + badCharacters;
      } else if (InputHandling.IMobileAuth(newCustomer.iMobileAuth) == false) {
        return "Batch Clients, these characters are not allowed:   " + badCharacters;
      } else if (InputHandling.IBCSCount(newCustomer.iBCSCount) == false) {
        return "Barcode Scanner, these characters are not allowed:   " + badCharacters;
      } else if (InputHandling.IVoiceProd(newCustomer.iVoiceProd) == false) {
        return "Vocollect Clients, these characters are not allowed:   " + badCharacters;
      } else if (InputHandling.IUsersAllowed(newCustomer.iUsersAllowed) == false) {
        return "Users Allowed, these characters are not allowed:   " + badCharacters;
      } else if (InputHandling.ITrialTime(newCustomer.iTrialTime) == false) {
        return "Trial Days, these characters are not allowed:   " + badCharacters;
      } else if (InputHandling.IServerAuth(newCustomer.iServerAuth) == false) {
        return "Server Count, these characters are not allowed:   " + badCharacters;
      } else if (InputHandling.DtSupportExp(newCustomer.dtSupportExp) == false) {
        return "Expiration Date invalid";
      } //TODO this is the one inputted in the SaaS details page, but mapped to the
        //same column as the one in record details
      /*else if (InputHandling.DtSupportExp(newCustomer.dtSupportExp) == false) {
        return "Server Expiry invalid";
      }*/ else if (InputHandling.DtHeartBeat(newCustomer.dtHeartbeat) == false) {
        return "HeartBeat Date invalid";
      } else if (InputHandling.DtContractSt(newCustomer.dtContractSt) == false) {
        return "Contract Start invalid";
      } else if (InputHandling.DtContractEd(newCustomer.dtContractEd) == false) {
        return "Contract End invalid";
      } else if (InputHandling.DtInstallDt(newCustomer.dtInstallDt) == false) {
        return "Entry Date invalid";
      }
      return "";
      }
      
  }
}