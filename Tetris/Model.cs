using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Diagnostics;

namespace Tetris
{
    public class Model
    {
        Random random = new Random();
        bool GameAlive = true;
        Graphics gfx;

        sbyte[,] gameField;
        Tetro currentTetro;
        Bitmap playGroundImage;
        public Bitmap PlayGroundImage
        {
            get { return playGroundImage; }
            private set { playGroundImage = value; }
        }
        int width = 10, height = 16;

        public Model()
        {
            gameField = new sbyte[height, width];            
            playGroundImage = new Bitmap(width * 20, height * 20);
            createNewTetro();
            
            Debug.Print(string.Format("height: {0} width: {1}", gameField.GetUpperBound(0), gameField.GetUpperBound(1)));
            for(int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    if (j == height - 1)
                        gameField[gameField.GetUpperBound(0), i] = 0;
                    else
                        gameField[j, i] = -1;
                }

            gfx = Graphics.FromImage(playGroundImage);

            drawGame();
        }

        int fps = 0, frames = 0;
        long timeStarted = Environment.TickCount;
        int plusTime = 750;
        public void Play()
        {

            while (GameAlive)
            {
                if (Environment.TickCount >= timeStarted + plusTime)
                {
                    fps = frames;
                    frames = 0;
                    timeStarted = Environment.TickCount;
                    if (currentTetro.Fall())
                    {

                    }
                    else if (currentTetro.Y != 0)
                    {
                        clearLines();                     
                        createNewTetro();
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("!");
                        GameAlive = false;
                    }
                }
                    drawGame();
                frames++;

            }
        }
        private void drawGame()
        {
            Monitor.Enter(playGroundImage);

            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (gameField[y, x] != -1)
                    {
                        gfx.DrawImage(Tetris.Properties.Resources.tiles,
                          x * 20, y * 20,
                          new Rectangle(gameField[y, x], 0, 20, 20),
                          GraphicsUnit.Pixel);
                    }
                    else
                    {
                        gfx.DrawImage(Tetris.Properties.Resources.tiles,
                        x * 20, y * 20,
                        new Rectangle(140, 0, 20, 20),
                        GraphicsUnit.Pixel);
                    }

                    if (x == 0 && y == 0)
                        gfx.DrawString("0", new Font("", 6), Brushes.Black, x * 20, y * 20);
                    else if (x == 0 && y != 0)
                        gfx.DrawString(y.ToString(), new Font("", 6), Brushes.Black, x * 20, y * 20);
                    else if (x != 0 && y == 0)
                        gfx.DrawString(x.ToString(), new Font("", 6), Brushes.Black, x * 20, y * 20);

                }
            }

            Monitor.Exit(playGroundImage);
        }
        void createNewTetro()
        {
            int r = (random.Next(70)/10);
            switch (r)
            {
                case 0:
                    currentTetro = new Tetro(gameField, TetroType.T);
                    break;

                case 1:
                    currentTetro = new Tetro(gameField, TetroType.I);
                    break;

                case 2:
                    currentTetro = new Tetro(gameField, TetroType.J);
                    break;

                case 3:
                    currentTetro = new Tetro(gameField, TetroType.L);
                    break;

                case 4:
                    currentTetro = new Tetro(gameField, TetroType.O);
                    break;

                case 5:
                    currentTetro = new Tetro(gameField, TetroType.S);
                    break;

                case 6:
                    currentTetro = new Tetro(gameField, TetroType.Z);
                    break;

            }
            //t = new Tetro(gameField, TetroType.O);
        }

        public void RotateCurrentTetro()
        {
            currentTetro.Rotate();
            drawGame();
        }
        public void MoveCurrentTetroRight()
        {
            if (currentTetro.CheckRight())
            {
                currentTetro.MoveRight();
                drawGame();
            }
        }
        public void MoveCurrentTetroLeft()
        {
            if (currentTetro.CheckLeft())
            {
                currentTetro.MoveLeft();
                drawGame();
            }
        }
        public void SpeedUp()
        {
            plusTime = 50;
        }
        public void StopSpeedUp()
        {
            plusTime = 750;
        }
        public void Finish()
        {
            currentTetro.KickDown();
            clearLines();
            createNewTetro();
            drawGame();            
        }
        
        int clearLines()
        {
            int index = 0;
            int blocks = 0;
            
            for (int y = 1; y < 15; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (gameField[y, x] != -1)
                        blocks++;
                }

                if (blocks == width)
                {
                    Debug.Print(" - LINE FULL - ");
                    index = y;
                    clearLine(index);
                    dropLines(index);
                }
                blocks = 0;
            }

            return index;
        }
        void clearLine(int index)
        {
            for (int i = 0; i < width; i++)
                gameField[index, i] = -1;

        }
        void dropLines(int index)
        {
            for (int y = index; y > 1; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    gameField[y, x] = gameField[y - 1, x];
                    gameField[y - 1, x] = -1;
                }
            }
        }

    }
}

