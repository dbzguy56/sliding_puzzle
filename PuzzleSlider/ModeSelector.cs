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
    public partial class ModeSelector : Form
    {
        public ModeSelector()
        {
            InitializeComponent();
        }

        private void loadGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameScreen gameWindow = new GameScreen(false, "", 3);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DifficultySelector difficultyWindow = new DifficultySelector(true, "");
            ModeSelector modeWindow = (ModeSelector)ActiveForm;
            modeWindow.Hide();
            difficultyWindow.ShowDialog();
            modeWindow.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files (*.bmp, *.jpg, *.gif, *.png, *.tiff)|*.bmp;*.jpg;*.gif;*.png;*.tiff";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                DifficultySelector difficultyWindow = new DifficultySelector(false, openFileDialog1.FileName);
                ModeSelector modeWindow = (ModeSelector)ActiveForm;
                modeWindow.Hide();
                difficultyWindow.ShowDialog();
                modeWindow.Show();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            HideImages(false);
        }
        public void fileLabelUpdateInfo(OpenFileDialog openFileDialog1)
        {
            Text = openFileDialog1.FileName;
        }
        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        private void HideImages(bool Hide)
        {
            pictureBox1.Visible = !Hide;
            pictureBox2.Visible = !Hide;
            pictureBox3.Visible = !Hide;
            pictureBox4.Visible = !Hide;
            pictureBox5.Visible = !Hide;
            pictureBox6.Visible = !Hide;
            pictureBox7.Visible = !Hide;
            pictureBox8.Visible = !Hide;
            pictureBox9.Visible = !Hide;
            button1.Visible = Hide;
            button2.Visible = Hide;
            button3.Visible = Hide;
        }

        private void ModeSelector_Load(object sender, EventArgs e)
        {
            HideImages(true);

            this.BackColor = System.Drawing.Color.Goldenrod;
            button1.BackColor = System.Drawing.Color.Aqua;
            button2.BackColor = System.Drawing.Color.LawnGreen;
            button3.BackColor = System.Drawing.Color.MediumOrchid;

            
            button1.Font = new Font("Times New Roman", 24, FontStyle.Bold);
            button2.Font = new Font("Times New Roman", 24, FontStyle.Bold);
            button3.Font = new Font("Times New Roman", 24, FontStyle.Bold);         
        }
 

        private void SetPresetPicture(string pic)
        {
            DifficultySelector difficultyWindow = new DifficultySelector(false, pic);
            ModeSelector modeWindow = (ModeSelector)ActiveForm;
            modeWindow.Hide();
            difficultyWindow.ShowDialog();
            modeWindow.Show();
            HideImages(true);
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            SetPresetPicture("stock1.jpg");
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            SetPresetPicture("stock2.jpg");
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            SetPresetPicture("stock4.png");
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            SetPresetPicture("stock3.jpg");
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            SetPresetPicture("stock5.jpg");
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            SetPresetPicture("stock6.jpg");
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            SetPresetPicture("stock7.jpg");
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            SetPresetPicture("stock8.jpg");
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            SetPresetPicture("stock9.jpg");
        }

    }
}
