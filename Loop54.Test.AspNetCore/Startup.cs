using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Loop54.AspNet;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

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
            var loop54Settings = new Loop54Settings("https://helloworld.54proxy.com", "TestApiKey");
            services.AddLoop54(Loop54SettingsCollection.Create().Add("English", loop54Settings));
            //Could be replaced with:
            //services.AddLoop54("https://helloworld.54proxy.com");
            //For the most basic implementations.

            services.AddMvc(options => options.EnableEndpointRouting = false);
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
