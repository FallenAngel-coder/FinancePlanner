using System;

namespace FinancePlanner.Factories
{
    public class TransactionFactory
    {
        public Transaction CreateIncome(decimal amount, DateTime date, string description, int categoryId)
        {
            return new Transaction
            {
                Amount = amount,
                Date = date,
                Description = description,
                CategoryId = categoryId,
                Type = "Income"
            };
        }

        public Transaction CreateExpense(decimal amount, DateTime date, string description, int categoryId)
        {
            return new Transaction
            {
                Amount = amount,
                Date = date,
                Description = description,
                CategoryId = categoryId,
                Type = "Expense"
            };
        }
    }
}
