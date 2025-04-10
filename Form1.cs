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
        // Flag to determine which calculator to use (normal or special)
        private bool useSpecialCalculator = false;

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
            customButton.Design btn = (customButton.Design)sender;
            if (txtScreenResult.Text == "0")
            {
                txtScreenResult.Text = string.Empty;
            }
            else  //tính bình phương
            {
                txtScreenResult.Text = txtScreenResult.Text + "";
            }

            //bỏ đi số 0 ở ScreenExpression khi thực hiện tính căn bậc hai của biểu thức khi mới khởi tạo chương trình
            if (txtScreenExpression.Text == "0")
            {
                txtScreenExpression.Text = string.Empty;
            }

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
            string operatorText = btn.Text;

            // If trigonometric function is selected, switch to Special calculator
            if (operatorText == "sin" || operatorText == "cos" || operatorText == "tan" || operatorText == "cot")
            {
                useSpecialCalculator = true;


                if (txtScreenExpression.Text == "0" || txtScreenExpression.Text == string.Empty)
                {
                    txtScreenExpression.Text = $"{operatorText}(";
                }
                else
                {
                    txtScreenExpression.Text += $"{operatorText}(";
                }
                txtScreenResult.Text = "0";
            }
            else if (operatorText == ")")
            {

                if (!string.IsNullOrEmpty(txtScreenResult.Text) && txtScreenResult.Text != "0")
                {
                    txtScreenExpression.Text += txtScreenResult.Text + operatorText;
                    txtScreenResult.Text = "0";
                }
                else
                {
                    txtScreenExpression.Text += operatorText;
                }
            }
            else if (!string.IsNullOrEmpty(txtScreenResult.Text) && txtScreenResult.Text != "0")
            {
                // Handle normal operators
                if (txtScreenExpression.Text == "0" || txtScreenExpression.Text == string.Empty)
                {
                    txtScreenExpression.Text = txtScreenResult.Text + " " + btn.Text + " ";
                }
                else
                {
                    txtScreenExpression.Text += txtScreenResult.Text + " " + btn.Text + " ";
                }
                txtScreenResult.Text = "0";
            }
            else if (operatorText == "+" || operatorText == "-" || operatorText == "×" || operatorText == "÷" || operatorText == "^")
            {
                // Allow operators at the end of expression even if no number is entered
                if (txtScreenExpression.Text.EndsWith(" "))
                {
                    // Replace last operator
                    txtScreenExpression.Text = txtScreenExpression.Text.TrimEnd(' ').TrimEnd('+', '-', '×', '÷', '^');
                    txtScreenExpression.Text += " " + operatorText + " ";
                }
                else if (!string.IsNullOrEmpty(txtScreenExpression.Text) && !txtScreenExpression.Text.EndsWith("("))
                {
                    txtScreenExpression.Text += " " + operatorText + " ";
                }
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

                // tính toán liên tục
                if (!string.IsNullOrEmpty(txtScreenExpression.Text) && txtScreenExpression.Text.Contains("="))
                {
                    string last = txtScreenExpression.Text.Split('=').Last().Trim();
                    infixExpression = $"{last} {txtScreenResult.Text}";
                }
                else
                {
                    // tính toán bình thường
                    infixExpression = $"{txtScreenExpression.Text}{txtScreenResult.Text}";
                }

                // tính toán với số âm (cần chỉnh lại)
                string cur = txtScreenResult.Text;
                if (cur.StartsWith("-"))
                {
                    infixExpression = $"{txtScreenExpression.Text}{txtScreenResult.Text}";
                }

                // Store original expression for display
                string originalExpression = infixExpression;

                // Choose which calculator to use based on the presence of trig functions
                if (useSpecialCalculator ||
                    infixExpression.Contains("sin") ||
                    infixExpression.Contains("cos") ||
                    infixExpression.Contains("tan") ||
                    infixExpression.Contains("cot"))
                {

                    string postfix = SpecialCalculator.InfixToPostfix(infixExpression);


                    //MessageBox.Show($"Trig postfix: {postfix}");


                    double symbolicResult = SpecialCalculator.EvaluatePostfix(postfix);


                    double numericalResult = SpecialCalculator.EvaluatePostfix(postfix);


                    txtScreenExpression.Text = $"{originalExpression} = ";
                    txtScreenResult.Text = numericalResult.ToString();


                    //MessageBox.Show($"Symbolic form: {symbolicResult}", "Symbolic Result");
                }
                else
                {

                    string postfixExpression = Calculator.InfixToPostfix(infixExpression);


                    //MessageBox.Show($"Regular postfix: {postfixExpression}");

                    double result = Calculator.EvaluatePostfix(postfixExpression);

                    txtScreenExpression.Text = $"{originalExpression} = ";
                    txtScreenResult.Text = result.ToString();
                }

                // Reset the special calculator flag for next calculation
                useSpecialCalculator = false;
            }
            catch (DivideByZeroException)
            {
                txtScreenResult.Text = "Cannot divide by zero";
                txtScreenExpression.Text = string.Empty;
            }
            catch (ArgumentException ex)
            {
                txtScreenResult.Text = "Error";
                MessageBox.Show($"Error: {ex.Message}", "Calculation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InvalidOperationException ex)
            {
                txtScreenResult.Text = "Invalid expression";
                MessageBox.Show($"Error: {ex.Message}", "Calculation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                txtScreenResult.Text = "Error";
                MessageBox.Show($"Error: {ex.Message}", "Calculation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //bổ sung thêm phương thức số âm
        private void btnNegative_Click(object sender, EventArgs e)
        {
            //// Nếu màn hình hiện "0" thì không làm gì
            //if (txtScreenResult.Text == "0")
            //    return;

            //// Nếu số đang là dương, thêm dấu âm
            //if (!txtScreenResult.Text.StartsWith("-"))
            //{
            //    double res = double.Parse(txtScreenResult.Text) * -1;
            //    txtScreenResult.Text = res.ToString();
            //}
            ////else
            ////{
            ////    // Nếu số đang là âm, đổi thành dương
            ////    double res = double.Parse(txtScreenResult.Text) * -1;
            ////    txtScreenResult.Text = res.ToString();
            ////}
            ///
            // Nếu màn hình hiện "0" thì không làm gì
            if (txtScreenResult.Text == "0")
                return;

            // Đổi dấu của số hiện tại
            if (txtScreenResult.Text.StartsWith("-"))
            {
                // Nếu số đang là âm, đổi thành dương (bỏ dấu trừ ở đầu)
                txtScreenResult.Text = txtScreenResult.Text.Substring(1);
            }
            else
            {
                // Nếu số đang là dương, thêm dấu âm
                txtScreenResult.Text = "-" + txtScreenResult.Text;
            }
        }

        // Add method to switch between normal and trigonometric calculator modes
        private void btnSwitchMode_Click(object sender, EventArgs e)
        {
            useSpecialCalculator = !useSpecialCalculator;
            string mode = useSpecialCalculator ? "Trigonometric" : "Standard";
            MessageBox.Show($"Switched to {mode} Calculator Mode", "Mode Change");
        }
    }
}
