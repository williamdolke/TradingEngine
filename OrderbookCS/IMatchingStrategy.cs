using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook
{
    public interface IMatchingStrategy
    {
        bool Match(Orderbook orderbook, Order order);
    }
}
