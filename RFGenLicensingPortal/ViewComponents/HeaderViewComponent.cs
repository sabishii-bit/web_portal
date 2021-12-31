using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RFGenLicensingPortal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RFGenLicensingPortal.ViewComponents {
  [ViewComponent(Name = "Header")]
  public class HeaderViewComponent : ViewComponent {
    public async Task<IViewComponentResult> InvokeAsync() {

      bool bAllowAdd = false;
      bool bAllowCopy = false;

      if (HttpContext.Session.GetString("permHDOwner").Equals("true")
        || HttpContext.Session.GetString("permHDPowerUser").Equals("true")) {
        bAllowAdd = true;
        bAllowCopy = true;
      }

      var viewModel = new HeaderViewModel {
        bAddNew = bAllowAdd,
        bCopyServer = bAllowCopy
      };

      return View("Header", viewModel);
    }
  }
}