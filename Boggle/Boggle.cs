﻿using Boggle;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Boggle : Form, IBoggleView
    {
        public event Action<string, string> ConnectionOpenedEvent;
        public event Action<string> WordEvent;
        public event Action ExitEvent;
        public event Action CancelEvent;
        public event Action JoinGameEvent;
        public event Action CancelGameEvent;

        public string Board
        {
            set
            {
                if (value.Length == 16)
                {
                    label1.Text = value[0].ToString();
                    label2.Text = value[1].ToString();
                    label3.Text = value[2].ToString();
                    label4.Text = value[3].ToString();
                    label5.Text = value[4].ToString();
                    label6.Text = value[5].ToString();
                    label7.Text = value[6].ToString();
                    label8.Text = value[7].ToString();
                    label9.Text = value[8].ToString();
                    label10.Text = value[9].ToString();
                    label11.Text = value[10].ToString();
                    label12.Text = value[11].ToString();
                    label13.Text = value[12].ToString();
                    label14.Text = value[13].ToString();
                    label15.Text = value[14].ToString();
                    label16.Text = value[15].ToString();
                }
                else
                {
                    throw new Exception("Invalid Value Length");
                }
            }
        }
        public string Message
        {
            set
            {
                toolStripStatusLabel1.Text = value;
            }
        }
        public string Log
        {
            set
            {
                textBox2.Text = value;
            }
        }
        public string Score
        {
            set
            {
                scoreLabel.Text = "Score" + value;
            }
        }

        public Boggle()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string w = textBox1.Text;
            textBox1.Clear();
            if (WordEvent != null)
            {
                WordEvent( w );
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ExitEvent != null)
            {
                ExitEvent();
            }
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConnectPrompt testDialog = new ConnectPrompt();
            testDialog.Text = "Connect to server:";
            if (testDialog.ShowDialog(this) == DialogResult.OK)
            {
                if (ConnectionOpenedEvent != null)
                {
                    // server, nickname.
                    ConnectionOpenedEvent(testDialog.textBox1.Text, testDialog.textBox2.Text);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (JoinGameEvent != null)
            {
                JoinGameEvent();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (CancelGameEvent != null)
            {
                CancelGameEvent();
            }
        }




        //The following functions I think I accidentally created but I am not ssure if they are needed for anything
        //or not so I just left them here


        private void Boggle_Load(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }
    }
}
