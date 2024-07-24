namespace TradingEngineServer.Orders
{
    public class OrderCore : IOrderCore
    {
        public OrderCore(long orderId, string username, int securityId)
        {
            OrderId = orderId;
            Username = username;
            SecurityId = securityId;
        }

        public long OrderId { get; private set; } // This is the same as only using get;
        public int SecurityId { get; private set; }
        public string Username { get; private set; }
    }
}