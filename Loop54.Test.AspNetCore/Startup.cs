using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Loop54.AspNet;
using Microsoft.AspNetCore.Http;

namespace Loop54.Test.AspNetCore
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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Here we add a named loop54 instance. We could add more named instances, for example if using multiple languages 
            //by adding them to the Loop54SettingsCollection. Note that if adding more than one named Loop54Setting the 
            //ILoop54Client interface wont be injectable. but instead you need to inject the ILoop54ClientProvider and use 
            //the GetNamed method, providing the same name used here, as seen in the SearchController.
            services.AddLoop54(Loop54SettingsCollection.Create().Add("English", "https://helloworld.54proxy.com"));
            //Could be replaced with:
            //services.AddLoop54("https://helloworld.54proxy.com");
            //For the most basic implementations.

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });
        }
    }
}
