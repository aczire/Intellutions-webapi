using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNetCore.Authorization;

namespace IntellutionsWeb.Controllers
{
  public class HomeController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }

    [Authorize]
    public IActionResult GetUserDetails()
    {
      OkObjectResult userDetails = new OkObjectResult(new
      {
        username = User.Identity.Name
      });

      return (userDetails);
    }
  }
}
