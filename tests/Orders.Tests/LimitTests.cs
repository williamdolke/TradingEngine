using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

using TradingEngineServer.Orders;

namespace Orders.Tests
{
    public class LimitTests
    {
        [Fact]
        public void IsEmpty_ReturnsTrueWhenHeadAndTailAreNull()
        {
            Limit x = new Limit(10);
            Assert.True(x.IsEmpty);
        }

        [Fact]
        public void Constructor_SetsPrice()
        {
            long price = 1000;
            var limit = new Limit(price);
            Assert.Equal(price, limit.Price);
        }
    }
}
