using System;
using System.Data.SqlClient;

namespace RFGenLicensingPortal.Models {
  public class HDEventsData {
    public string sRFUser { get; set; }
    public string sEvent { get; set; }
    public string sUserID { get; set; }
    public string sUserPC { get; set; }
    public DateTime oTimeStamp { get; set; }

    public HDEventsData(SqlDataReader oReader) {
      try {
        if (oReader == null) {
          Logger.Error("Record object set to null.", Environment.StackTrace);
          return;
        }

        if (!oReader.HasRows) {
          Logger.Error("No records supplied in object creation.", Environment.StackTrace);
          return;
        }

        int iRFUserIndex = oReader.GetOrdinal("RFUser");
        int iEventIndex = oReader.GetOrdinal("Event");
        int iUserIDIndex = oReader.GetOrdinal("UserID");
        int iUserPCIndex = oReader.GetOrdinal("UserPC");
        int iTimeStampIndex = oReader.GetOrdinal("TimeStamp");

        if (!oReader.IsDBNull(iRFUserIndex))
          this.sRFUser = oReader.GetString(iRFUserIndex);
        else this.sRFUser = null;

        if (!oReader.IsDBNull(iEventIndex))
          this.sEvent = oReader.GetString(iEventIndex);
        else this.sEvent = null;

        if (!oReader.IsDBNull(iUserIDIndex))
          this.sUserID = oReader.GetString(iUserIDIndex);
        else this.sUserID = null;

        if (!oReader.IsDBNull(iUserPCIndex))
          this.sUserPC = oReader.GetString(iUserPCIndex);
        else this.sUserPC = null;

        if (!oReader.IsDBNull(iTimeStampIndex))
          this.oTimeStamp = oReader.GetDateTime(iTimeStampIndex);
        else this.oTimeStamp = default;

      } catch(Exception e) {
        Logger.Error("Object Creation Error: ", Environment.StackTrace);
        return;
      }
    }
  }
}
