using TradingSystem;

namespace TradingSystem;

public class Order : Base
{
    public Guid OrderNumber { get; }

    public string Instrument { get; }

    public string OrderName { get; }

    public Order(string instrument, string orderName)
    {
        Instrument = instrument;
        OrderName = orderName;
        OrderNumber = Guid.NewGuid();
    }
}