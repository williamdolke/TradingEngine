namespace TradingEngineServer.Orders
{
    /// <summary>
    /// Read-only representation of an order.
    /// </summary>
    public record OrderRecord(
        long OrderId,
        uint Quantity,
        long Price,
        bool IsBuySide,
        string Username,
        int SecurityId,
        uint TheoreticalQueuePosition
    );
}