using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Chap6
{
    public partial class MainForm : Form
    {
        DrawForm dr;
        Form1 f;
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dr != null)
            { dr = null; }
            dr = new DrawForm();
            dr.Show();
            dr.Left = this.Left + this.Width + 1;
            dr.Top = this.Top;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dr == null)
            {
                button1_Click(sender, e);
            }
            dr.Draw1();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dr == null)
            {
                button1_Click(sender, e);
            }
            dr.Draw2();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (dr == null)
            { button1_Click(sender, e); }
            dr.SetDblBuff();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dr == null)
            { button1_Click(sender, e); }
            dr.Draw3();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dr == null)
            { button1_Click(sender, e); }
            dr.Draw4();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (f == null)
            {
                f = new Form1();
            }

            f.Show();
        }

    }
}