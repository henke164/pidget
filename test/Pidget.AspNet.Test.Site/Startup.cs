﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pidget.AspNet.Test.Site
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
            => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
            => services.ConfigurePidgetMiddleware(
                Configuration.GetSection("ExceptionReporting"));

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<OnExceptionMiddleware>();
            app.UsePidgetMiddleware();

            app.Run(context =>
            {
                throw new InvalidOperationException(
                    "You shall not pass!");
            });
        }
    }
}
