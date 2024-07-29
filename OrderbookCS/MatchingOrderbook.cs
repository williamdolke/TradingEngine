using TradingEngineServer.Instrument;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook
{
    public class MatchingOrderbook : Orderbook, IMatchingOrderbook
    {
        private readonly FIFOMatchingStrategy _matchingStrategy;
        private readonly object _lock = new object();

        public MatchingOrderbook(FIFOMatchingStrategy matchingStrategy, Security instrument) : base(instrument)
        {
            _matchingStrategy = matchingStrategy;
        }

        public bool Match(Order order)
        {
            return _matchingStrategy.Match(this, order);
        }

        public override void AddOrder(Order order)
        {
            lock (_lock)
            {
                // Attempt to match the order first
                if (order.CurrentQuantity > 0 && !Match(order))
                {
                    // If the order is not fully matched, add it to the order book
                    base.AddOrder(order);
                }
            }
        }

        public override void ChangeOrder(ModifyOrder modifyOrder)
        {
            lock (_lock)
            {
                base.ChangeOrder(modifyOrder);
            }
        }

        public override void RemoveOrder(CancelOrder cancelOrder)
        {
            lock (_lock)
            {
                base.RemoveOrder(cancelOrder);
            }
        }
    }
}
