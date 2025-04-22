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
    public partial class MainForm : Form
    {
        // Flag to determine which calculator to use (normal or special)
        private bool useSpecialCalculator = false;
        private bool isRadianMode;
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            isRadianMode = false;  //mặc định tính toán với chế độ DEG
            btnRad.Text = "RAD";
            btnRad.ForeColor = Color.Red;
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
                    infixExpression = $"{txtScreenExpression.Text}{cur}";
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


                    MessageBox.Show($"Trig postfix: {postfix}");


                    double symbolicResult = SpecialCalculator.EvaluatePostfix(postfix, isRadianMode);


                    double numericalResult = SpecialCalculator.EvaluatePostfix(postfix, isRadianMode);


                    txtScreenExpression.Text = $"{originalExpression} = ";
                    txtScreenResult.Text = numericalResult.ToString();


                    MessageBox.Show($"Symbolic form: {symbolicResult}", "Symbolic Result");
                }
                else
                {

                    string postfixExpression = Calculator.InfixToPostfix(infixExpression);
                    MessageBox.Show($"Postfix: {postfixExpression}");

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

        private void btnNegative_Click(object sender, EventArgs e)
        {
            // Nếu màn hình hiện "0" thì không làm gì cả
            if (txtScreenResult.Text == "0")
                return;

            // Đổi dấu của số hiện tại
            if (!txtScreenResult.Text.StartsWith("-"))
            {
                // Nếu số đang là dương, thêm dấu âm
                txtScreenResult.Text = "-" + txtScreenResult.Text;
            }
        }

        private void btnToRadian_Click(object sender, EventArgs e)
        {
            // Toggle between RAD and DEG
            isRadianMode = !isRadianMode;

            // Update UI button to reflect the current mode
            UpdateAngleModeIndicator();

            // Optional: Display a message to inform user
            string mode = isRadianMode ? "RADIAN" : "DEGREE";
            MessageBox.Show($"Chuyển sang chế độ {mode}.", "Chế độ góc", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateAngleModeIndicator()
        {
            btnRad.Text = isRadianMode ? "RAD" : "DEG";
            btnRad.ForeColor = isRadianMode ? Color.Red : Color.Black;
        }


        private void btnPi_Click(object sender, EventArgs e)
        {
            // Thay thế số hiện tại bằng π
            if (txtScreenResult.Text == "0")
            {
                txtScreenResult.Text = Math.PI.ToString();
            }
            else
            {
                txtScreenResult.Text = txtScreenResult.Text + Math.PI.ToString();
            }
        }

        private void btnFraction_Click(object sender, EventArgs e)
        {
            // Thay thế số hiện tại bằng 1/x
            if (txtScreenResult.Text == "0")
            {
                txtScreenResult.Text = "0";
            }
            else
            {
                double currentValue = double.Parse(txtScreenResult.Text);
                if (currentValue != 0)
                {
                    txtScreenResult.Text = (1 / currentValue).ToString();
                }
                else
                {
                    MessageBox.Show("Không tính toán với số 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
