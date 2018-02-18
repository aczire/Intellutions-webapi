using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using IntellutionsWeb.Helper;
using IntellutionsWeb.Models;

namespace IntellutionsWeb.Controllers
{
  [Produces("application/json")]
  [Route("api/Login")]
  public class LoginController : Controller
  {
    // POST: api/Login
    [HttpPost]
    public IActionResult Authenticate([FromBody] CredentialsViewModel creds)
    {
      HttpResponseMessage responseMsg = new HttpResponseMessage();

      HttpContext context = HTTPContextHelper.HttpContext;

      bool isUsernamePasswordValid = false;

      String username = creds.UserName;
      String password = creds.Password;

      if (username != null && !String.IsNullOrWhiteSpace(username))
        isUsernamePasswordValid = password == "1" ? true : false;
      // if credentials are valid
      if (isUsernamePasswordValid)
      {

        string token = JWTHelper.CreateToken(username);
        //return the auth
        OkObjectResult auth = new OkObjectResult(new
        {
          auth_token = token
        });

        return (auth);
      }
      else
      {
        // if credentials are not valid send unauthorized status code in response
        return Unauthorized();
      }
    }



  }
}
