using FarazTechTest.Services.FileService;
using FarazTechTest.Services.FolderService;
using FileDatabase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static FarazTechTest.Program;

namespace FarazTechTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<FileDatabaseContext>(
                options => options.UseNpgsql("name=ConnectionStrings:DefaultConnection"));
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            builder.Services.AddTransient<IFolderService, FolderService>();
            builder.Services.AddTransient<IFileService, FileService>();

            //For simplicity's sake I'm hard coding this, but ideally put it in appsettings.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    builder => builder
                        .WithOrigins("http://localhost:3000")
                        .WithExposedHeaders("Content-Disposition")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("AllowReactApp");

            app.MapControllers();
            app.Run();
        }

    }
}
