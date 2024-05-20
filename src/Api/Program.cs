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

            builder.Services.AddTransient<Converter>();
            builder.Services.AddTransient<IConverter>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var restrictedCurrencies = config.GetSection("RestrictedCurrencies").Get<string[]>();
                var converter = sp.GetService<Converter>();
                return new FilteredConverter(
                    converter,
                    restrictedCurrencies.Select(x => new Currency(x)));
            });

            builder.Services.AddMemoryCache();

            builder.Services.AddHttpClient<FrankfurterApi>(
                configureClient: static (sp, client) =>
                {
                    var config = sp.GetRequiredService<IConfiguration>();
                    var frankfurterApiUrl = config.GetValue<string>("FrankfurterApiUrl");
                    client.BaseAddress = new(frankfurterApiUrl);
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
