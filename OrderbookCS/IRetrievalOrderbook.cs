using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook
{
    /// <summary>
    /// Interface that allows mutation of the Orderbook. 
    /// </summary>
    public interface IRetrievalOrderbook : IOrderEntryOrderbook
    {
        List<OrderbookEntry> GetAskOrders();
        List<OrderbookEntry> GetBidOrders();
    }
}
