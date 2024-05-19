
using Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Services;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddTransient<FrankfurterApi>();
            builder.Services.AddTransient<IFrankfurterApi>(x =>
            {
                var frankfurterApi = x.GetService<FrankfurterApi>();
                var memoryCache = x.GetService<IMemoryCache>();
                return new CachedFrankfurterApi(frankfurterApi, memoryCache);

            });
            builder.Services.AddTransient<IExchangeRatesManager, ExchangeRatesManager>();

            builder.Services.AddMemoryCache();
            builder.Services.AddHttpClient();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
