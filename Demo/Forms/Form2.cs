﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shell;

namespace Demo.Forms
{
    public partial class VouchEntry : Form
    {
        public VouchEntry()
        {
            InitializeComponent();
            this.Focus();
            textBox1.Focus();
            textBox1.SelectAll();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double m = double.Parse(textBox1.Text);
                
                Form1.notifyVoucher(m);
            }catch(Exception ex) {
                Utils.log($"error {ex.Message}");
                Form1.notifyVoucher(-99);
            }
            finally
            {
                this.Dispose();
            }

        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        private void VouchEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1.notifyVoucher(-99);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
