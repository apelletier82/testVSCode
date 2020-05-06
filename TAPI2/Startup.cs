using System.Security.Principal;
using System.Security.AccessControl;
using System.Reflection;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TAPI2.DB;
using TAPI2.Exceptions;
using TAPI2.Services;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using TAPI2.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TAPI2.Services.Abstract;

namespace TAPI2
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(o =>
            {
                o.Authority = "http://keycloak-devtest/auth/realms/DEVTEST";
                o.Audience = "app_tapi2_test";
                o.IncludeErrorDetails = true;
                o.SaveToken = true;
                o.TokenValidationParameters.ValidateIssuer = true;
                o.TokenValidationParameters.ValidateAudience = true;
                o.TokenValidationParameters.NameClaimType = "preferred_username";
                //o.TokenValidationParameters.RoleClaimType = "roles";
                if (Environment != null && Environment.IsDevelopment())
                    o.RequireHttpsMetadata = false;
                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";

                        if (Environment != null && Environment.IsDevelopment())
                            return c.Response.WriteAsync(c.Exception.ToString());

                        return c.Response.WriteAsync("An error occured processing your authentication.");
                    }
                };
            });
            services.AddAuthorization(c =>
            {                
                c.AddPolicy("RequireContactViewerRole", p => p.RequireRole("ContactViewer"));
                //c.AddPolicy("RequireContactOperatorRole", p => p.RequireRole("ContactOperator"));
                c.AddPolicy("ElevatedRights", p => p.RequireRole(new[] { "Administrator", "SuperUser" }));
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IUserIdentity, HttpContextUserIdentity>();
            services.AddControllers();
            services.AddDbContext<TAPIDataContext>(o =>
            {
                string cnt = Configuration.GetConnectionString("TAPI");
                o.UseMySql(cnt);
            });
            services.AddScoped<IContactService, ContactDBService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler(Err =>
                {
                    Err.Run(async context =>
                    {
                        HttpStatusCode status = HttpStatusCode.InternalServerError;
                        String message = String.Empty;
                        String errorTypeClass = String.Empty;
                        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        ExceptionType exceptionType = ExceptionType.Runtime;

                        if (exceptionHandlerPathFeature != null)
                        {
                            message = exceptionHandlerPathFeature.Error.Message;
                            if (exceptionHandlerPathFeature.Error is UnauthorizedAccessException)
                                status = HttpStatusCode.Unauthorized;
                            else if (exceptionHandlerPathFeature.Error is NotImplementedException)
                                status = HttpStatusCode.NotImplemented;
                            else if (exceptionHandlerPathFeature.Error is ContactException)
                            {
                                status = ((ContactException)exceptionHandlerPathFeature.Error).HttpStatusCode;
                                exceptionType = ((ContactException)exceptionHandlerPathFeature.Error).ExceptionType;
                            }
                            errorTypeClass = nameof(exceptionHandlerPathFeature.Error);
                        }

                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int)status;
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(
                                new ErrorDto(exceptionType,
                                    nameof(status),
                                    message,
                                    (int)status,
                                    context.TraceIdentifier,
                                    errorTypeClass)));

                        await context.Response.WriteAsync(new string(' ', 512)); // IE padding
                    });
                });
                app.UseHttpsRedirection();
                app.UseHsts();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //test change + compile + gitignore 
        }
    }
}
