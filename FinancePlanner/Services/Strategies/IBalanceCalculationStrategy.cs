using System.Collections.Generic;

namespace FinancePlanner.Services.Strategies
{
    public interface IBalanceCalculationStrategy
    {
        decimal Calculate(List<Transaction> transactions);
    }
}
