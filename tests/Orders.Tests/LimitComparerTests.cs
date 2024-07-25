using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

using TradingEngineServer.Orders;

namespace OrdersTests
{
    public class TestBidLimitComparer
    {
        [Fact]
        public void Compare_XGreaterThanY_ReturnsMinus1()
        {
            Limit x = new Limit(10);
            Limit y = new Limit(9);

            var comparer = new BidLimitComparer();

            var res = comparer.Compare(x, y);

            Assert.True(res == -1);
        }

        [Fact]
        public void Compare_XLessThanY_Returns1()
        {
            Limit y = new Limit(10);
            Limit x = new Limit(9);

            var comparer = new BidLimitComparer();

            var res = comparer.Compare(x, y);

            Assert.True(res == 1);
        }

        [Fact]
        public void Compare_XEqualY_Returns0()
        {
            Limit x = new Limit(10);
            Limit y = new Limit(10);

            var comparer = new BidLimitComparer();

            var res = comparer.Compare(x, y);

            Assert.True(res == 0);
        }
    }

    public class TestAskLimitComparer
    {
        [Fact]
        public void Compare_XGreaterThanY_Returns1()
        {
            Limit x = new Limit(10);
            Limit y = new Limit(9);

            var comparer = new AskLimitComparer();

            var res = comparer.Compare(x, y);

            Assert.True(res == 1);
        }

        [Fact]
        public void Compare_XLessThanY_ReturnsMinus1()
        {
            Limit y = new Limit(10);
            Limit x = new Limit(9);

            var comparer = new AskLimitComparer();

            var res = comparer.Compare(x, y);

            Assert.True(res == -1);
        }

        [Fact]
        public void Compare_XEqualY_Returns0()
        {
            Limit x = new Limit(10);
            Limit y = new Limit(10);

            var comparer = new AskLimitComparer();

            var res = comparer.Compare(x, y);

            Assert.True(res == 0);
        }
    }
}