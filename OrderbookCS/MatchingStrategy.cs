using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook
{
    public class FIFOMatchingStrategy : IMatchingStrategy
    {
        public bool Match(Orderbook orderbook, Order order)
        {
            var oppositeLimits = order.IsBuySide ? orderbook._askLimits : orderbook._bidLimits;
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
                        ExecuteTrade(order, currentOrder.CurrentOrder, matchedQuantity);

                        order.decreaseQuantity(matchedQuantity);
                        currentOrder.CurrentOrder.decreaseQuantity(matchedQuantity);

                        if (currentOrder.CurrentOrder.CurrentQuantity == 0)
                        {
                            orderbook.RemoveOrder(currentOrder.CurrentOrder.ToCancelOrder());
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

        private void ExecuteTrade(Order incomingOrder, Order existingOrder, uint matchedQuantity)
        {
            var trade = new Trade(
                buyOrderId: incomingOrder.IsBuySide ? incomingOrder.OrderId : existingOrder.OrderId,
                sellOrderId: incomingOrder.IsBuySide ? existingOrder.OrderId : incomingOrder.OrderId,
                price: existingOrder.Price,
                quantity: matchedQuantity
            );

            Console.WriteLine($"Matched {matchedQuantity} units between Order {incomingOrder.OrderId} and Order {existingOrder.OrderId} at price {existingOrder.Price}");
        }
    }
}
