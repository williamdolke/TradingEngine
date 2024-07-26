using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using TradingEngineServer.Core.Configuration;
using TradingEngineServer.Instrument;
using TradingEngineServer.Logging;
using TradingEngineServer.Orderbook;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Core
{
    sealed class TradingEngineServer : BackgroundService, ITradingEngineServer
    {

        private readonly ITextLogger _logger;
        private readonly TradingEngineServerConfiguration _tradingEngineServerConfig;

        public TradingEngineServer(ITextLogger textLogger, IOptions<TradingEngineServerConfiguration> config)
        {
            _logger = textLogger ?? throw new ArgumentNullException(nameof(textLogger));
            _tradingEngineServerConfig = config.Value ?? throw new ArgumentNullException(nameof(config));
        }

        public Task Run(CancellationToken token) => ExecuteAsync(token);

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Information(nameof(TradingEngineServer), "Starting Trading Engine");

            FIFOMatchingStrategy matchingStrategy = new FIFOMatchingStrategy();
            Security security = new Security("Example Symbol");
            MatchingOrderbook orderbook = new MatchingOrderbook(matchingStrategy, security);

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Enter order command: ");
                _logger.Information(nameof(TradingEngineServer), "Enter order command: ");
                string input = Console.ReadLine();
                _logger.Information(nameof(TradingEngineServer), input);
                string[] parts = input.Split(' ');

                if (parts.Length < 1) continue;

                string command = parts[0].ToLower();

                try
                {
                    string username;
                    int securityId;
                    long orderId;

                    switch (command)
                    {
                        // e.g. new ID 999 1 99 10 buy
                        // e.g. new ID 999 2 101 10 sell
                        case "new":
                            if (parts.Length < 7) throw new ArgumentException("Invalid new order command");

                            username = parts[1];
                            securityId = int.Parse(parts[2]);
                            orderId = long.Parse(parts[3]);
                            long price = long.Parse(parts[4]);
                            uint quantity = uint.Parse(parts[5]);
                            bool isBuySide = parts[6].ToLower() == "buy";

                            Order newOrder = new Order(new OrderCore(orderId, username, securityId), price, quantity, isBuySide);
                            orderbook.AddOrder(newOrder);
                            break;
                        // e.g. cancel ID 999 3
                        case "cancel":
                            if (parts.Length < 4) throw new ArgumentException("Invalid cancel order command");

                            username = parts[1];
                            securityId = int.Parse(parts[2]);
                            orderId = long.Parse(parts[3]);

                            CancelOrder cancelOrder = new CancelOrder(new OrderCore(orderId, username, securityId));
                            orderbook.RemoveOrder(cancelOrder);
                            break;
                        // e.g. modify ID 999 1 98 11 buy
                        // e.g. modify ID 999 2 102 11 sell
                        case "modify":
                            if (parts.Length < 7) throw new ArgumentException("Invalid modify order command");

                            username = parts[1];
                            securityId = int.Parse(parts[2]);
                            orderId = long.Parse(parts[3]);
                            long newPrice = long.Parse(parts[4]);
                            uint newQuantity = uint.Parse(parts[5]);
                            bool buySide = parts[6].ToLower() == "buy";

                            ModifyOrder modifyOrder = new ModifyOrder(new OrderCore(orderId, username, securityId), newPrice, newQuantity, buySide);
                            orderbook.ChangeOrder(modifyOrder);
                            break;

                        default:
                            Console.WriteLine("Unknown command");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            _logger.Information(nameof(TradingEngineServer), "Stopping Trading Engine");
            return Task.CompletedTask;
        }
    }
}
