namespace TradingEngineServer.Orderbook
{
    public class Trade
    {
        public long BuyOrderId { get; }
        public long SellOrderId { get; }
        public long Price { get; }
        public uint Quantity { get; }

        public Trade(long buyOrderId, long sellOrderId, long price, uint quantity)
        {
            BuyOrderId = buyOrderId;
            SellOrderId = sellOrderId;
            Price = price;
            Quantity = quantity;
        }
    }
}
