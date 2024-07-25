using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook
{
    public interface IOrderEntryOrderbook : IReadOnlyOrderBook
    {
        /// <summary>
        /// Interface that allows interaction with the Orderbook, e.g. creating, changing and cancelling orders.
        /// </summary>
        void AddOrder(Order order);
        void ChangeOrder(ModifyOrder modifyOrder);
        void RemoveOrder(CancelOrder cancelOrder);
    }
}
