using TradingEngineServer.Instrument;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook
{
    /// <summary>
    /// Base Orderbook class that stores the state of Orders.
    /// </summary>
    public class Orderbook : IRetrievalOrderbook
    {
        // PRIVATE FIELDS //
        private readonly Security _instrument;
        private readonly object _lock = new object();
        private readonly Dictionary<long, OrderbookEntry> _orders = new Dictionary<long, OrderbookEntry>();
        // nlog(n) retrieval times so better than linear searches
        private readonly SortedSet<Limit> _askLimits = new SortedSet<Limit>(AskLimitComparer.Comparer);
        private readonly SortedSet<Limit> _bidLimits = new SortedSet<Limit>(BidLimitComparer.Comparer);

        public Orderbook(Security instrument)
        {
            _instrument = instrument;
        }

        public int Count => _orders.Count;

        public void AddOrder(Order order)
        {
            var baseLimit = new Limit(order.Price);
            lock (_lock)
            {
                // Attempt to match the order first
                if (!MatchOrder(order))
                {
                    // If the order is not fully matched, add it to the order book
                    AddOrderToBook(order, baseLimit, order.IsBuySide ? _bidLimits : _askLimits, _orders);
                }
            }
        }

        private bool MatchOrder(Order order)
        {
            var oppositeLimits = order.IsBuySide ? _askLimits : _bidLimits;
            var matched = false;

            foreach (var limit in oppositeLimits)
            {
                if ((order.IsBuySide && order.Price >= limit.Price) ||
                    (!order.IsBuySide && order.Price <= limit.Price))
                {
                    var currentOrder = limit.Head;
                    while (currentOrder != null)
                    {
                        var matchedQuantity = Math.Min(order.CurrentQuantity, currentOrder.CurrentOrder.CurrentQuantity);
                        ExecuteTrade(order, currentOrder, matchedQuantity);

                        order.decreaseQuantity(matchedQuantity);
                        currentOrder.CurrentOrder.decreaseQuantity(matchedQuantity);

                        if (currentOrder.CurrentOrder.CurrentQuantity == 0)
                        {
                            RemoveOrder(currentOrder.CurrentOrder.ToCancelOrder());
                        }

                        if (order.CurrentQuantity == 0)
                        {
                            matched = true;
                            break;
                        }

                        currentOrder = currentOrder.Next;
                    }
                }

                if (order.CurrentQuantity == 0)
                {
                    break;
                }
            }

            return matched;
        }

        private void AddOrderToBook(Order order, Limit baseLimit, SortedSet<Limit> limitLevels, Dictionary<long, OrderbookEntry> internalBook)
        {
            if (limitLevels.TryGetValue(baseLimit, out Limit limit))
            {
                OrderbookEntry orderbookEntry = new OrderbookEntry(order, baseLimit);
                if (limit.Head == null)
                {
                    limit.Head = orderbookEntry;
                    limit.Tail = orderbookEntry;
                }
                else
                {
                    OrderbookEntry tailPointer = limit.Tail;
                    tailPointer.Next = orderbookEntry;
                    orderbookEntry.Previous = tailPointer;
                    limit.Tail = orderbookEntry;
                }
                internalBook.Add(order.OrderId, orderbookEntry);
            }
            else
            {
                // Limit doesn't exist yet in the orderbook
                limitLevels.Add(baseLimit);
                OrderbookEntry orderbookEntry = new OrderbookEntry(order, baseLimit);
                baseLimit.Head = orderbookEntry;
                baseLimit.Tail = orderbookEntry;
                internalBook.Add(order.OrderId, orderbookEntry);
            }
        }

        private void ExecuteTrade(Order incomingOrder, OrderbookEntry existingOrder, uint matchedQuantity)
        {
            // Placeholder for the trade execution logic
            // Update the trades, notify the parties involved, etc.
            Console.WriteLine($"Executed trade: {matchedQuantity} units at {existingOrder.CurrentOrder.Price} price.");
        }

        public void ChangeOrder(ModifyOrder modifyOrder)
        {
            lock (_lock)
            {
                if (_orders.TryGetValue(modifyOrder.OrderId, out OrderbookEntry orderbookEntry))
                {
                    RemoveOrder(modifyOrder.ToCancelOrder());
                    AddOrder(modifyOrder.ToNewOrder());
                }
            }
        }

        public bool ContainsOrder(long orderId)
        {
            return _orders.ContainsKey(orderId);
        }

        public List<OrderbookEntry> GetAskOrders()
        {
            List<OrderbookEntry> orderbookEntries = new List<OrderbookEntry>();
            foreach (var askLimit in _askLimits)
            {
                if (askLimit.IsEmpty)
                    continue;

                var askLimitPointer = askLimit.Head;
                while (askLimitPointer != null)
                {
                    orderbookEntries.Add(askLimitPointer);
                    askLimitPointer = askLimitPointer.Next;
                }
            }
            return orderbookEntries;
        }

        public List<OrderbookEntry> GetBidOrders()
        {
            List<OrderbookEntry> orderbookEntries = new List<OrderbookEntry>();
            foreach (var bidLimit in _bidLimits)
            {
                if (bidLimit.IsEmpty)
                    continue;

                var bidLimitPointer = bidLimit.Head;
                while (bidLimitPointer != null)
                {
                    orderbookEntries.Add(bidLimitPointer);
                    bidLimitPointer = bidLimitPointer.Next;
                }
            }
            return orderbookEntries;
        }

        public OrderbookSpread GetSpread()
        {
            long? bestAsk = _askLimits.Any() && !_askLimits.Min.IsEmpty ? _askLimits.Min.Price : (long?)null;
            long? bestBid = _bidLimits.Any() && !_bidLimits.Max.IsEmpty ? _bidLimits.Max.Price : (long?)null;
            return new OrderbookSpread(bestBid, bestAsk);
        }

        public void RemoveOrder(CancelOrder cancelOrder)
        {
            lock (_lock)
            {
                if (_orders.TryGetValue(cancelOrder.OrderId, out var orderbookEntry))
                {
                    RemoveOrder(cancelOrder.OrderId, orderbookEntry, _orders);
                }
            }
        }

        private static void RemoveOrder(long orderId, OrderbookEntry orderbookEntry, Dictionary<long, OrderbookEntry> internalBook)
        {
            // Deal with the location of OrderbookEntry within the LinkedList
            if (orderbookEntry.Previous != null && orderbookEntry.Next != null)
            {
                orderbookEntry.Next.Previous = orderbookEntry.Previous;
                orderbookEntry.Previous.Next = orderbookEntry.Next;
            }
            else if (orderbookEntry.Previous != null)
            {
                orderbookEntry.Previous.Next = null;
            }
            else if (orderbookEntry.Next != null)
            {
                orderbookEntry.Next.Previous = null;
            }

            // Deal with OrderbookEntry on Limit-Level
            if (orderbookEntry.ParentLimit.Head == orderbookEntry && orderbookEntry.ParentLimit.Tail == orderbookEntry)
            {
                orderbookEntry.ParentLimit.Head = null;
                orderbookEntry.ParentLimit.Tail = null;
            }
            else if (orderbookEntry.ParentLimit.Head == orderbookEntry)
            {
                // More than one order, but orderbookEntry is the first order.
                orderbookEntry.ParentLimit.Head = orderbookEntry.Next;
            }
            else if (orderbookEntry.ParentLimit.Tail == orderbookEntry)
            {
                // More than one order, but orderbookEntry is the last order.
                orderbookEntry.ParentLimit.Tail = orderbookEntry.Previous;
            }

            internalBook.Remove(orderId);
        }
    }
}
