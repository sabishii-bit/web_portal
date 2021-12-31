using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RFGenLicensingPortal.Models {
  public class UserSession {
    private string msUserName;
    private string msUserEmail;

    private bool mbHDOwner;
    private bool mbHDPowerUser;
    private bool mbHDGiveCodes;
    private bool mbHDSales;

    private bool mbInit;

    public UserSession() {
      mbInit = false;
    }

    public void SetUserName(string sUserName) {
      if (mbInit == true)
        return;

      msUserName = sUserName;
    }

    public void SetUserEmail(string sUserEmail) {
      if (mbInit == true)
        return;

      msUserEmail = sUserEmail;
    }

    public void SetPermissions(bool bHDOwner, bool bHDPowerUser, bool bHDGiveCodes, bool bHDSales) {
      if (mbInit == true)
        return;

      mbHDOwner = bHDOwner;
      mbHDPowerUser = bHDPowerUser;
      mbHDGiveCodes = bHDGiveCodes;
      mbHDSales = bHDSales;
    }

    public void SetInit() { mbInit = true; }

    public string GetUserName() { return msUserName; }
    public string GetUserEmail() { return msUserEmail; }
    public bool IsHDOwner() { return mbHDOwner; }
    public bool IsHDPowerUser() { return mbHDPowerUser; }
    public bool IsHDGiveCodes() { return mbHDGiveCodes; }
    public bool IsHDSales() { return mbHDSales; }
    public bool IsInit() { return mbInit; }
  }
}
