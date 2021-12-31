using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RFGenLicensingPortal.Models {
  //TODO add null exception errors
  public class InputHandling {
   
    public static Boolean SRFUser(String input) {
      //TODO check that this value already is not a primary key in table
      if (!StringCheck(input)) return false;
      return true;
    }

    public static Boolean SCustomer(String input) {
      if (!StringCheck(input)) return false;
      return true;
    }

    public static Boolean sAcctID(String input) {
      if (!StringCheck(input)) return false;
      return true;
    }

    public static Boolean SAllocGrp(String input) {
      if (!StringCheck(input)) return false;
      return true;
    }

    public static Boolean SVAR(String input) {
      if (!StringCheck(input)) return false;
      return true;
    }

    public static Boolean SSerialID(String input) {
      if (!StringCheck(input)) return false;
      return true;
    }

    public static Boolean SComments(String input) {
      if (!CommentsStringCheck(input)) return false;
      return true;
    }

    public static Boolean SServerCost(String input) {
      if (!StringCheck(input)) return false;
      return true;
    }

    public static Boolean SSysID(String input) {
      if (!StringCheck(input)) return false;
      return true;
    }

    public static Boolean SInstallCnt(String input) {
      if (!StringCheck(input)) return false;
      return true;
    }

    public static Boolean SServerNotes(String input) {
      if (!CommentsStringCheck(input)) return false;
      return true;
    }

    //public static Boolean Duration(CustomerInfoData input) {
    //  return true;
    //}

    public static Boolean FReleaseNo(float input) {
      if (!StringCheck(input.ToString())) return false;
     
      return true;
    }

    public static Boolean IMinUsers(int input) {
      if (!StringCheck(input.ToString())) return false;
      return true;
    }

    public static Boolean IUsersAuth(int input) {
      if (!StringCheck(input.ToString())) return false;
      return true;
    }

    public static Boolean IAppsAuth(int input) {
      if (!StringCheck(input.ToString())) return false;
      return true;
    }

    public static Boolean IMobileAuth(int input) {
      if (!StringCheck(input.ToString())) return false;
      return true;
    }

    public static Boolean IBCSCount(int input) {
      if (!StringCheck(input.ToString())) return false;
      return true;
    }

    public static Boolean IVoiceProd(int input) {
      if (!StringCheck(input.ToString())) return false;
      return true;
    }

    public static Boolean IUsersAllowed(int input) {
      if (!StringCheck(input.ToString())) return false;
      return true;
    }

    public static Boolean ITrialTime(int input) {
      if (!StringCheck(input.ToString())) return false;
      return true;
    }

    public static Boolean IServerAuth(int input) {
      if (!StringCheck(input.ToString())) return false;
      return true;
    }

    public static Boolean DtSupportExp(DateTime input) {
      if (!DateTimeCheck(input)) return false;
      return true;
    }

    //Server expire under SaaS Details
    //public static String DtSupportExp(CustomerInfoData input) {
    //  return "";
    //}

    public static Boolean DtHeartBeat(DateTime input) {
      if (!DateTimeCheck(input)) return false;
      return true;
    }

    public static Boolean DtContractSt(DateTime input) {
      if (!DateTimeCheck(input)) return false;
      return true;
    }

    public static Boolean DtContractEd(DateTime input) {
      if (!DateTimeCheck(input)) return false;
      return true;
    }
    public static Boolean DtInstallDt(DateTime input) {
      if (!DateTimeCheck(input)) return false;
      return true;
    }

    private static Boolean StringCheck(string test) {
      string badCharacters = ";:.,\\$!@#%^&*()/[]{}<>?|_+\"-";
      char[] badCharacterCharArr = badCharacters.ToCharArray();

      for (int i = 0; i < badCharacterCharArr.Length; i++) {
        if (test.Contains(badCharacterCharArr[i])) return false;
      }

      return true;
    }
    private static Boolean CommentsStringCheck(string test) {
      if (test == null) {
        return true;
      }
      string badCharacters = ";\\#%^&*()<>|_+-";
      char[] badCharacterCharArr = badCharacters.ToCharArray();

      for (int i = 0; i < badCharacterCharArr.Length; i++) {
        if (test.Contains(badCharacterCharArr[i])) return false;
      }

      return true;
    }
    private static Boolean DateTimeCheck(DateTime test) {
      //TODO when return false it crashes the program rather
      //than just returning view
      if (test.Year < 1983 || test.Year > 2200) return false;
      else if (test.Month < 1 || test.Month > 12) return false;
      else if (test.Day < 1 || test.Day > 31) return false;
      return true;
    }

  }
}
