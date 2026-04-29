using System.Collections.Generic;
using System.Linq;

namespace FinancePlanner.Services.Strategies
{
    public class TotalBalanceStrategy : IBalanceCalculationStrategy
    {
        public decimal Calculate(List<Transaction> transactions)
        {
            decimal income = transactions.Where(t => t.Type == "Income").Sum(t => t.Amount);
            decimal expense = transactions.Where(t => t.Type == "Expense").Sum(t => t.Amount);
            return income - expense;
        }
    }
}
