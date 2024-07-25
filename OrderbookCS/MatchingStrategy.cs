namespace TradingEngineServer.Orderbook
{
    public class MatchingStrategy : IMatchingStrategy
    {
        public MatchResult Match(Orderbook orderbook)
        {
            return MatchResult.Error;
        }
    }
}
