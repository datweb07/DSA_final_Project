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
    public partial class Form1: Form
    {
        bool enterValue = false;
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

        private void btnPercent_Click(object sender, EventArgs e)
        {

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
            if (!string.IsNullOrEmpty(txtScreenResult.Text) &&txtScreenResult.Text != "0")
            {
                txtScreenExpression.Text += txtScreenResult.Text + " " + btn.Text + " ";
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
    }
}
