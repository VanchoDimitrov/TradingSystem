using TradingSystem;

namespace TradingSystem;

public class Trade : Base
{
    public Guid TradeIdentifier { get; }

    public string TradeName { get; set; }

    public Trade(string tradeName)
    {
        TradeName = tradeName;
        TradeIdentifier = Guid.NewGuid();
    }
}