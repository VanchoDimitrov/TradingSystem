using System.Collections;

namespace TradingSystem;

public static class TradingSystem
{
    public delegate List<Trade> HandleTradeOrder(Order order);

    public static async Task Main(string[] args)
    {

        Order newOrder = new Order("MK", "Bonds");

        await StartApp(newOrder);

        Console.ReadKey();
    }

    public static async Task<List<Guid>> StartApp(Order newOrder)
    {
        var identifiers = new List<Guid>();

        HandleTradeOrder transactionOrder = new HandleTradeOrder(ProcessOrderForTheFollowingTrades);

        List<Trade> orderResult = transactionOrder.Invoke(newOrder);

        List<Trade> trades = await Task.FromResult(orderResult);

        foreach (var trade in trades)
        {
            identifiers.Add(trade.TradeIdentifier);
        }
        return identifiers;
    }

    public static List<Trade> ProcessOrderForTheFollowingTrades(Order order)
    {
        var trades1 = new Trade("Trade 1");

        var trades2 = new Trade("Trade 2");

        return new List<Trade> { trades1, trades2 };
    }
}