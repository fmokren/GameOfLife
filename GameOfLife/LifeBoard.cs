using System;

namespace GameOfLife
{
    public class LifeBoard
    {
        private bool[,] board;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public LifeBoard(int width, int height)
        {
            board = new bool[width, height];
            Width = width;
            Height = height;
        }

        public LifeBoard(LifeBoard otherBoard, int width, int height) : this(width, height)
        {
            for (int y = 0; y < Math.Min(otherBoard.Height, Height); y++)
            {
                for (int x = 0; x < Math.Min(otherBoard.Width, Width); x++)
                {
                    this[x, y] = otherBoard[x, y];
                }
            }
        }

        public bool this[int x, int y]
        {
            get { return board[x, y]; }
            set { board[x, y] = value; }
        }

        public void PlayRound()
        {
            bool[,] next = new bool[Width, Height];

            for(int y = 0; y < Height; y++)
            {
                for(int x = 0; x < Width; x++)
                {
                    next[x, y] = GetNextState(x, y);
                }
            }

            board = next;
        }

        private bool GetNextState(int x, int y)
        {
            int aliveCount = 0;

            for (int yo = -1; yo <= 1; yo++)
            {
                for (int xo = -1; xo <= 1; xo++)
                {
                    if(xo == 0 && yo == 0)
                    {
                        continue;
                    }

                    int xn = xo + x;
                    int yn = yo + y;

                    if(yn == -1)
                    {
                        yn = Height - 1;
                    }
                    else if(yn == Height)
                    {
                        yn = 0;
                    }

                    if (xn == -1)
                    {
                        xn = Width - 1;
                    }
                    else if(xn == Width)
                    {
                        xn = 0;
                    }

                    if(this[xn, yn])
                    {
                        aliveCount++;
                    }
                }
            }

            // A living cell is sustained by 2 or 3 neighbors.
            if((aliveCount == 2 || aliveCount == 3) && this[x, y])
            {
                return true;
            }
            // A dead cell is revived by 3 living neighbors.
            else if(aliveCount == 3 && !this[x,y])
            {
                return true;
            }
            // All other conditions result in death.
            return false;
        }

        public void Randomize()
        {
            Random r = new Random();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    this[x, y] = ((r.Next() % 2 == 0) ? false : true);
                }
            }
        }

        public void Clear()
        {
            board = new bool[Width, Height];
        }
    }
}
