using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using N = NUnit.Framework;

using TradingEngineServer.Instrument;
using TradingEngineServer.Orders;
using TradingEngineServer.Orderbook;

namespace OrderbookTests
{
    [N.TestFixture]
    public class OrderbookTests
    {
        [N.Test]
        public void TestAddOrders()
        {
            Security instrument = new Security("TestInstrument");
            Orderbook orderbook = new Orderbook(instrument);

            List<Order> testOrders = GenerateTestOrders();
            for (int i = 0; i < testOrders.Count; i++)
            {
                orderbook.AddOrder(testOrders[i]);
            }

            N.Assert.AreEqual(3, orderbook.Count);

            var askOrders = orderbook.GetAskOrders();
            var bidOrders = orderbook.GetBidOrders();

            N.Assert.AreEqual(2, askOrders.Count);
            N.Assert.AreEqual(1, bidOrders.Count);

            OrderbookSpread spread = orderbook.GetSpread();
            N.Assert.AreEqual(99, spread.Bid);
            N.Assert.AreEqual(100, spread.Ask);
        }

        private List<Order> GenerateTestOrders()
        {
            IOrderCore orderCore1 = new OrderCore(1, "user1", 1);
            IOrderCore orderCore2 = new OrderCore(2, "user2", 1);
            IOrderCore orderCore3 = new OrderCore(3, "user3", 1);
            IOrderCore orderCore4 = new OrderCore(4, "user4", 1);

            return new List<Order> 
            {
                new Order(orderCore1, 100, 10, true),
                new Order(orderCore2, 101, 5, false),
                new Order(orderCore3, 99, 15, true),
                new Order(orderCore4, 100, 20, false),
            };
        }
    }
}
