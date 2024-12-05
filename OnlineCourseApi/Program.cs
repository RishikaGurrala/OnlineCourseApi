
using Microsoft.EntityFrameworkCore;
using OnlineCourseApi.Data;
using OnlineCourseApi.Data.Entities;
using OnlineCourseApi.Service;

namespace OnlineCourseApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            if (builder.Environment.IsDevelopment()) { 
                builder.Configuration.AddJsonFile
                    ("appsettings.Development.json", optional: true, reloadOnChange: true);
            } 
            else { 
                builder.Configuration.AddJsonFile
                    ("appsettings.json", optional: false, reloadOnChange: true); 
            }
            var configuration = builder.Configuration;
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

            //configuring services
            builder.Services.AddScoped<ICourseCategoryRepository, CourseCategoryRepository>();
            builder.Services.AddScoped<ICourseCategoryService, CourseCategoryService>();
            builder.Services.AddScoped<ICourseService, CourseService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            /*app.UseCors(builder => builder.WithOrigins("https://localhost:7013;http://localhost:5294") // Adjust the URL as needed
                       .AllowAnyHeader() .AllowAnyMethod());*/

            app.MapControllers();

            app.Run();
        }
    }
}
