namespace TradingEngineServer.Orderbook
{
    /// <summary>
    /// Interface that allows you to modify the state of the Orderbook via a matching algorithm. 
    /// </summary>
    public interface IMatchingOrderbook : IRetrievalOrderbook
    {
        MatchResult Match();
    }
}
