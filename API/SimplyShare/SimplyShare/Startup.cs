using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Azure.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimplyShare.Hubs;
using SimplyShare.Models;

namespace SimplyShare
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var clientId = Configuration[Constants.AZUREKEYVAULT_B2C_CLIENT_ID];
            var tenantName = Configuration[Constants.AZUREKEYVAULT_B2C_TENANT];
            var signalRConnectionString = Configuration[Constants.AZUREKEYVAULT_SIGNALR_CONNECTIONSTRING_KEY];

            services.AddAuthentication(AzureADB2CDefaults.AuthenticationScheme)
                .AddAzureADB2C(options =>
                {

                    options.Instance = $"https://{tenantName}.b2clogin.com/tfp/";
                    options.ClientId = clientId;
                    options.CallbackPath = "/signin-oidc";
                    options.Domain = $"{tenantName}.onmicrosoft.com";
                    options.SignUpSignInPolicyId = Constants.POLICY_SIGNIN_OR_SIGNUP;
                    options.ResetPasswordPolicyId = Constants.POLICY_PASSWORD_RESET;
                })
                .AddJwtBearer(Constants.SCHEMA_NAME, options =>
                {
                    options.Authority = $"https://login.microsoftonline.com/{tenantName}.onmicrosoft.com";
                    options.Audience = clientId;
                });

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSignalR().AddAzureSignalR(options =>
            {
                options.ServerStickyMode = ServerStickyMode.Required;
                options.ConnectionString = signalRConnectionString;

            });
            //services.AddSingleton<WeatherForecastService>();
            services.AddApplicationInsightsTelemetry();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRewriter(new RewriteOptions().AddRedirect("AzureADB2C/Account/SignedOut", "/"));
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapHub<UserMessageHub>("/usermessagehub");
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
