using System;
using System.Drawing;
using System.Windows.Forms;
using FinancePlanner.Repositories;

namespace FinancePlanner.Forms
{
    public class AddCategoryForm : Form
    {
        private string _type;
        private TextBox txtName;

        public AddCategoryForm(string type)
        {
            _type = type;
            Text = "Додати категорію";
            Size = new Size(300, 150);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            Label lblName = new Label { Text = "Назва:", Location = new Point(20, 20), AutoSize = true };
            txtName = new TextBox { Location = new Point(80, 18), Width = 180 };

            Button btnOk = new Button { Text = "Зберегти", Location = new Point(50, 70), Width = 90 };
            btnOk.Click += BtnOk_Click;

            Button btnCancel = new Button { Text = "Скасувати", Location = new Point(150, 70), Width = 90 };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.Add(lblName);
            Controls.Add(txtName);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            if (!string.IsNullOrEmpty(name))
            {
                var repo = new CategoryRepository();
                repo.Add(new Category { Name = name, Type = _type });
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Введіть назву категорії!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
