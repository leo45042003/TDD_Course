using System;
using System.Linq;

namespace BudgetService
{
    public class BudgetService
    {
        public IBudgetRepo BudgetRepo { get; set; }

        public BudgetService(IBudgetRepo budgetRepo)
        {
            BudgetRepo = budgetRepo;
        }

        public double Query(DateTime start, DateTime end)
        {
            if (start > end)
            {
                return 0;
            }

            if (start.Year == end.Year && start.Month == end.Month)
            {
                return GetBudgetOfTheMonth(start);
            }

            double amount = 0;
            amount += GetBudgetOfTheFirstMonth(start);
            amount += GetBudgetOfTheMiddleMonth(start, end);
            amount += GetBudgetOfTheLastMonth(end);

            return amount;
        }

        private double GetBudgetOfTheFirstMonth(DateTime start) 
        {
            return GetBudgetInTheSameMonthFromStart(start);
        }

        private double GetBudgetOfTheMiddleMonth(DateTime start, DateTime end)
        {
            double amount = 0;
            var currentDate = new DateTime(start.Year, start.Month + 1, 1);
            while (currentDate.Month < end.Month)
            {
                amount += GetBudgetInTheSameMonthFromStartToEnd(new DateTime(start.Year, start.Month, 1), new DateTime(start.Year, start.Month, DateTime.DaysInMonth(start.Year, start.Month)));
                currentDate = currentDate.AddMonths(1);
            }

            return amount;
        }

        private double GetBudgetOfTheLastMonth(DateTime end)
        {
            return GetBudgetInTheSameMonthToEnd(end);
        }

        private double GetBudgetInTheSameMonthFromStart(DateTime start) 
        {
            var endOfTheMonth = new DateTime(start.Year, start.Month, DateTime.DaysInMonth(start.Year, start.Month));
            return GetBudgetInTheSameMonthFromStartToEnd(start, endOfTheMonth);
        }

        private double GetBudgetInTheSameMonthToEnd(DateTime end)
        {
            var startOfTheMonth = new DateTime(end.Year, end.Month, 1);
            return GetBudgetInTheSameMonthFromStartToEnd(startOfTheMonth, end);
        }

        private double GetBudgetInTheSameMonthFromStartToEnd(DateTime start, DateTime end) 
        {
            double monthBudget = GetBudgetOfTheMonth(start);
            var days = end.Day - start.Day + 1;
            var daysInMonth = DateTime.DaysInMonth(start.Year, start.Month);

            return monthBudget * days / daysInMonth;
        }

        private double GetBudgetOfTheMonth(DateTime date)
        {
            return BudgetRepo.GetAll().Where(n => n.YearMonth == date.ToString("yyyyMM")).Select(n => n.Amount).FirstOrDefault();
        }
    }
}