namespace TradingEngineServer.Orders
{
    public enum ModifyOrderStatus
    {
        Success,
        NotFound,
        InvalidPrice,
        InvalidQuantity,
        AlreadyCancelled,
        Error
    }
}
