using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook
{
    public class MatchResult
    {
        public List<Order> MatchedOrders { get; private set; }
        public List<Trade> Trades { get; private set; }

        public MatchResult()
        {
            MatchedOrders = new List<Order>();
            Trades = new List<Trade>();
        }

        public void AddTrade(Trade trade)
        {
            Trades.Add(trade);
        }

        public void AddMatchedOrder(Order order)
        {
            if (!MatchedOrders.Contains(order))
            {
                MatchedOrders.Add(order);
            }
        }
    }
}
