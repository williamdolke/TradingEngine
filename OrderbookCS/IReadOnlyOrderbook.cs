namespace TradingEngineServer.Orderbook
{
    public interface IReadOnlyOrderBook
    {
        bool ContainsOrder(long orderId);
        OrderbookSpread GetSpread();
        int Count { get; }
    }
}
