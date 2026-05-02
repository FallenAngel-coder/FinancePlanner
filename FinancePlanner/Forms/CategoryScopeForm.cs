using System;
using System.Drawing;
using System.Windows.Forms;

namespace FinancePlanner.Forms
{
    public class CategoryScopeForm : Form
    {
        public string CategoryName { get; private set; }
        public int Scope { get; private set; } // 0=All, 1=This month, 2=From this month onwards
        
        private TextBox txtName;
        private RadioButton rbAll;
        private RadioButton rbThisMonth;
        private RadioButton rbFromNowOn;
        private bool _isRename;

        public CategoryScopeForm(string currentName, bool isRename)
        {
            _isRename = isRename;
            Text = isRename ? "Перейменувати категорію" : "Видалити категорію";
            Size = new Size(350, 250);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            int currentY = 20;

            if (isRename)
            {
                Label lblName = new Label { Text = "Нова назва:", Location = new Point(20, currentY), AutoSize = true };
                txtName = new TextBox { Text = currentName, Location = new Point(100, currentY - 2), Width = 200 };
                Controls.Add(lblName);
                Controls.Add(txtName);
                currentY += 40;
            }

            Label lblScope = new Label { Text = "Застосувати зміни:", Location = new Point(20, currentY), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            Controls.Add(lblScope);
            currentY += 25;

            rbAll = new RadioButton { Text = "До всіх записів (Всі)", Location = new Point(30, currentY), Width = 300, Checked = true };
            currentY += 25;
            rbThisMonth = new RadioButton { Text = "Тільки для цього місяця", Location = new Point(30, currentY), Width = 300 };
            currentY += 25;
            rbFromNowOn = new RadioButton { Text = "З цього місяця і надалі", Location = new Point(30, currentY), Width = 300 };
            currentY += 35;

            Controls.Add(rbAll);
            Controls.Add(rbThisMonth);
            Controls.Add(rbFromNowOn);

            Button btnOk = new Button { Text = isRename ? "Зберегти" : "Видалити", Location = new Point(70, currentY), Width = 90 };
            if (!isRename) btnOk.BackColor = Color.LightCoral;
            btnOk.Click += BtnOk_Click;

            Button btnCancel = new Button { Text = "Скасувати", Location = new Point(170, currentY), Width = 90 };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.Add(btnOk);
            Controls.Add(btnCancel);
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (_isRename)
            {
                CategoryName = txtName.Text.Trim();
                if (string.IsNullOrEmpty(CategoryName))
                {
                    MessageBox.Show("Введіть нову назву!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (rbAll.Checked) Scope = 0;
            else if (rbThisMonth.Checked) Scope = 1;
            else if (rbFromNowOn.Checked) Scope = 2;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
