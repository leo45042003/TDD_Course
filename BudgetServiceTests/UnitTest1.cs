using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Xunit;

namespace BudgetService
{
    public class UnitTest1
    {
        private Mock<IBudgetRepo> MockIBudgetRepo { get; set; } = new Mock<IBudgetRepo>();
        
        [Fact]
        public void InvalidDate_ReturnZero()
        {
            MockIBudgetRepo.Setup(x => x.GetAll())
                .Returns(new List<Budget> { new Budget
                {
                    YearMonth = "202109", Amount = 300
                }});

            var startDate = new DateTime(2021, 9, 2);
            var endDate = new DateTime(2021, 9, 1);

            var result = Query(startDate, endDate);

            result.Should().Be(0);
        }
        
        [Fact]
        public void QueryADay_ReturnOneDayData()
        {
            MockIBudgetRepo.Setup(x => x.GetAll())
                .Returns(new List<Budget> { new Budget
                {
                    YearMonth = "202109", Amount = 300
                }});

            var startDate = new DateTime(2021, 9, 1);
            var endDate = new DateTime(2021, 9, 1);

            var result = Query(startDate, endDate);

            result.Should().Be(10);
        }
        
        [Fact]
        public void QueryCrossDay_ReturnCrossDayData()
        {
            MockIBudgetRepo.Setup(x => x.GetAll())
                .Returns(new List<Budget> { new Budget
                {
                    YearMonth = "202109", Amount = 300
                }});

            var startDate = new DateTime(2021, 9, 1);
            var endDate = new DateTime(2021, 9, 2);

            var result = Query(startDate, endDate);

            result.Should().Be(20);
        }
        
        [Fact]
        public void QueryMonth_ReturnMonthData()
        {
            MockIBudgetRepo.Setup(x => x.GetAll())
                .Returns(new List<Budget> { new Budget
                {
                    YearMonth = "202109", Amount = 300
                }});

            var startDate = new DateTime(2021, 9, 1);
            var endDate = new DateTime(2021, 9, 30);

            var result = Query(startDate, endDate);

            result.Should().Be(300);
        }
        
        [Fact]
        public void QueryCrossonth_ReturnCrossMonthData()
        {
            MockIBudgetRepo.Setup(x => x.GetAll())
                .Returns(new List<Budget>
                {
                    new Budget { YearMonth = "202109", Amount = 300 },
                    new Budget { YearMonth = "202110", Amount = 3100 },
                    new Budget { YearMonth = "202111", Amount = 30 }
                });

            var startDate = new DateTime(2021, 9, 1);
            var endDate = new DateTime(2021, 11, 4);

            var result = Query(startDate, endDate);

            result.Should().Be(300+3100+4);
        }

        private double Query(DateTime startDate, DateTime endDate)
        {
            return new BudgetService(MockIBudgetRepo.Object).Query(startDate, endDate);
        }
    }
}