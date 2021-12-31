using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RFGenLicensingPortal.Models {
  public class LogFilter {
    public DateTime moStartingDate { get; set; }
    public DateTime moEndingDate { get; set; }
    public string msUser { get; set; }
    public Boolean mbErrorsOnly { get; set; }

    public LogFilter(DateTime oStartingDate = default,
                     DateTime oEndingDate = default, string sUser = "",
                     Boolean bErrorsOnly = false) {
      moStartingDate = oStartingDate;
      if (moStartingDate == default)
        moStartingDate = new DateTime(1753, 1, 1, 0, 0, 0);
      moEndingDate = oEndingDate;
      if (moEndingDate == default)
        moEndingDate = new DateTime(9999, 12, 31, 23, 59, 59);
      msUser = sUser;
      mbErrorsOnly = bErrorsOnly;
    }
  }
}
