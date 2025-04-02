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
            if (txtScreenResult.Text == "0")
            {
                txtScreenResult.Text = string.Empty;
            }
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
                string infixExpression;
                //kiểm tra với việc tính toán liên tục
                if (!string.IsNullOrEmpty(txtScreenExpression.Text) && txtScreenExpression.Text.Contains("="))
                {
                    string last = txtScreenExpression.Text.Split('=').Last().Trim();
                    infixExpression = $"{last} {txtScreenResult.Text}";
                }
                else
                {
                    //tính toán bình thường
                    infixExpression = $"{txtScreenExpression.Text}{txtScreenResult.Text}";   //gắn biểu thức cần tính toán của hai màn hình
                }

                string postfixExpression = Calculator.InfixToPostfix(infixExpression);
                MessageBox.Show(postfixExpression);
                double result = Calculator.EvaluatePostfix(postfixExpression);

                txtScreenExpression.Text = $"{infixExpression} = ";
                txtScreenResult.Text = result.ToString();
            }
            catch (DivideByZeroException)
            {
                txtScreenResult.Text = "Cannot divide";
                txtScreenExpression.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi Tính Toán",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
