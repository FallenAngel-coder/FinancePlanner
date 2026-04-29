using System;
using System.Collections.Generic;
using System.Linq;

namespace FinancePlanner.Services.Strategies
{
    public class MonthlyBalanceStrategy : IBalanceCalculationStrategy
    {
        private int _month;
        private int _year;

        public MonthlyBalanceStrategy(int year, int month)
        {
            _year = year;
            _month = month;
        }

        public decimal Calculate(List<Transaction> transactions)
        {
            var monthlyTransactions = transactions.Where(t => t.Date.Year == _year && t.Date.Month == _month);
            decimal income = monthlyTransactions.Where(t => t.Type == "Income").Sum(t => t.Amount);
            decimal expense = monthlyTransactions.Where(t => t.Type == "Expense").Sum(t => t.Amount);
            return income - expense;
        }
    }
}
