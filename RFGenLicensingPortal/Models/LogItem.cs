using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RFGenLicensingPortal.Models {
  public class LogItem {
    public string miUserId { get; set; }
    public DateTime moTimeStamp { get; set; }
    public Boolean mbIsError { get; set; }
    public string msDescription { get; set; }
    public string msStackTrace { get; set; }

    public LogItem(SqlDataReader oReader) {
      try {
        if (oReader == null) {
          Logger.Error("Record object set to null.", Environment.StackTrace);
          return;
        }

        if (!oReader.HasRows) {
          Logger.Error("No records supplied in object creation.", Environment.StackTrace);
          return;
        }

        int iUserIdIndex = oReader.GetOrdinal("UserID");
        int iTimeStampIndex = oReader.GetOrdinal("TimeStamp");
        int iIsErrorIndex = oReader.GetOrdinal("IsError");
        int iDescriptionIndex = oReader.GetOrdinal("Description");
        int iStackTraceIndex = oReader.GetOrdinal("StackTrace");

        if (!oReader.IsDBNull(iUserIdIndex))
          this.miUserId = oReader.GetString(iUserIdIndex);
        else this.miUserId = null;

        if (!oReader.IsDBNull(iTimeStampIndex))
          this.moTimeStamp = oReader.GetDateTime(iTimeStampIndex);
        else this.moTimeStamp = default;

        if (!oReader.IsDBNull(iIsErrorIndex))
          this.mbIsError = oReader.GetBoolean(iIsErrorIndex);
        else this.mbIsError = false;

        if (!oReader.IsDBNull(iDescriptionIndex))
          this.msDescription = oReader.GetString(iDescriptionIndex);
        else this.msDescription = null;

        if (!oReader.IsDBNull(iStackTraceIndex))
          this.msStackTrace = oReader.GetString(iStackTraceIndex);
        else this.msStackTrace = null;
      } catch (Exception e) {
        Logger.Error(e.Message, Environment.StackTrace);
        return;
      }
    }
  }
}
