using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using CaseOPGElgiganten.Controllers;
using CaseOPGElgiganten.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CaseOPGElgiganten
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<ElgigantenContext>();
      services.AddScoped<IElgigantenRepository, ElgigantenRepository>();

      services.AddAutoMapper(Assembly.GetExecutingAssembly());

      services.AddControllers();

      services.AddApiVersioning(opt =>
      {
          opt.AssumeDefaultVersionWhenUnspecified = true;
          opt.DefaultApiVersion = new ApiVersion(1, 1);
          opt.ReportApiVersions = true;

          opt.ApiVersionReader = ApiVersionReader.Combine(
              new HeaderApiVersionReader("X-Version"),
              new QueryStringApiVersionReader("ver", "version"));
      });

      services.AddMvc(opt => opt.EnableEndpointRouting = false)
        .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(cfg =>
      {
        cfg.MapControllers();
      });
    }
  }
}
