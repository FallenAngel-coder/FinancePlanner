using System.Collections.Generic;
using System.Linq;

namespace FinancePlanner.Services.Strategies
{
    public class ExpensesOnlyStrategy : IBalanceCalculationStrategy
    {
        public decimal Calculate(List<Transaction> transactions)
        {
            return transactions.Where(t => t.Type == "Expense").Sum(t => t.Amount);
        }
    }
}
