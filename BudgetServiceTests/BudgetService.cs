using System;
using System.Linq;

namespace BudgetService
{
    using System.Collections.Generic;


    public class BudgetService
    {
        public IBudgetRepo BudgetRepo { get; set; }

        public BudgetService(IBudgetRepo budgetRepo)
        {
            BudgetRepo = budgetRepo;
        }

        public double Query(DateTime start, DateTime end)
        {
            var comp = DateTime.Compare(start, end);
            if (comp > 0)
            {
                return 0;
            }
            else
            {
                double amount = 0;
                var currentDate = new DateTime(start.Year, start.Month + 1, 1);
                while (currentDate < end.AddMonths(-1))
                {
                    amount += BudgetRepo.GetAll().Where(n => n.YearMonth == currentDate.ToString($"yyyyMM"))
                        .Select(n => n.Amount).FirstOrDefault();
                    currentDate = currentDate.AddMonths(1);
                }
                
                if (start.Year == end.Year && start.Month == end.Month)
                {
                   var alldays= DateTime.DaysInMonth(start.Year, start.Month);
                   
                    return BudgetRepo.GetAll().Where(n => n.YearMonth == start.ToString("yyyyMM")).Select(n => n.Amount).FirstOrDefault()* (double)((end.Day-start.Day+1)/(double)alldays);
                }
                else
                {
                    
                  amount+= BudgetRepo.GetAll().Where(n => n.YearMonth == start.ToString("yyyyMM")).Select(n => n.Amount).FirstOrDefault()* (double)((DateTime.DaysInMonth(start.Year,start.Month)-start.Day+1)/(double)DateTime.DaysInMonth(start.Year, start.Month));
                  amount+= BudgetRepo.GetAll().Where(n => n.YearMonth == end.ToString("yyyyMM")).Select(n => n.Amount).FirstOrDefault()* (double)((end.Day)/(double)DateTime.DaysInMonth(end.Year, end.Month));
                  return amount;
                }
            }
            

            

            return 0;
        }
    }
}