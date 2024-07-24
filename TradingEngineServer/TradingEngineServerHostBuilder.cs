using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using TradingEngineServer.Core.Configuration;
using TradingEngineServer.Logging;
using TradingEngineServer.Logging.LoggingConfiguration;

namespace TradingEngineServer.Core
{
    public sealed class TradingEngineServerHostBuilder
    {
        public static IHost buildTradingEngineServer() => Host.CreateDefaultBuilder().ConfigureServices((hostContext, services) =>
        {
            // Configuration
            services.AddOptions();
            services.Configure<TradingEngineServerConfiguration>(hostContext.Configuration.GetSection(nameof(TradingEngineServerConfiguration)));
            services.Configure<LoggerConfiguration>(hostContext.Configuration.GetSection(nameof(LoggerConfiguration)));

            // Singletons
            services.AddSingleton<ITradingEngineServer, TradingEngineServer>();
            services.AddSingleton<ITextLogger, TextLogger>();

            // Add hosted service
            services.AddHostedService<TradingEngineServer>();
        }).Build();
    }
}
