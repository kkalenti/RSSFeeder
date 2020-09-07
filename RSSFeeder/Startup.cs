using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RSSFeeder.Data.Interfaces;
using RSSFeeder.Data.Repositories;

namespace RSSFeeder
{
    public class Startup
    {
        private IConfiguration _confString { get; }

        public Startup(IHostEnvironment hostEnvironment)
        {
            _confString = new ConfigurationBuilder().SetBasePath(hostEnvironment.ContentRootPath).
                AddXmlFile("feedersettings.xml", optional: true, reloadOnChange: true).Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSingleton(provider => _confString);
            services.AddSingleton<IFeederSettings, FeederSettingsRepository>();
            services.AddScoped<IRSSFeed, RSSFeedRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
