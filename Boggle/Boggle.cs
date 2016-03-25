using Boggle;
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
        public string Board { set; }
        public string Message { set; }
        public string Log { set; }
        public string Score { set; }
        public Boggle()
        {
            InitializeComponent();
        }

        private void Boggle_Load(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
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
        private void label1_Click(object sender, EventArgs e)
        {
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

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
