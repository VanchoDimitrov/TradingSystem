namespace TradingSystem;

public class Base
{
    private readonly int _id;

    private int GetId()
    {
        return _id;
    }

    protected Base()
    {
        _id = GetId() + 1;
    }
}