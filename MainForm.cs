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
using customButton;

namespace finalProject
{
    public partial class MainForm : Form
    {
        // Thiết lập cờ cho việc tính toán Calculator hay SpecialCalculator
        private bool useSpecialCalculator = false;
        private bool isRadianMode;
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            isRadianMode = false;  // Mặc định tính toán với chế độ DEG
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

            if (txtScreenExpression.Text == "0")
            {
                txtScreenExpression.Text = string.Empty;  //Xóa số 0 để tính toán với căn bậc 2
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

            // Nếu là hàm lượng giác thì sử dụng SpecialCalculator
            if (operatorText == "sin⁡" || operatorText == "cos" || operatorText == "tan" || operatorText == "cot")
            {
                useSpecialCalculator = true;
            }

            if (!string.IsNullOrEmpty(txtScreenResult.Text) && txtScreenResult.Text != "0")
            {
                // Xử lý với các toán tử và toán hạng được lấy từ 2 màn hình
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

                // Tính toán liên tục với biểu thức sau dấu =
                if (!string.IsNullOrEmpty(txtScreenExpression.Text) && txtScreenExpression.Text.Contains("="))
                {
                    string last = txtScreenExpression.Text.Split('=').Last().Trim();
                    infixExpression = $"{last} {txtScreenResult.Text}";
                }
                // Tính toán bình thường
                else
                {
                    infixExpression = $"{txtScreenExpression.Text}{txtScreenResult.Text}";
                }


                // Chuyển đổi tính toán giữa Calculator và SpecialCalculator
                if (useSpecialCalculator || infixExpression.Contains("sin") || infixExpression.Contains("cos") || infixExpression.Contains("tan") || infixExpression.Contains("cot"))
                {
                    string postfixSpecialCalculator = SpecialCalculator.InfixToPostfix(infixExpression);
                    MessageBox.Show($"Trig postfix: {postfixSpecialCalculator}");
                    double resultSpecialCalculator = SpecialCalculator.EvaluatePostfix(postfixSpecialCalculator, isRadianMode);
                    MessageBox.Show($"Result: {resultSpecialCalculator}");
                    txtScreenExpression.Text = $"{infixExpression} = ";
                    txtScreenResult.Text = resultSpecialCalculator.ToString();
                }
                else
                {
                    string postfixCalculator = Calculator.InfixToPostfix(infixExpression);
                    MessageBox.Show($"Postfix: {postfixCalculator}");
                    double resultCalculator = Calculator.EvaluatePostfix(postfixCalculator);
                    txtScreenExpression.Text = $"{infixExpression} = ";
                    MessageBox.Show($"Result: {resultCalculator}");
                    txtScreenResult.Text = resultCalculator.ToString();
                }

                // Đặt lại chế độ tính toán 
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
                MessageBox.Show($"Error: {ex.Message}", "Calculation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InvalidOperationException ex)
            {
                txtScreenResult.Text = "Invalid expression";
                MessageBox.Show($"Error: {ex.Message}", "Calculation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                txtScreenResult.Text = "Error";
                MessageBox.Show($"Error: {ex.Message}", "Calculation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNegative_Click(object sender, EventArgs e)
        {
            // Nếu màn hình hiện "0" thì không làm gì cả
            if (txtScreenResult.Text == "0")
            { 
                return; 
            }

            // Đổi dấu của số hiện tại
            if (!txtScreenResult.Text.StartsWith("-"))
            {
                // Nếu số đang là dương, thêm dấu âm
                txtScreenResult.Text = "-" + txtScreenResult.Text;
            }
        }

        private void btnToRadian_Click(object sender, EventArgs e)
        {
            // Cập nhật lại nút hiển thị
            UpdateAngleModeIndicator();

            // Chuyển đổi giữa RAD và DEG
            isRadianMode = !isRadianMode;

            // Hiển thị thông báo khi chuyển đổi
            string mode;
            if (isRadianMode)
            {
                mode = "RADIAN";
            }
            else
            {
                mode = "DEGREE";
            }
            MessageBox.Show($"Đã chuyển sang chế độ {mode}.", "Chế độ góc", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateAngleModeIndicator()
        {
            // Kiểm tra isRadianMode
            if (isRadianMode)
            {
                // Nếu đúng thì cập nhật cho chữ và màu cho RAD
                btnRad.Text = "RAD";
                btnRad.ForeColor = Color.Red;
            }
            else
            {
                // Nếu sai thì cập nhật chữ và màu cho DEG
                btnRad.Text = "DEG";
                btnRad.ForeColor = Color.Black;
            }
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
                // Thêm PI vào số sau
                txtScreenResult.Text = txtScreenResult.Text + Math.PI.ToString();
            }
        }

        private void btnFraction_Click(object sender, EventArgs e)
        {
            // Nếu bằng 0 thì không làm gì cả
            if (txtScreenResult.Text == "0")
            {
                txtScreenResult.Text = "0";
            }
            else
            {
                double currentValue = double.Parse(txtScreenResult.Text);
                // Nếu khác 0 thì tính 1/x
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
