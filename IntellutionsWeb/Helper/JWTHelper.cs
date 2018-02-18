using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IntellutionsWeb.Helper
{
  public static class JWTHelper
  {
    // Read this from Configuration.
    public const string ENC_KEY = "ljhb3r4lhj34rbl1jh4v090fb337591abd32l4jhtb234hjtb243j65eae731f5a65ed1";
    private const string iSSUER = "Intellutions";
    private const string aUDIENCE = "Intellutions";

    public static string ISSUER => iSSUER;

    public static string AUDIENCE => aUDIENCE;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public static string CreateToken(string username)
    {
      //Set issued at date
      DateTime issuedAt = DateTime.UtcNow;
      //set the time when it expires
      DateTime expires = DateTime.UtcNow.AddDays(7);

      var tokenHandler = new JwtSecurityTokenHandler();

      //create a identity and add claims to the user which we want to log in
      ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
      {
                new Claim(ClaimTypes.Name, username)
            });


      var now = DateTime.UtcNow;
      var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(ENC_KEY));
      var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);


      var token =
          (JwtSecurityToken)
              tokenHandler.CreateJwtSecurityToken(issuer: ISSUER, audience: AUDIENCE,
                  subject: claimsIdentity, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);
      var tokenString = tokenHandler.WriteToken(token);

      return tokenString;
    }

  }
}
