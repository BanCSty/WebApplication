using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebShop.DAL;
using WebShop.DAL.Interfaces;
using WebShop.DAL.Repositories;
using WebShop.Domain.ViewModels.Account;
using WebShop.Service.Implementations;
using WebShop.Service.Interfaces;
using WebShop.Service.Middleware;
using WebShop.Services.Implentations;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddDbContext<AppDbContext>(options =>
                   options.UseNpgsql(Configuration.GetConnectionString("WebSHop")));

            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));

            services.AddControllersWithViews();
            services.AddHttpClient();
            services.InitializeRepositories();
            services.InitializeServices();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddCookie(x =>
            {
                x.Cookie.Name = "Bearer";
                x.Events = new CookieAuthenticationEvents
                {
                    OnValidatePrincipal = async x =>
                    {

                        // since our cookie lifetime is based on the access token one,
                        // check if we're more than halfway of the cookie lifetime
                        var now = DateTimeOffset.UtcNow;
                        var timeElapsed = now.Subtract(x.Properties.IssuedUtc.Value);
                        var timeRemaining = x.Properties.ExpiresUtc.Value.Subtract(now);

                        if (timeElapsed > timeRemaining)
                        {
                            var identity = (ClaimsIdentity)x.Principal.Identity;
                            var accessTokenClaim = identity.FindFirst("access_token");
                            var refreshTokenClaim = identity.FindFirst("refresh_token");

                            // if we have to refresh, grab the refresh token from the claims, and request
                            // new access token and refresh token
                            var refreshToken = refreshTokenClaim.Value;
                            var response = await new HttpClient().RequestRefreshTokenAsync(new RefreshTokenRequest
                            {
                                Address = "https://localhost:44314/Account/RefreshToken",
                                ClientId = "mvc",
                                ClientSecret = "mvc",
                                RefreshToken = refreshToken
                            });

                            if (!response.IsError)
                            {
                                // everything went right, remove old tokens and add new ones
                                identity.RemoveClaim(accessTokenClaim);
                                identity.RemoveClaim(refreshTokenClaim);

                                identity.AddClaims(new[]
                                {
                                        new Claim("access_token", response.AccessToken),
                                        new Claim("refresh_token", response.RefreshToken)
                                    });

                                // indicate to the cookie middleware to renew the session cookie
                                // the new lifetime will be the same as the old one, so the alignment
                                // between cookie and access token is preserved
                                x.ShouldRenew = true;
                            }
                        }
                    }
                };
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JwtSettings:Issuer"],
                    ValidAudience = Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:Secret"]))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["Bearer"];
                        return Task.CompletedTask;
                    }
                };
            });      
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseDeveloperExceptionPage();

                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();


            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
          
            app.UseStaticFiles();

            app.UseMiddleware<TokenValidationMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
