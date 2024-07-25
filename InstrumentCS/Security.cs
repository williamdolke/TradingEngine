namespace TradingEngineServer.Instrument
{
    public class Security
    {
        public string Symbol { get; }
        public Security(string symbol)
        {
            Symbol = symbol;
        }
    }
}
