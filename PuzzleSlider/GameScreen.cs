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
    public struct blankBlockStruct
    {
        public int value;
        public int x;
        public int y;
    }
    public partial class GameScreen : Form
    {
        private PictureBox frame = new PictureBox(); //picture box frame
        private string imageLocation = "Koala.jpg";
        private Image puzzleImage;
        private int dimensionBlocks = 2; //dimensions we want the puzzle to be
        private int blockWidth;
        private int blockHeight;
        private int blockSpacing = 1; //the number of pixels we want for spacing
        private int numberBlocks;
        private int[] blocks; //keeps track of which block is which
        public blankBlockStruct blankBlock;
        private int startXSpace = 10;//offset starting space
        private int startYSpace = 50;
        private bool numbersMode;
        private Rectangle[] imageSamples;
        private bool winner;
        
        public GameScreen(bool mode, string imgLoc, int difficulty)
        {
            this.dimensionBlocks = difficulty;
            if (imgLoc != "")
            {
                imageLocation = imgLoc;
            }
            numbersMode = mode;
            InitializeComponent();
        }
        public void OpenImage(OpenFileDialog openFileDialog1)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr = System.IO.File.OpenText(openFileDialog1.FileName);
                this.imageLocation = openFileDialog1.FileName;
                this.puzzleImage = Image.FromFile(imageLocation);
                pictureBox1.ImageLocation = this.imageLocation;
                sr.Close();
            }
        }

        private void frame_Paint(object sender, PaintEventArgs e)
        {
            
            Graphics g = e.Graphics; //graphics object that contains the paintEventArgs

            int sampleWidth = this.puzzleImage.Width / this.dimensionBlocks; //we want 3 blocks horizontally, this comes from the actual image
            int sampleHeight = this.puzzleImage.Height / this.dimensionBlocks; //also vertically

            Array.Resize<Rectangle>(ref this.imageSamples, numberBlocks);

            //Portions of Images
            for (int y = 0; y < dimensionBlocks; y++)
            {
                for (int x = 0; x < dimensionBlocks; x++)
                {
                    imageSamples[GetIndex(x, y)] = new Rectangle(x * sampleWidth, y * sampleHeight, sampleWidth, sampleHeight);//x,y,firstParameter + width,secondParameter + height
                }
            }

            //Loop that actually draws to the screen
            for (int y = 0; y < dimensionBlocks; y++)
            {//(x * blockWidth) + (x * blockSpacing)
                for (int x = 0; x < dimensionBlocks; x++)
                {
                    Rectangle destRect = new Rectangle((x * (blockWidth + blockSpacing)) + startXSpace, (y * (blockHeight + blockSpacing)) + startYSpace, blockWidth, blockHeight); // this is the actual destination we want the image to appear
                    if ((blocks[GetIndex(x, y)] != blankBlock.value) || (this.winner)) //as long as we see that the value in the array is not the blank block, we draw the actual image
                    {
                        g.DrawImage(this.puzzleImage,
                            destRect,
                            imageSamples[blocks[GetIndex(x,y)]], GraphicsUnit.Pixel); //image, destination, portion of image we want to draw, type of measurement 
                    }
                    else
                    {
                        g.FillRectangle(new SolidBrush(Color.Black),
                            destRect); //if it is the blank block draw a black rectangle in place of the image
                        g.DrawRectangle(new Pen(Color.Red),
                            destRect);
                    }
                    
                    if(numbersMode)
                    {
                        for (int j = 0; j < dimensionBlocks; j++)
                        {
                            for (int i = 0; i < dimensionBlocks; i++)
                            {
                                String drawString = (blocks[GetIndex(i,j)]).ToString();

                                Font drawFont = new Font("Arial", 32);
                                SolidBrush drawBrush = new SolidBrush(Color.Black);

                                if((drawString == "0") && (!winner))
                                {
                                    drawBrush = new SolidBrush(Color.White);
                                }

                                PointF drawPoint = new PointF(((blockWidth) / 2)+ (blockWidth * i), ((blockHeight)/2)+ (blockHeight * j));

                                e.Graphics.DrawString(drawString, drawFont, drawBrush, drawPoint);
                            }
                        }
                    }
                }
            }
        }
        private int GetIndex(int x, int y)
        {
            return y * this.dimensionBlocks + x; //since it is a 1Dimesnional array, the row component is determined by the 'y' plus the offset which is 'x'
        }
        private int GetXValue(int blankBlockIndex)
        {
            return blankBlockIndex - (this.blankBlock.y * this.dimensionBlocks);
        }
        private int GetYValue(int blankBlockIndex)
        {
            return (blankBlockIndex - blankBlock.x) / this.dimensionBlocks;
        }
        private bool TryChangeBlank(int newValue, int testX, int testY)
        {
            bool returnValue = false;

            if ((testX < this.dimensionBlocks) && (testX > -1)
                && (testY < this.dimensionBlocks) && (testY > -1) &&
                (this.blocks[GetIndex(testX, testY)] == this.blankBlock.value))
            { //if the current block is around a blank block and not out of range of the blocks
                blocks[GetIndex(testX, testY)] = newValue; //place the image in the blank blocks position
                returnValue = true;
            }

            return returnValue;
        }
        private void frame_MouseClick(Object sender, MouseEventArgs e)
        {
            int indexX = (e.X - startXSpace) / (this.blockWidth + this.blockSpacing); //determines which block we click -> x pos of mouse / (width of block + the spacing in between)
            int indexY = (e.Y - startYSpace)/ (this.blockHeight + this.blockSpacing);
            
            int clickBlockValue = blocks[GetIndex(indexX, indexY)];//this is the actual block you click
            bool blankFound = false;
            
            blankFound = (TryChangeBlank(clickBlockValue, indexX + 1, indexY) ||
            TryChangeBlank(clickBlockValue, indexX - 1, indexY) ||
            TryChangeBlank(clickBlockValue, indexX, indexY + 1) ||
            TryChangeBlank(clickBlockValue, indexX, indexY - 1)); 

            if (blankFound)//if there was a swap that happened then assign the blank block to the images old position before it switched
            {
                blocks[GetIndex(indexX, indexY)] = this.blankBlock.value;
                this.blankBlock.x = indexX;
                this.blankBlock.y = indexY;
            }

            frame.Update();
            this.winner = true;
            for (int index = 0; index < numberBlocks; index++)
            {//this loop goes through each block to see if the user has won
                if(blocks[index] != index)
                {//if it is not in order then the player is not a winner
                    this.winner = false;
                    break;
                }
            }


            frame.Invalidate();//draws everything to the screen
            if (this.winner)
            {
                MessageBox.Show("You did it! ", "Congratulations!!!");
                winner = false;
            }
        }
        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            if (!numbersMode)
            {
                label1.Visible = true;
                pictureBox1.Visible = true;
                pictureBox2.Visible = true;
                Graphics g = e.Graphics; //graphics object that contains the paintEventArgs

                float frameToPreviewRatioX = pictureBox1.Width / dimensionBlocks;
                float frameToPreviewRatioY = pictureBox1.Height / dimensionBlocks;

                Image previewImage = this.puzzleImage;//***
                pictureBox1.Image = previewImage;//preview image

                pictureBox2.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y);
                pictureBox2.Size = new Size((int)(frameToPreviewRatioX), (int)(frameToPreviewRatioY));

                int previewBlockWidth = (int)(frameToPreviewRatioX) / (dimensionBlocks + blockSpacing);
                int previewBlockHeight = (int)(frameToPreviewRatioY) / (dimensionBlocks + blockSpacing);
            }
        }
        private void TryChangeBlankForRandom(int xDirection, int yDirection)
        {
            if (blocks[GetIndex(this.blankBlock.x + xDirection, this.blankBlock.y + yDirection)] != blankBlock.value)
            {
                int temp = blocks[GetIndex(this.blankBlock.x + xDirection, this.blankBlock.y + yDirection)];
                blocks[GetIndex(this.blankBlock.x + xDirection, this.blankBlock.y + yDirection)] = this.blankBlock.value;
                blocks[GetIndex(this.blankBlock.x, this.blankBlock.y)] = temp;
                blankBlock.y += yDirection;
                blankBlock.x += xDirection;
            }
        }
        private void randomize()
        {
            bool inOrder = true;
            while (inOrder)
            {
                Random rand = new Random();
                int direction;
                int loopThisManyTimes = rand.Next((25 * this.numberBlocks), (50 * this.numberBlocks));
                while (loopThisManyTimes < (25 * this.numberBlocks))
                {
                    loopThisManyTimes = rand.Next((25 * this.numberBlocks), (50 * this.numberBlocks));
                }

                for (int loop = 0; loop < loopThisManyTimes; loop++)
                {
                    direction = rand.Next(1, 5);

                    if (direction == 1)//Swap with top block
                    {
                        if (blankBlock.y > 0)
                        {
                            TryChangeBlankForRandom(0, -1);
                        }
                    }
                    else if (direction == 2)//Swap with bottom block
                    {
                        if (blankBlock.y < (this.dimensionBlocks - 1))
                        {
                            TryChangeBlankForRandom(0, 1);
                        }
                    }
                    else if (direction == 3)//Swap with left block
                    {
                        if (blankBlock.x > 0)
                        {
                            TryChangeBlankForRandom(-1, 0);
                        }
                    }
                    else if (direction == 4)//Swap with top block
                    {
                        if (blankBlock.x < (this.dimensionBlocks - 1))
                        {
                            TryChangeBlankForRandom(1, 0);
                        }
                    }
                }

                inOrder = true;
                for (int i = 0; i < this.numberBlocks; i++)
                {
                    if (blocks[i] != i)
                    {
                        inOrder = false;
                    }
                }
            }
        }
        private void GameScreen_Load(object sender, EventArgs e) //When the form loads
        {
            label1.Visible = false;
            pictureBox2.Visible = false;
            pictureBox1.Visible = false;
            this.BackColor = System.Drawing.Color.Goldenrod;
            mainMenu.BackColor = System.Drawing.Color.Aqua;
            button2.BackColor = System.Drawing.Color.LawnGreen;
            button3.BackColor = System.Drawing.Color.MediumOrchid;

            if (numbersMode)
            {
                imageLocation = "numbersBackground.png";
            }
            puzzleImage = Image.FromFile(imageLocation);

            frame.Paint += new PaintEventHandler(this.frame_Paint); //Add a new paint event handler to the picture box (gonna refer to this as pb)
            frame.MouseClick += new MouseEventHandler(this.frame_MouseClick); //Add a new mouse event handler to the picture box
            frame.Size = new Size(640,640); //Adjust the picture box size

            this.blockHeight = this.frame.Height / dimensionBlocks - blockSpacing; //defines the block height -> (pb height/number of blocks we want) - the white spacing
            this.blockWidth = this.frame.Width / dimensionBlocks - blockSpacing;
            this.numberBlocks = dimensionBlocks * dimensionBlocks;
            this.blocks = new int[numberBlocks]; //makes an array where want to store the number of the block (when you have them in order, you win the game)

            for (int blockIndex = 0; blockIndex < blocks.Length; blockIndex++)
            {
                blocks[blockIndex] = blockIndex; //here we are just assigning the numbers of each block (upper left one is biggest, then bottom right is smallest)
            }
            this.Controls.Add(frame);//this links the newly created picture box with the GameScreen form

            this.Paint += new PaintEventHandler(this.GameScreen_Paint);

            blankBlock.value = 0;
            blankBlock.x = 0;
            blankBlock.y = 0;
            randomize();            
        }

        private void saveCurrentGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                int mode;
                if(numbersMode)
                {
                    mode = 1;
                }
                else
                {
                    mode = 0;
                }
                //to prevent a user to select an invalid file
                string saveData = mode + "\n" + imageLocation + "\nThis is valid game file 11101001100010110111010111110001110\n" 
                    +  this.dimensionBlocks + "," + this.imageLocation + "\n";
                for(int num = 0; num < numberBlocks; num++)
                {
                    saveData += blocks[num].ToString() + ",";
                }
                System.IO.StreamWriter sw = System.IO.File.CreateText(saveFileDialog1.FileName);
                sw.Write(saveData);
                sw.Close();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        private void loadGameToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            pictureBox2.Visible = false;
            pictureBox1.Visible = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog1.FileName != "")
                {
                    bool valid = true;
                    System.IO.StreamReader sr = System.IO.File.OpenText(openFileDialog1.FileName);

                    string s = sr.ReadLine();

                    if (s == "0")
                    {
                        numbersMode = false;
                    }
                    else if (s == "1")
                    {
                        numbersMode = true;
                    }
                    else
                    {
                        valid = false;
                    }
                    if (valid)
                    {
                        s = sr.ReadLine();
                        this.puzzleImage = Image.FromFile(s);
                        s = sr.ReadLine();

                        if (s == "This is valid game file 11101001100010110111010111110001110")
                        {
                            string dimensionsToChange = "";
                            s = sr.ReadLine();
                            string imageToChange = "";
                            int count = 0;//to determine which part is the dimensionBlocks or imageSourceFilePath
                            for (int i = 0; i < s.Length; i++)
                            {
                                if (s[i] != ',')
                                {
                                    if (count == 0)
                                    {
                                        dimensionsToChange += s[i];
                                    }
                                    else if (count == 1)
                                    {
                                        imageToChange += s[i];
                                    }
                                }
                                else
                                {
                                    count++;
                                }
                            }
                            if (dimensionsToChange != "")
                            {
                                this.dimensionBlocks = int.Parse(dimensionsToChange);
                                this.blockHeight = this.frame.Height / dimensionBlocks - blockSpacing; //defines the block height -> (pb height/number of blocks we want) - the white spacing
                                this.blockWidth = this.frame.Width / dimensionBlocks - blockSpacing;
                                this.numberBlocks = dimensionBlocks * dimensionBlocks;
                                Array.Resize<int>(ref this.blocks, numberBlocks);
                            }
                            if ((imageToChange != "") || (imageToChange == imageLocation))
                            {
                                imageLocation = imageToChange;
                                this.pictureBox1.ImageLocation = imageLocation;
                                this.puzzleImage = Image.FromFile(imageLocation);
                            }

                            s = sr.ReadLine();
                            count = 0;
                            string numberToAdd = "";
                            for (int i = 0; i < s.Length; i++)
                            {
                                if (s[i] != ',')
                                {
                                    //this.blocks[count] = int.Parse(s[i].ToString());
                                    numberToAdd += s[i].ToString();
                                }
                                else
                                {
                                    this.blocks[count] = int.Parse(numberToAdd);
                                    count++;
                                    numberToAdd = "";
                                }
                            }
                        }
                        else
                        {
                            valid = false;
                        }
                    }
                    if (!valid)
                    {
                        MessageBox.Show("This is an invalid file!");
                    }
                    sr.Close();
                    frame.Invalidate();
                }
            }
        }
        
        private void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
        private void mainMenu_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
            this.DialogResult = DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            pictureBox2.Visible = false;
            pictureBox1.Visible = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog1.FileName != "")
                {
                    bool valid = true;
                    System.IO.StreamReader sr = System.IO.File.OpenText(openFileDialog1.FileName);

                    string s = sr.ReadLine();

                    if (s == "0")
                    {
                        numbersMode = false;
                    }
                    else if (s == "1")
                    {
                        numbersMode = true;
                    }
                    else
                    {
                        valid = false;
                    }

                    if (valid)
                    {
                        s = sr.ReadLine();
                        this.puzzleImage = Image.FromFile(s);
                        s = sr.ReadLine();

                        if (s == "This is valid game file 11101001100010110111010111110001110")
                        {
                            string dimensionsToChange = "";
                            s = sr.ReadLine();
                            string imageToChange = "";
                            int count = 0;//to determine which part is the dimensionBlocks or imageSourceFilePath
                            for (int i = 0; i < s.Length; i++)
                            {
                                if (s[i] != ',')
                                {
                                    if (count == 0)
                                    {
                                        dimensionsToChange += s[i];
                                    }
                                    else if (count == 1)
                                    {
                                        imageToChange += s[i];
                                    }
                                }
                                else
                                {
                                    count++;
                                }
                            }
                            if (dimensionsToChange != "")
                            {
                                this.dimensionBlocks = int.Parse(dimensionsToChange);
                                this.blockHeight = this.frame.Height / dimensionBlocks - blockSpacing; //defines the block height -> (pb height/number of blocks we want) - the white spacing
                                this.blockWidth = this.frame.Width / dimensionBlocks - blockSpacing;
                                this.numberBlocks = dimensionBlocks * dimensionBlocks;
                                Array.Resize<int>(ref this.blocks, numberBlocks);
                            }
                            if ((imageToChange != "") || (imageToChange == imageLocation))
                            {
                                imageLocation = imageToChange;
                                this.pictureBox1.ImageLocation = imageLocation;
                                this.puzzleImage = Image.FromFile(imageLocation);
                            }

                            s = sr.ReadLine();
                            count = 0;
                            string numberToAdd = "";
                            for (int i = 0; i < s.Length; i++)
                            {
                                if (s[i] != ',')
                                {
                                    //this.blocks[count] = int.Parse(s[i].ToString());
                                    numberToAdd += s[i].ToString();
                                }
                                else
                                {
                                    this.blocks[count] = int.Parse(numberToAdd);
                                    count++;
                                    numberToAdd = "";
                                }
                            }
                        }
                        else
                        {
                            valid = false;
                        }
                    }
                    if(!valid)
                    {
                        MessageBox.Show("This is an invalid file!");
                    }
                    sr.Close();
                    frame.Invalidate();
                }
            }
            
        }

    }
}
