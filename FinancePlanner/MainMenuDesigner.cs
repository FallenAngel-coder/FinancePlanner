using System;
using System.Drawing;
using System.Windows.Forms;
using FinancePlanner.Factories;
using FinancePlanner.Services;
using FinancePlanner.Services.Strategies;
using FinancePlanner.Forms;

namespace FinancePlanner
{
    public class MainMenuDesigner
    {
        private TransactionService _transactionService;

        public void Setup(Form mainForm)
        {
            // Ініціалізація сервісів для тестування патернів
            var factory = new TransactionFactory();
            var repository = new TransactionRepository();
            _transactionService = new TransactionService(repository, factory);

            // Налаштування головної форми
            mainForm.Text = $"Finance Planner - Головне меню ({SessionManager.Instance.CurrentUser?.Username})";
            mainForm.Size = new Size(500, 500);
            mainForm.StartPosition = FormStartPosition.CenterScreen;

            // Заголовок
            Label titleLabel = new Label();
            titleLabel.Text = "Фінансовий Планувальник";
            titleLabel.Font = new Font("Arial", 16, FontStyle.Bold);
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(100, 30);

            // Кнопка "Додати дохід"
            Button btnIncome = new Button();
            btnIncome.Text = "Додати дохід";
            btnIncome.Size = new Size(300, 40);
            btnIncome.Location = new Point(90, 80);
            btnIncome.Font = new Font("Arial", 12);
            btnIncome.Click += (sender, e) => 
            { 
                using (var addForm = new AddTransactionForm("дохід"))
                {
                    if (addForm.ShowDialog() == DialogResult.OK)
                    {
                        _transactionService.AddIncomeForDay(addForm.Amount, DateTime.Now, addForm.Description, addForm.CategoryId);
                        MessageBox.Show("Дохід додано успішно!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                    }
                }
            };

            // Кнопка "Додати витрату"
            Button btnExpense = new Button();
            btnExpense.Text = "Додати витрату";
            btnExpense.Size = new Size(300, 40);
            btnExpense.Location = new Point(90, 130);
            btnExpense.Font = new Font("Arial", 12);
            btnExpense.Click += (sender, e) => 
            { 
                using (var addForm = new AddTransactionForm("витрату"))
                {
                    if (addForm.ShowDialog() == DialogResult.OK)
                    {
                        _transactionService.AddExpenseForDay(addForm.Amount, DateTime.Now, addForm.Description, addForm.CategoryId);
                        MessageBox.Show("Витрату додано успішно!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                    }
                }
            };

            // Кнопка "Загальний баланс"
            Button btnTotalBalance = new Button();
            btnTotalBalance.Text = "Загальний баланс";
            btnTotalBalance.Size = new Size(300, 40);
            btnTotalBalance.Location = new Point(90, 180);
            btnTotalBalance.Font = new Font("Arial", 12);
            btnTotalBalance.Click += (sender, e) => 
            { 
                var balance = _transactionService.CalculateBalance(new TotalBalanceStrategy());
                MessageBox.Show($"Загальний баланс (всі транзакції): {balance}", "Баланс", MessageBoxButtons.OK, MessageBoxIcon.Information); 
            };

            // Кнопка "Баланс за поточний місяць"
            Button btnMonthlyBalance = new Button();
            btnMonthlyBalance.Text = "Баланс за цей місяць";
            btnMonthlyBalance.Size = new Size(300, 40);
            btnMonthlyBalance.Location = new Point(90, 230);
            btnMonthlyBalance.Font = new Font("Arial", 12);
            btnMonthlyBalance.Click += (sender, e) => 
            { 
                var now = DateTime.Now;
                var balance = _transactionService.CalculateBalance(new MonthlyBalanceStrategy(now.Year, now.Month));
                MessageBox.Show($"Баланс за поточний місяць ({now:MMMM}): {balance}", "Баланс", MessageBoxButtons.OK, MessageBoxIcon.Information); 
            };

            // Кнопка "Тільки витрати"
            Button btnExpensesOnly = new Button();
            btnExpensesOnly.Text = "Всього витрат";
            btnExpensesOnly.Size = new Size(300, 40);
            btnExpensesOnly.Location = new Point(90, 280);
            btnExpensesOnly.Font = new Font("Arial", 12);
            btnExpensesOnly.Click += (sender, e) => 
            { 
                var balance = _transactionService.CalculateBalance(new ExpensesOnlyStrategy());
                MessageBox.Show($"Сума всіх витрат: {balance}", "Витрати", MessageBoxButtons.OK, MessageBoxIcon.Information); 
            };

            // Кнопка "Вихід"
            Button btnExit = new Button();
            btnExit.Text = "Вихід";
            btnExit.Size = new Size(300, 40);
            btnExit.Location = new Point(90, 350);
            btnExit.Font = new Font("Arial", 12);
            btnExit.BackColor = Color.LightCoral;
            btnExit.Click += (sender, e) => 
            { 
                Application.Exit(); 
            };

            // Додавання елементів на форму
            mainForm.Controls.Add(titleLabel);
            mainForm.Controls.Add(btnIncome);
            mainForm.Controls.Add(btnExpense);
            mainForm.Controls.Add(btnTotalBalance);
            mainForm.Controls.Add(btnMonthlyBalance);
            mainForm.Controls.Add(btnExpensesOnly);
            mainForm.Controls.Add(btnExit);
        }
    }
}
