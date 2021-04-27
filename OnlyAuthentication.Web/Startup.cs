using System.IO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OnlyAuthentication.Web
{
    public class Startup
    {

        //public Startup(IConfiguration configuration, IHostingEnvironment environment)
        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private readonly IHostEnvironment _hostEnvironment;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // create a directory for keys if it doesn't exist
            //it'll be created in the root, beside the wwwroot directory
            var keysDirectoryName = "Keys";
            //var keysDirectoryPath = Path.Combine(hostingEnvironment.ContentRootPath, keysDirectoryName);
            var keysDirectoryPath = Path.Combine(_hostEnvironment.ContentRootPath, keysDirectoryName);
            if (!Directory.Exists(keysDirectoryPath)) Directory.CreateDirectory(keysDirectoryPath);

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(keysDirectoryPath))
                .SetApplicationName("CustomCookieAuthentication");


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            services.AddRazorPages();
            services.AddHttpContextAccessor();
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
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // If we use strict then will have problems on iPhone Chrome coming back to the site
            // and being logged out.
            //app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict });
            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
