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
        public Form1()
        {
            InitializeComponent();

            //btnExit.MouseMove += BtnExit_MouseEnter;
            //btnExit.MouseLeave += BtnExit_MouseLeave;
        }

        //private void BtnExit_MouseEnter(object sender, EventArgs e)
        //{
        //    btnExit.BackColor = Color.Blue;
        //}
        //private void BtnExit_MouseLeave(object sender, EventArgs e)
        //{
        //    btnExit.BackColor = SystemColors.Window;
        //}
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void pnHistory_Paint(object sender, PaintEventArgs e)
        {

        }

        private void design2_Click(object sender, EventArgs e)
        {

        }

        private void design10_Click(object sender, EventArgs e)
        {

        }

        private void design3_Click(object sender, EventArgs e)
        {

        }

        private void design1_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (btnExit !=  null) 
                {
                btnExit.BackColor = Color.Green;

            }
           
            this.Close();   
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
