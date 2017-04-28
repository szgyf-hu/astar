using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace astar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void stepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // algoritmus lép
            astar_Control1.AstarStep();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            stepToolStripMenuItem_Click(null, null);
        }

        private void astar_Control1_Click(object sender, EventArgs e)
        {

        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            astar_Control1.AstarInit(0, 0, 99, 99, 100, 100);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
