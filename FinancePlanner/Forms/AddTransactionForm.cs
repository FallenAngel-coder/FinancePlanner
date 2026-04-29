using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FinancePlanner.Repositories;

namespace FinancePlanner.Forms
{
    public class AddTransactionForm : Form
    {
        public decimal Amount { get; private set; }
        public string Description { get; private set; }
        public int CategoryId { get; private set; }

        private TextBox txtAmount;
        private TextBox txtDescription;
        private ComboBox cmbCategory;
        private string _internalTransactionType; // "Income" or "Expense"
        private CategoryRepository _categoryRepo;

        public AddTransactionForm(string transactionType)
        {
            // Map "дохід"/"витрату" to "Income"/"Expense"
            _internalTransactionType = transactionType.ToLower() == "дохід" ? "Income" : "Expense";
            _categoryRepo = new CategoryRepository();

            Text = $"Додати {transactionType}";
            Size = new Size(350, 260);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            // Сума
            Label lblAmount = new Label { Text = "Сума:", Location = new Point(30, 30), AutoSize = true };
            txtAmount = new TextBox { Location = new Point(100, 28), Width = 180 };

            // Категорія
            Label lblCategory = new Label { Text = "Категорія:", Location = new Point(30, 70), AutoSize = true };
            cmbCategory = new ComboBox { Location = new Point(100, 68), Width = 140, DropDownStyle = ComboBoxStyle.DropDownList };
            
            Button btnAddCategory = new Button { Text = "+", Location = new Point(250, 67), Width = 30 };
            btnAddCategory.Click += BtnAddCategory_Click;

            // Опис
            Label lblDescription = new Label { Text = "Опис:", Location = new Point(30, 110), AutoSize = true };
            txtDescription = new TextBox { Location = new Point(100, 108), Width = 180 };

            // Кнопки
            Button btnOk = new Button { Text = "Зберегти", Location = new Point(60, 160), Width = 90 };
            btnOk.Click += BtnOk_Click;

            Button btnCancel = new Button { Text = "Скасувати", Location = new Point(170, 160), Width = 90 };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.Add(lblAmount);
            Controls.Add(txtAmount);
            Controls.Add(lblCategory);
            Controls.Add(cmbCategory);
            Controls.Add(btnAddCategory);
            Controls.Add(lblDescription);
            Controls.Add(txtDescription);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);

            LoadCategories();
        }

        private void LoadCategories()
        {
            var categories = _categoryRepo.GetByType(_internalTransactionType);
            
            // Створюємо список, де перший елемент - порожній
            var displayList = new List<Category> { new Category { Id = 0, Name = "(Немає)" } };
            displayList.AddRange(categories);

            cmbCategory.DataSource = displayList;
            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "Id";
            cmbCategory.SelectedIndex = 0;
        }

        private void BtnAddCategory_Click(object sender, EventArgs e)
        {
            using (var form = new AddCategoryForm(_internalTransactionType))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadCategories();
                }
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtAmount.Text, out decimal amount) && amount > 0)
            {
                Amount = amount;
                Description = txtDescription.Text.Trim();
                CategoryId = (int)cmbCategory.SelectedValue;
                
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Введіть коректну суму більше нуля!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
