using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace Tetris
{
    class Tetro
    {
        Random random = new Random();
        sbyte[,] gameField;
        sbyte[,] tetroField;
        public sbyte[,] TetroField
        {
            get { return tetroField; }
            private set { tetroField = value; }
        }

        TetroType type;
        public TetroType Type
        {
            get { return type; }
            set 
            { 
                type = value;
            }
        }

        int x, y;
        public int X
        {
            get { return x; }
            private set { x = value; }
        }
        public int Y
        {
            get { return y; }
            private set { y = value; }
        }

        public Tetro(sbyte[,] gameField, TetroType type)
        {
            this.Type = type;
            this.gameField = gameField;

            setTetroField();
            y = 0;
            x = gameField.GetUpperBound(1) / 2 - tetroField.GetUpperBound(1)/2;
            
            Fill();
        }
        private void setTetroField()
        {
            sbyte t = (sbyte)this.Type;
            switch (this.Type)
            {
                case TetroType.T:
                    TetroField = new sbyte[3, 3] 
                    { 
                        {-1, -1, -1 }, 
                        { t, t, t }, 
                        { -1, t, -1 }, 
                    };
                    break;

                case TetroType.Z:
                    this.TetroField = new sbyte[3, 3] 
                    { 
                        { t, t, -1 }, 
                        { -1, t, t }, 
                        { -1, -1, -1 }, 
                    };
                    break;

                case TetroType.S:
                    TetroField = new sbyte[3, 3] 
                    { 
                        { -1, t, t }, 
                        { t, t, -1 }, 
                        { -1, -1, -1 }, 
                    };
                    break;

                case TetroType.O:
                    TetroField = new sbyte[2, 2] 
                    {                         
                        { t, t }, 
                        { t, t }, 
                    };
                    break;

                case TetroType.L:
                    TetroField = new sbyte[3, 3] 
                    { 
                        { -1, t, -1 }, 
                        { -1, t, -1 }, 
                        { -1, t, t }, 
                    };
                    break;

                case TetroType.J:
                    TetroField = new sbyte[3, 3] 
                    { 
                        { -1, t, -1 }, 
                        { -1, t, -1 }, 
                        { t, t, -1 }, 
                    };
                    break;

                case TetroType.I:
                    TetroField = new sbyte[4, 4] 
                    { 
                        { -1, t, -1, -1}, 
                        { -1, t, -1, -1}, 
                        { -1, t, -1, -1}, 
                        { -1, t, -1, -1}, 
                    };
                    break;
            }
        }

        private void Fill()
        {
            for (int y = 0; y < TetroField.GetUpperBound(0) + 1; y++)
            {
                for (int x = 0; x < TetroField.GetUpperBound(1) + 1; x++)
                {
                    if (this.tetroField[y, x] != -1)
                        gameField[Math.Abs(y + this.Y), Math.Abs(x + this.X)] = tetroField[y, x];
                }
            }
        }
        private void Clear()
        {
            for (int y = 0; y < TetroField.GetUpperBound(0) + 1; y++)
            {
                for (int x = 0; x < TetroField.GetUpperBound(1) + 1; x++)
                {
                    if (this.tetroField[y, x] != -1)
                        gameField[Math.Abs(y + this.Y), Math.Abs(x + this.X)] = -1;
                }
            }
        }

        public bool Fall()
        {
            int summand = 1;
            
            for (int y = 0; y < TetroField.GetUpperBound(0) + 1; y++)
            {
                for (int x = 0; x < TetroField.GetUpperBound(1) + 1; x++)
                {
                    if (tetroField[y, x] != -1)
                    {
                        if (y == TetroField.GetUpperBound(1))
                            summand = 0;

                        if (gameField[this.Y + y + 1, this.X + x] != -1 && 
                            (summand == 0 || tetroField[y + summand, x] == -1))
                            return false;
                    }
                }
            }

            Clear();

            this.Y++;

            Fill();

            return true;
        }
        public bool KickDown()
        {
            int summand = 1;

            Clear();
            do
            {
                for (int y = 0; y < TetroField.GetUpperBound(1) + 1; y++)
                {
                    for (int x = 0; x < TetroField.GetUpperBound(0) + 1; x++)
                    {
                        if (tetroField[y, x] != -1)
                        {
                            if (y == TetroField.GetUpperBound(1))
                                summand = 0;
                            if (gameField[this.Y + y + 1, this.X + x] != -1 && (summand == 0 || tetroField[y + summand, x] == -1))
                            {
                                Fill();
                                return false;
                            }
                        }
                    }
                }

                this.Y++;
            }
            while (true);
        }
        public void Rotate()
        {
            if (this.Type != TetroType.O)
            {
                int center = 2;
                if(this.type == TetroType.I)
                    center = 3;


                sbyte[,] temp = new sbyte[TetroField.GetUpperBound(0) + 1, TetroField.GetUpperBound(0) + 1];

                for (int y = 0; y < TetroField.GetUpperBound(0) + 1; y++)
                {
                    for (int x = 0; x < TetroField.GetUpperBound(1) + 1; x++)
                    {
                        if (-y + center >= 0)
                            temp[-y + center, x] = TetroField[x, y];

                    }
                }
                for (int y = 0; y < TetroField.GetUpperBound(0) + 1; y++)
                {
                    for (int x = 0; x < TetroField.GetUpperBound(1) + 1; x++)
                    {
                        if (temp[y, x] != -1)
                        {
                            if (this.X < 0)
                            {
                                if (CheckRight())
                                {                                
                                    MoveRight();
                                }                                
                                else
                                    return;

                            }
                            else if (this.X + this.tetroField.GetUpperBound(1) + 1 > gameField.GetUpperBound(1) + 1)
                            {
                                if (CheckLeft())
                                {
                                    MoveLeft();
                                }
                                else
                                    return;
                            }
                        }
                    }
                }

                Clear();

                TetroField = temp;

                Fill();
            }
        }

        public void MoveRight()
        {
            Clear();
            this.X++;
            Fill();
        }
        public void MoveLeft()
        {
            Clear();
            this.X--;
            Fill();
        }
        public void MoveUp()
        {
            if (this.Y > 0)
            {
                Clear();
                this.Y--;
                Fill();
            }
        }

        public bool CheckRight()
        {
            int summand = 1;
            for (int x = 0; x < TetroField.GetUpperBound(1) + 1; x++)
            {
                for (int y = 0; y < TetroField.GetUpperBound(0) + 1; y++)
                {
                    if (x + this.X >= gameField.GetUpperBound(1))
                    {
                        if (tetroField[y, x] != -1)
                            return false;
                    }

                    if (tetroField[y, x] != -1)
                    {
                        if (x == TetroField.GetUpperBound(0))
                            summand = 0;
                        if (gameField[this.Y + y, this.X + x + 1] != -1 && (summand == 0 || tetroField[y, x + summand] == -1))
                            return false;
                    }
                    summand = 1;                    
                }
            }

            return true;
        }
        public bool CheckLeft()
        {
            int summand = 1;
            for (int x = 0; x < TetroField.GetUpperBound(1); x++)
            {
                for (int y = 0; y < TetroField.GetUpperBound(0); y++)
                {
                    if (x + this.X <= 0)
                    {
                        if (tetroField[y, x] != -1)
                            return false;
                    }

                    if (tetroField[y, x] != -1)
                    {
                        if (x == 0)
                            summand = 0;
                        if (gameField[this.Y + y, this.X + x - 1] != -1 && (summand == 0 || tetroField[y, x - summand] == -1))
                            return false;
                    }
                    summand = 1;
                }
            }

            return true;
        }
    }
}
