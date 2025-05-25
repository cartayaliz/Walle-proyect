using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{  
    public class Context
    {
        public int size { get; set; }
        public char color { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int n { get; set; }
        public char[,] M { get; set; }

        public Context(int n)
        {
            this.n = n;
            size = 1;
            color = '_';
            x = 0;
            y = 0;
            this.M = new char[n, n];
            CreateEmptyMatrix();
        }

        // Config methods
        public void CreateEmptyMatrix()
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    this.M[i, j] = '_';
        }

        public void Spawn(int i , int j)
        {
            this.x = i; this.y = j;
        }

        public void SetColor(char nbruch)
        {
            this.color = nbruch;
        }
        public void SetAndMove(int i, int j, int nx, int ny)
        {
            if (color == ' ')
            {
                return;
            }
            //pinta i, j del color elegido y mueve a Wally a nx,ny
            if (Inside(i,j))
            {
                //this.M[i, j] = color;
                int m = (size - 1) / 2;
                for ( int ki = i - m;  ki <= i + m;  ki++)
                {
                    for (int kj = j - m; kj <= j + m; kj++)
                    {
                        if (Inside(ki, kj))
                        {
                            this.M[ki, kj] = color;  
                        }
                    }
                }
            }
            if (Inside(nx, ny))
            {
                x = nx;
                y = ny;
            }
        }
        public void SetSize(int size)
        {
            if (size % 2 == 0)
            {
                this.size = size - 1;
            }
            else
            {
                this.size = size;
            }
           
        }

        public void DoAction((int, int, int, int) action)
        {
            SetAndMove(action.Item1, action.Item2, action.Item3, action.Item4);
        }


        // Métodos de pintar

        public List<(int, int, int , int)> DrawLine(int dirx, int diry, int distance)
        {
            List<(int, int, int, int)> path = new List<(int, int, int, int)>();

            int i = x, j = y;
            int step = 0;

            while(distance > 0 && Inside(i, j))
            {
                int ni = i + dirx, nj = j + diry;

                //if(step > 0)
                path.Add((i, j, ni, nj));

                i = ni;
                j = nj;
                step++;
                distance--;
            }

            if(Inside(i, j))
                path.Add((i, j, i, j));
            return path;
        }
        public List<(int, int, int, int)> DrawRectangle(int dirx, int diry, int distance, int width, int heigth)
        {
            List<(int, int, int, int)> path = new List<(int, int, int, int)>();
            int i = x, j = y;
            while (distance > 0 && Inside(i, j))
            {
                int ni = i + dirx, nj = j + diry;
                i = ni;
                j = nj;
                distance--;
            }


            return path;
        }



        // Métodos de ayuda

        public bool Inside(int i, int j)
        {
            return (i >= 0 && j >= 0 && i < n && j < n);
        }

    }
}
