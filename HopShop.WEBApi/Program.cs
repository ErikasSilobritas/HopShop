
using DbUp;
using HopShop.WEBApi.Contexts;
using HopShop.WEBApi.Interfaces;
using HopShop.WEBApi.Middlewares;
using HopShop.WEBApi.Repositories;
using HopShop.WEBApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
            builder.Services.AddSwaggerGen();

            //builder.Services.AddDbContext<DataContext>(o => o.UseInMemoryDatabase("MyDatabase"));
            builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));

            builder.Services.AddTransient<ItemService>();
            builder.Services.AddScoped<IItemRepository, ItemRepositoryForEFCore>();

            builder.Services.AddTransient<IDbConnection>(sp => new NpgsqlConnection(connectionString));


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
