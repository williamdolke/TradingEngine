using TradingEngineServer.Orders;

namespace TradingEngineServer.Rejects
{
    public class RejectCreator
    {
        public static Rejection GetOrderCoreRejection(IOrderCore rejectedOrder, RejectionReason rejectionReason)
        {
            return new Rejection(rejectedOrder, rejectionReason);
        }
    }
}
