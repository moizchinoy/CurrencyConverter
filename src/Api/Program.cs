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
                var logger = x.GetService<ILogger<CachedFrankfurterApi>>();
                return new CachedFrankfurterApi(frankfurterApi, memoryCache, logger);
            });


            builder.Services.AddTransient<ExchangeRatesManager>();
            builder.Services.AddTransient<IExchangeRatesManager>(x =>
            {
                var exchangeRatesManager = x.GetService<ExchangeRatesManager>();
                return new FilteredExchangeRatesManager(
                    exchangeRatesManager,
                    [new("TRY"), new("PLN"), new("THB"), new("MXN")]);
            });

            builder.Services.AddMemoryCache();

            builder.Services.AddHttpClient<FrankfurterApi>(
                configureClient: static client =>
                {
                    client.BaseAddress = new("https://api.frankfurter.app");
                })
                .AddStandardHedgingHandler();

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
