using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// A recreation of the space race arcade game
//Mitchell Moore
//Jan 11 2023
namespace Space_Race
{
    public partial class Form1 : Form
    {
        Rectangle player1 = new Rectangle(200, 465, 10, 20);
        Rectangle player2 = new Rectangle(470, 465, 10, 20);

        Rectangle end = new Rectangle(0, 90, 600, 5);

        Rectangle test = new Rectangle(200, 200, 10, 1);
        List<Rectangle> asteroidsLeft = new List<Rectangle>();
        List<Rectangle> asteroidsRight = new List<Rectangle>();

        int randValue;

        int player1Score = 0;
        int player2Score = 0;

        int playerSpeed = 4;
        List<int> asteroidsSpeedLeft = new List<int>();
        List<int> asteroidsSpeedRight = new List<int>();

        bool sDown = false;
        bool wDown = false;
        bool downDown = false;
        bool upDown = false;
        bool spaceDown = false;
        bool rDown = false;


        SolidBrush whiteBrush = new SolidBrush(Color.White);

        Random randGen = new Random();

        string gameState = "start";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.Down:
                    downDown = true;
                    break;
                case Keys.Up:
                    upDown = true;
                    break;
                case Keys.Space:
                    spaceDown = true;
                    break;
                case Keys.R:
                    rDown = true;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.Down:
                    downDown = false;
                    break;
                case Keys.Up:
                    upDown = false;
                    break;
                case Keys.Space:
                    spaceDown = false;
                    break;
                case Keys.R:
                    rDown = false;
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // start game if space is pressed on menu page
            if(gameState == "start")
            {
                if (spaceDown == true)
                {
                    gameState = "running";
                }
            }

            if (gameState == "running")
            {
                //move player1
                if (wDown == true)
                {
                    player1.Y -= playerSpeed;
                }

                if (sDown == true && player1.Y < this.Height - 25)
                {
                    player1.Y += playerSpeed;
                }

                //move player2
                if (upDown == true)
                {
                    player2.Y -= playerSpeed;
                }

                if (downDown == true && player2.Y < this.Height - 25)
                {
                    player2.Y += playerSpeed;
                }

                //make a random value and add an asteroid if chance says so

                randValue = randGen.Next(1, 101);

                if (randValue <= 30)
                {

                    asteroidsLeft.Add(new Rectangle(0, randGen.Next(90, 464), 10, 1));
                    asteroidsSpeedLeft.Add(randGen.Next(3, 10));

                }

                randValue = randGen.Next(1, 101);

                if (randValue <= 30)
                {

                    asteroidsRight.Add(new Rectangle(this.Width, randGen.Next(90, 464), 10, 1));
                    asteroidsSpeedRight.Add(randGen.Next(3, 10));
                }

                //move asteroids
                for (int i = 0; i < asteroidsLeft.Count; i++)
                {
                    int x = asteroidsLeft[i].X + asteroidsSpeedLeft[i];
                    asteroidsLeft[i] = new Rectangle(x, asteroidsLeft[i].Y, asteroidsLeft[i].Width, asteroidsLeft[i].Height);
                }

                for (int i = 0; i < asteroidsRight.Count; i++)
                {
                    int x = asteroidsRight[i].X - asteroidsSpeedRight[i];
                    asteroidsRight[i] = new Rectangle(x, asteroidsRight[i].Y, asteroidsRight[i].Width, asteroidsRight[i].Height);
                }
                //remove asteroids if they go off screen
                for (int i = 0; i < asteroidsLeft.Count; i++)
                {
                    if (asteroidsLeft[i].X >= this.Width)
                    {
                        asteroidsLeft.RemoveAt(i);
                        asteroidsSpeedLeft.RemoveAt(i);
                    }
                }

                for (int i = 0; i < asteroidsRight.Count; i++)
                {
                    if (asteroidsRight[i].X >= this.Width)
                    {
                        asteroidsRight.RemoveAt(i);
                        asteroidsSpeedRight.RemoveAt(i);
                    }
                }
                //check if player has intersected with an asteroid and reset them
                for (int i = 0; i < asteroidsLeft.Count; i++)
                {
                    if (player1.IntersectsWith(asteroidsLeft[i]))
                    {
                        player1.Y = 465;

                    }
                }

                for (int i = 0; i < asteroidsLeft.Count; i++)
                {
                    if (player2.IntersectsWith(asteroidsLeft[i]))
                    {
                        player2.Y = 465;

                    }
                }

                for (int i = 0; i < asteroidsRight.Count; i++)
                {
                    if (player1.IntersectsWith(asteroidsRight[i]))
                    {
                        player1.Y = 465;

                    }
                }

                for (int i = 0; i < asteroidsRight.Count; i++)
                {
                    if (player2.IntersectsWith(asteroidsRight[i]))
                    {
                        player2.Y = 465;

                    }
                }
                //check if a player has reached the end (send back to start and give them a point if they have)
                if (player1.IntersectsWith(end))
                {
                    player1Score++;
                    player1.Y = 465;
                }

                if (player2.IntersectsWith(end))
                {
                    player2Score++;
                    player2.Y = 465;
                }



                //end game if a player reaches 3 points; if so endgame
                if (player1Score == 3)
                {
                    gameState = "done";
                }
                if (player2Score == 3)
                {
                    gameState = "done";
                }

            }
            //if r is pressed after game is done, game restarts
            if(gameState == "done")
            {
                if(rDown == true)
                {
                    player1Score = 0;
                    player2Score = 0;
                    gameState = "running";
                }
            }

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //show menu/start screen
            if(gameState == "start")
            {
                titleLabel.Visible = true;
                startLabel.Text = "Press Space To Begin";
                startLabel.Visible = true;
                winLabel.Visible = false;
            }
            // show game screen (paint players, asteroids, etc.)
            if (gameState == "running")
            {
                titleLabel.Visible = false;
                startLabel.Visible = false;
                winLabel.Visible = false;

                e.Graphics.FillRectangle(whiteBrush, this.Width / 2 - 2, this.Height / 4, 5, this.Height);

                e.Graphics.FillRectangle(whiteBrush, player1);

                e.Graphics.FillRectangle(whiteBrush, player2);

                for (int i = 0; i < asteroidsLeft.Count; i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, asteroidsLeft[i]);
                }
                for (int i = 0; i < asteroidsRight.Count; i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, asteroidsRight[i]);
                }


                player1ScoreLabel.Text = $"{player1Score}";
                player2ScoreLabel.Text = $"{player2Score}";
            }
            //show end screen
            if (gameState == "done")
            {
                titleLabel.Visible = false;
                startLabel.Text = "Press R To Restart";
                startLabel.Visible = true;
                if(player1Score == 3)
                {
                    winLabel.Text = "Player 1 WIns";
                }
                else if (player2Score == 3)
                {
                    winLabel.Text = "Player 2 WIns";
                }
                winLabel.Visible = true;
            }
        }
    }
}
