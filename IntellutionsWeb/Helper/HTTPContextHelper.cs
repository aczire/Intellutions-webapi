using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntellutionsWeb.Helper
{
  public class HTTPContextHelper
  {
    private static IHttpContextAccessor _accessor;
    public static void Configure(IHttpContextAccessor httpContextAccessor)
    {
      _accessor = httpContextAccessor;
    }

    public static HttpContext HttpContext => _accessor.HttpContext;
  }
}
