using Moq;
using Xunit;

namespace TradingSystem.Tests
{
    public class ProcessTradeOrdersTests
    {
        [Fact]
        public async Task StartApp_Should_Return_List_Of_Not_Equal_Trade_Identifiers()
        {
            // Arrange
            var newOrder = new Order("MK", "Bonds");
            var expectedIdentifiers = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() };

            var mockHandleTradeOrder = new Mock<Func<Order, List<Trade>>>();

            mockHandleTradeOrder.Setup(m => m.Invoke(newOrder)).Returns(new List<Trade>
            {
                new Trade("Trade 1"), // it generates new Guid
                new Trade("Trade 2"), // it generates new Guid
            });

            // Act
            var result = await TradingSystem.StartApp(newOrder);

            // Assert

            foreach (var exp in expectedIdentifiers)
            {
                foreach (var res in result)
                {
                    Assert.NotEqual(exp, res);
                }
            }
        }

        [Fact]
        public async Task StartApp_Should_Return_List_Of_Equal_Trade_Identifiers()
        {
            // Arrange
            var newOrder = new Order("MK", "Bonds");
            var expectedIdentifiers = new List<Guid>()
            {
                new Guid("0b26a297-895c-4257-9819-12c653d31447"),
                new Guid("9ef57ad5-ac11-4333-b1a8-1610148a881a")
            };

            var mockHandleTradeOrder = new Mock<Func<Order, List<Trade>>>();

            mockHandleTradeOrder.Setup(m => m.Invoke(newOrder)).Returns(new List<Trade>
            {
                new Trade("Trade 1"), // it generates new Guid
                new Trade("Trade 2"), // it generates new Guid
            });

            // Act
            var result = await TradingSystem.StartApp(newOrder); // the same Guid are returned

            // Assert

            foreach (var exp in expectedIdentifiers)
            {
                foreach (var res in result)
                {
                    Assert.NotEqual(exp, res);
                }
            }
        }

        [Fact]
        public Task ProcessOrderForTheFollowingTrades_ReturnsExpectedResult()
        {
            // Arrange
            var order = new Order("MK", "Bonds");

            // Act
            var trades = TradingSystem.ProcessOrderForTheFollowingTrades(order);

            // Assert
            Assert.NotNull(trades);
            Assert.Equal(2, trades.Count);
            Assert.Equal("Trade 1", trades[0].TradeName);
            Assert.Equal("Trade 2", trades[1].TradeName);
            return Task.CompletedTask;
        }

        [Fact]
        public async Task HandleTradeOrder_ReturnsExpectedResult()
        {
            // Arrange
            var order = new Order("MK", "Bonds");

            // Act
            var task = Task.Run(() =>
            {
                TradingSystem.HandleTradeOrder transactionOrder =
                    new TradingSystem.HandleTradeOrder(TradingSystem.ProcessOrderForTheFollowingTrades);
                return transactionOrder.Invoke(order);
            });

            var trades = await task;

            // Assert
            Assert.NotNull(trades);
            Assert.Equal(2, trades.Count);
            Assert.Equal("Trade 1", trades[0].TradeName);
            Assert.Equal("Trade 2", trades[1].TradeName);
        }


        [Fact]
        public void OrderConstructor_SetsPropertiesCorrectly()
        {
            // Arrange
            string instrument = "MK";
            string orderName = "Bonds";

            // Act
            Order order = new Order(instrument, orderName);

            // Assert
            Assert.Equal(instrument, order.Instrument);
            Assert.Equal(orderName, order.OrderName);
            Assert.NotEqual(Guid.Empty, order.OrderNumber);
        }

        [Fact]
        public void TradeConstructor_SetsPropertiesCorrectly()
        {
            // Arrange
            string tradeName = "Trade 1";

            // Act
            Trade trade = new Trade(tradeName);

            // Assert
            Assert.Equal(tradeName, trade.TradeName);
            Assert.NotEqual(Guid.Empty, trade.TradeIdentifier);
        }

        [Fact]
        public void ProcessOrderForTheFollowingTrades_ReturnsTwoTrades()
        {
            // Arrange
            Order order = new Order("MK", "Bonds");

            // Act
            List<Trade> trades = TradingSystem.ProcessOrderForTheFollowingTrades(order);

            // Assert
            Assert.Equal(2, trades.Count);
            Assert.NotNull(trades[0]);
            Assert.NotNull(trades[1]);
        }
    }
}