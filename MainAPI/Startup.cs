using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainAPI.Services;
using MainAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace MainAPI
{
    public class Startup
    {
        private readonly string v1 = "v1";
        private readonly string appTitle = "Spider API";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            DependencyInjections.Register(services);

            services.AddDbContext<MainAPIContext>(
            o => o.UseSqlServer(Configuration.GetConnectionString(nameof(MainAPIContext))
        ));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(v1, new OpenApiInfo { Title = appTitle, Version = v1 });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options => options
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
          );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{appTitle} {v1}");
                    c.RoutePrefix = string.Empty;
                });
            }

          //  app.UseDeveloperExceptionPage();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
