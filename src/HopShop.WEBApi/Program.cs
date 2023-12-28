using Application.Services;
using DbUp;
using Domain.Interfaces;
using HopShop.WEBApi.Middlewares;
using Infrastructure.Clients;
using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Data;
using System.Reflection;

namespace HopShop.WEBApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration["MySecrets:PostgreConnection"] ?? throw new ArgumentNullException("Connection string was not found.");

            EnsureDatabase.For.PostgresqlDatabase(connectionString);

            var upgrader =
                DeployChanges.To
                    .PostgresqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();


            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
            #if DEBUG
                Console.ReadLine();
            #endif
            }

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HopShop", Version = "v1" });

                // Include the XML comments file
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));

            builder.Services.AddTransient<ItemService>();
            builder.Services.AddTransient<UserService>();
            builder.Services.AddTransient<ShopService>();
            builder.Services.AddTransient<IJsonPlaceholderClient, JsonPlaceholderClient>();
            builder.Services.AddScoped<IItemRepository, ItemRepositoryForEFCore>();
            builder.Services.AddScoped<IShopRepository, ShopRepository>();


            builder.Services.AddTransient<IDbConnection>(sp => new NpgsqlConnection(connectionString));

            builder.Services.AddHttpClient();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
