namespace TradingEngineServer.Orderbook
{
    /// <summary>
    /// Interface to read from the Orderbook. 
    /// </summary>
    public interface IReadOnlyOrderBook
    {
        bool ContainsOrder(long orderId);
        OrderbookSpread GetSpread();
        int Count { get; }
    }
}
