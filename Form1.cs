using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace finalProject
{
    public partial class Form1 : Form
    {
        bool enterValue = false;
        private string lastResult = "";
        private string lastOperator = "";
        private string lastOperand = "";
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pnHistory_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnC_Click(object sender, EventArgs e)
        {
            txtScreenResult.Text = "0";
            txtScreenExpression.Text = string.Empty;
        }

        private void btnCE_Click(object sender, EventArgs e)
        {
            txtScreenResult.Text = "0";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtScreenResult_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnNumber_Click(object sender, EventArgs e)
        {
            if (txtScreenResult.Text == "0" || enterValue)
            {
                txtScreenResult.Text = string.Empty;
            }
            enterValue = false;
            customButton.Design btn = (customButton.Design)sender;
            if (btn.Text == ".")
            {
                if (!txtScreenResult.Text.Contains("."))
                {
                    txtScreenResult.Text = txtScreenResult.Text + btn.Text;
                }
            }
            else
            {
                txtScreenResult.Text = txtScreenResult.Text + btn.Text;
            }
        }

        private void btnOperator_Click(object sender, EventArgs e)
        {
            customButton.Design btn = (customButton.Design)sender;
            if (!string.IsNullOrEmpty(txtScreenResult.Text) && txtScreenResult.Text != "0")
            {
                if (txtScreenExpression.Text == "0")
                {
                    txtScreenExpression.Text = txtScreenResult.Text + " " + btn.Text + " ";
                }
                else
                {
                    txtScreenExpression.Text += txtScreenResult.Text + " " + btn.Text + " ";
                }
                enterValue = true;
                txtScreenResult.Text = "0";
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtScreenResult.Text.Length > 0)
            {
                txtScreenResult.Text = txtScreenResult.Text.Remove(txtScreenResult.Text.Length - 1, 1);
            }
            if (txtScreenResult.Text == string.Empty)
            {
                txtScreenResult.Text = "0";
            }
        }

        private void btnEqual_Click(object sender, EventArgs e)
        {
            try
            {
                // Nếu không có biểu thức và chưa nhập số, không làm gì
                if (string.IsNullOrEmpty(txtScreenExpression.Text) && txtScreenResult.Text == "0")
                    return;

                string infixExpression;

                // Nếu đang nhập biểu thức (có toán tử cuối)
                if (!string.IsNullOrEmpty(txtScreenExpression.Text))
                {
                    infixExpression = $"{txtScreenExpression.Text}{txtScreenResult.Text}";
                }
                // Nếu chỉ nhập số rồi bấm = (tính lại kết quả trước đó)
                else if (!string.IsNullOrEmpty(lastResult))
                {
                    infixExpression = $"{txtScreenResult.Text} {lastOperator}{lastOperand}";
                }
                else
                {
                    infixExpression = txtScreenResult.Text;
                }

                string postfixExpression = Calculator.InfixToPostfix(infixExpression);
                MessageBox.Show(postfixExpression);
                double result = Calculator.EvaluatePostfix(postfixExpression);

                // Lưu lại toán tử và toán hạng cho tính toán tiếp theo
                if (!string.IsNullOrEmpty(txtScreenExpression.Text))
                {
                    lastOperator = txtScreenExpression.Text.Trim().Split(' ').Last();
                    lastOperand = txtScreenResult.Text;
                }

                lastResult = result.ToString();
                txtScreenExpression.Text = $"{infixExpression} =";
                txtScreenResult.Text = FormatResult(result);
                enterValue = true;
            }
            catch (DivideByZeroException)
            {
                txtScreenResult.Text = "Cannot divide";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi Tính Toán",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnC_Click(sender, e);
            }
        }
        private string FormatResult(double result)
        {
            // Hiển thị số nguyên không có phần thập phân nếu là số nguyên
            return result % 1 == 0 ? result.ToString("0") : result.ToString("0.##########");
        }
    }
}
