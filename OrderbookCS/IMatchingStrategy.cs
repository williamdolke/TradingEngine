namespace TradingEngineServer.Orderbook
{
    public interface IMatchingStrategy
    {
        MatchResult Match(Orderbook orderbook);
    }
}
