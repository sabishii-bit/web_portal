using System;
using System.Data.SqlClient;
using System.Configuration;

namespace RFGenLicensingPortal.Models {
  public static class DBHandle {
    public static SqlConnection mdbHandle { get; set; }
    private static bool bInit;

    static DBHandle() { bInit = false; }

    public static bool Init() {
      if (bInit == true) {
        Logger.Error("DB Reinitialization attempted.", Environment.StackTrace);
        return false;
      }

      try {
        /* TODO: Move string param to web.config.
         * Having issues pulling from that file atm. 
         * Need to research once core architecture is laid down 
         * - Dmitriy */
        mdbHandle = new SqlConnection("Data Source=164.90.148.165,1433;" +
                                      "Initial Catalog=LicensePortalDEV;" +
                                      "User ID=dbuser;" +
                                      "Password=b0ttmGear2021!;");
        mdbHandle.Open();

        bInit = true;
        return true;
      } catch (Exception e) {
        Logger.Error("Issue initializing DB connection: " + e.Message, Environment.StackTrace);
        return false;
      }
    }

    public static SqlDataReader Query(SqlCommand dbcmd) {
      SqlDataReader dbReader = dbcmd.ExecuteReader();
      if (!dbReader.HasRows) {
        dbReader.Close();
        return null;
      }
      return dbReader;
    }

    public static int NonQuery(SqlCommand dbcmd) {
      if(dbcmd.CommandText.ToUpper().Contains("DELETE")) {
        Logger.Error("Record Deletion Attempted and blocked!", Environment.StackTrace);
        return -1;
      }

      try {
        int iRows = dbcmd.ExecuteNonQuery();
        return iRows;
      } catch(Exception e) {
        //Logger.Error("NonQuery Error: " + e.Message, Environment.StackTrace);
        return -1;
      }
    }
  }
}
