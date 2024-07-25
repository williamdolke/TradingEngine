namespace TradingEngineServer.Orders
{
    public class OrderStatusCreator
    {
        public static CancelOrderStatus GetCancelOrderStatus(CancelOrder cancelOrder)
        {
            return CancelOrderStatus.Error;
        }

        public static NewOrderStatus GenerateNewOrderStatus(Order order)
        {
            return NewOrderStatus.Error;
        }

        public static ModifyOrderStatus GenerateModifyOrderStatus(ModifyOrder modifyOrder)
        {
            return ModifyOrderStatus.Error;
        }
    }
}
