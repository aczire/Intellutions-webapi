using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IntellutionsWeb.Extensions;
using IntellutionsWeb.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IntellutionsWeb
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      //const string sec = "ljhb3r4lhj34rbl1jh4v090fb337591abd32l4jhtb234hjtb243j65eae731f5a65ed1";

      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(options =>
      {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,

          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTHelper.ENC_KEY)),

          ValidateIssuer = true,
          ValidIssuer = JWTHelper.ISSUER,

          ValidateAudience = true,
          ValidAudience = JWTHelper.AUDIENCE,

          ValidateLifetime = true, //validate the expiration and not before values in the token

          ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
        };
      });
      services.AddCors(options =>
      {
        options.AddPolicy("AllowAllOrigins",
        builder =>
        {
          builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
        });
      });
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      HTTPContextHelper.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
      app.UseAuthentication();
      app.UseDefaultFiles();
      app.UseStaticFiles();

      app.UseCors(builder =>
          builder.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader());


      app.Use(async (context, next) =>
      {
        await next();
        if (context.Response.StatusCode == 404 &&
              !context.Request.Path.Value.StartsWith("/api/"))
        {
          context.Request.Path = "/index.html";
          await next();
        }
      });

      app.UseMvcWithDefaultRoute();

      app.UseExceptionHandler(
          builder =>
          {
            builder.Run(
                      async context =>
                      {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                          context.Response.AddApplicationError(error.Error.Message);
                          await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                        }
                      });
          });

    }
  }
}
