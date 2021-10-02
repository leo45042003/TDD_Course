using System.Collections.Generic;

namespace BudgetService
{
    public interface IBudgetRepo
    {
        public List<Budget> GetAll();
    }
}