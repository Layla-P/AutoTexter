using AutoTexter.Models;
using AutoTexter.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutoTexter
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
            //The following lines map the appsettings to the models in the Model folder
            services.Configure<TwilioAccount>(Configuration.GetSection("TwilioAccount"));
            services.Configure<CsAccount>(Configuration.GetSection("CsAccount"));
            services.Configure<StorageCreds>(Configuration.GetSection("StorageCreds"));
            //The following two lines setup the dependency injection of both our services
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITextToSpeechService, TextToSpeechService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
