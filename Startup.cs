using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OOP_WORKSHOP_PROJECT.Authorization;
using OOP_WORKSHOP_PROJECT.Controllers;
using OOP_WORKSHOP_PROJECT.Data;

namespace OOP_WORKSHOP_PROJECT
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
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddDbContext<PostContext>
                (opt => opt.UseSqlServer
                (Configuration.GetConnectionString("PostConnection")));

            services.AddDbContext<UserContext>
                (opt => opt.UseSqlServer
                (Configuration.GetConnectionString("UserConnection")));

            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            //add the data
            // services.AddSingleton<IUserRepo, MockUserRepo>();
            // services.AddSingleton<IPostRepo, MockPostRepo>();
            services.AddScoped<IUserRepo,SqlUserRepo>();
            services.AddScoped<IPostRepo, SqlPostRepo>();
            services.AddScoped<IAuthorize,JwtService>();
            services.AddScoped<IReportRepo, SqlReportRepo>();


            services.AddOptions();
            services.AddAuthentication();
            /*services.AddMvcCore().AddJsonFormatters(options => options.ContractResolver = new CamelCasePropertyNamesContractResolver());*/
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

            app.UseCors("MyPolicy");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
