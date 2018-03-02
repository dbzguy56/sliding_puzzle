using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleSlider
{
    public partial class DifficultySelector : Form
    {
        private bool mode;
        private string imgLoc;
        public DifficultySelector(bool m, string imageLocation)
        {
            mode = m;
            imgLoc = imageLocation;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GameScreen newMDIChild = new GameScreen(mode, imgLoc, 2);
            this.Hide();
            newMDIChild.ShowDialog();
        }

        private void DifficultySelector_Load(object sender, EventArgs e)
        {
            this.BackColor = System.Drawing.Color.Goldenrod;
            button1.BackColor = System.Drawing.Color.Aqua;
            button2.BackColor = System.Drawing.Color.LawnGreen;
            button3.BackColor = System.Drawing.Color.MediumOrchid;

            button1.Font = new Font("Times New Roman", 24, FontStyle.Bold);
            button2.Font = new Font("Times New Roman", 24, FontStyle.Bold);
            button3.Font = new Font("Times New Roman", 24, FontStyle.Bold);

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
            return;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            GameScreen newMDIChild = new GameScreen(mode, imgLoc, 3);
            this.Hide();
            newMDIChild.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GameScreen newMDIChild = new GameScreen(mode, imgLoc, 4);
            this.Hide();
            newMDIChild.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1.Text != "")
            {
                GameScreen newMDIChild = new GameScreen(mode, imgLoc, int.Parse(maskedTextBox1.Text));
                this.Hide();
                newMDIChild.ShowDialog();
            }
        }
    }
}
