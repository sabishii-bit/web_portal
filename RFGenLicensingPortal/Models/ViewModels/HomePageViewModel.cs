using System;
using System.Collections.Generic;

namespace RFGenLicensingPortal.Models {
  public class HomePageViewModel {
    public IEnumerable<CustomerInfoData> oCustomerList { get; set; }
    public Pager oPaging { get; set; }
  }
}
