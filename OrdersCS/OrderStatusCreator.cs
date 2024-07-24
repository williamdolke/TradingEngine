namespace TradingEngineServer.Orders
{
    public class OrderStatusCreator
    {
        public static CancelOrderStatus GetCancelOrderStatus(CancelOrder cancelOrder)
        {
            return new CancelOrderStatus();
        }

        public static NewOrderStatus GenerateNewOrderStatus(Order order)
        {
            return new NewOrderStatus();
        }

        public static ModifyOrderStatus GenerateModifyOrderStatus(ModifyOrder modifyOrder)
        {
            return new ModifyOrderStatus();
        }
    }
}