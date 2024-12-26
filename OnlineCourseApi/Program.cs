
using System.Net;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using OnlineCourseApi.Data;
using OnlineCourseApi.Data.Entities;
using OnlineCourseApi.Middlewares;
using OnlineCourseApi.Service;
using Serilog;
using Serilog.Templates;

namespace OnlineCourseApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure Serilog with the settings
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Debug()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .CreateBootstrapLogger();

            try
            {
                //we have 2 parts here, one is service configuration for DI and 2nd one is Middlewares

                #region Service Configuration
                var builder = WebApplication.CreateBuilder(args);
                var configuration = builder.Configuration;

                builder.Services.AddApplicationInsightsTelemetry();

                builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .WriteTo.Console(new ExpressionTemplate(
                    // Include trace and span ids when present.
                    "[{@t:HH:mm:ss} {@l:u3}{#if @tr is not null} ({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}"))
                .WriteTo.ApplicationInsights(
                  services.GetRequiredService<TelemetryConfiguration>(),
                  TelemetryConverter.Traces));

                Log.Information("Starting the SmartLearnByRishika API...");

                //#region AD B2C configuration
                //// Adds Microsoft Identity platform (AAD v2.0) support to protect this Api
                //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                //        .AddMicrosoftIdentityWebApi(options =>

                //        {
                //            configuration.Bind("AzureAdB2C", options);
                //            options.Events = new JwtBearerEvents();

                //            /// <summary>
                //            /// Below you can do extended token validation and check for additional claims, such as:
                //            ///
                //            /// - check if the caller's account is homed or guest via the 'acct' optional claim
                //            /// - check if the caller belongs to right roles or groups via the 'roles' or 'groups' claim, respectively
                //            ///
                //            /// Bear in mind that you can do any of the above checks within the individual routes and/or controllers as well.
                //            /// For more information, visit: https://docs.microsoft.com/azure/active-directory/develop/access-tokens#validate-the-user-has-permission-to-access-this-data
                //            /// </summary>

                //            //options.Events.OnTokenValidated = async context =>
                //            //{
                //            //    string[] allowedClientApps = { /* list of client ids to allow */ };

                //            //    string clientAppId = context?.Principal?.Claims
                //            //        .FirstOrDefault(x => x.Type == "azp" || x.Type == "appid")?.Value;

                //            //    if (!allowedClientApps.Contains(clientAppId))
                //            //    {
                //            //        throw new System.Exception("This client is not authorized");
                //            //    }
                //            //};
                //        }, options => { configuration.Bind("AzureAdB2C", options); });

                //// The following flag can be used to get more descriptive errors in development environments
                //IdentityModelEventSource.ShowPII = false;
                //#endregion  AD B2C configuration


                //var builder = WebApplication.CreateBuilder(args);


                //if (builder.Environment.IsDevelopment())
                //{
                //    builder.Configuration.AddJsonFile
                //        ("appsettings.Development.json", optional: true, reloadOnChange: true);
                //}
                //else
                //{
                //    builder.Configuration.AddJsonFile
                //        ("appsettings.json", optional: false, reloadOnChange: true);
                //}

                //var configuration = builder.Configuration;
                //DB configuration goes here
                builder.Services.AddDbContextPool<OnlineCourseDbContext>(options =>
                {
                    options.UseSqlServer(
                        configuration.GetConnectionString("DbContext"),
                        provideroptions => provideroptions.EnableRetryOnFailure()
                        );
                    //options.EnableSensitiveDataLogging();
                });

                // Add services to the container.

                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
                #endregion

                //configuring services
                builder.Services.AddScoped<ICourseCategoryRepository, CourseCategoryRepository>();
                builder.Services.AddScoped<ICourseCategoryService, CourseCategoryService>();
                builder.Services.AddScoped<ICourseService, CourseService>();
                builder.Services.AddScoped<ICourseRepository, CourseRepository>();

                builder.Services.AddTransient<RequestBodyLoggingMiddleware>();
                builder.Services.AddTransient<ResponseBodyLoggingMiddleware>();

                builder.Services.AddCors(options =>
                {
                    options.AddPolicy(name: "AllowOrigin", builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    });
                });

                #region Middlewares
                var app = builder.Build();

                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        var exception = exceptionHandlerPathFeature?.Error;

                        Log.Error(exception, "Unhandled exception occurred. {ExceptionDetails}", exception?.ToString());
                        Console.WriteLine(exception?.ToString());
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
                    });
                });

                app.UseMiddleware<RequestResponseLoggingMiddleware>();
                app.UseMiddleware<RequestBodyLoggingMiddleware>();
                app.UseMiddleware<ResponseBodyLoggingMiddleware>();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

               // #region AD B2C
               // app.UseAuthorization();
                app.UseAuthorization();
                /*app.UseCors(builder => builder.WithOrigins("https://localhost:7013;http://localhost:5294") // Adjust the URL as needed
                           .AllowAnyHeader() .AllowAnyMethod());*/
               // #endregion  AD B2C

                app.MapControllers();
                app.UseCors("AllowOrigin");

                app.Run();
                #endregion Middlewares
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
