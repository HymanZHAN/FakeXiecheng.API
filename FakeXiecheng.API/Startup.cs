using FakeXiecheng.API.Database;
using FakeXiecheng.API.Extensions;
using FakeXiecheng.API.Profiles;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FakeXiecheng.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddSwaggerDocumentation();
            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                //setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            }).AddXmlDataContractSerializerFormatters();
            services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
                //option.UseSqlServer("server=localhost; Database=FakeXiechengDb; User Id=sa; Password=PaSSworld12!");
                //option.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FakeXiechengDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            });
            services.AddScoped<ITouristRouteRepository, TouristRepository>();
            services.AddAutoMapper(typeof(TouristRouteProfile), typeof(TouristRoutePictureProfile));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwaggerDocumentation();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}