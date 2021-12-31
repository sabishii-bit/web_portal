using System;
using System.Collections.Generic;

namespace RFGenLicensingPortal.Models {
  public class ClientDetailsViewModel {
    public CustomerInfoData oCustomerInfoData { get; set; }
    public CustomerInfoData oPreCustomerInfoData { get; set; }
    public List<AuthHistoryData> oAuthList { get; set; }
    public List<HDEventsData> oHDEventsList { get; set; }
    public bool bDelete { get; set; }
    public bool bEdit { get; set; }
  }
}
