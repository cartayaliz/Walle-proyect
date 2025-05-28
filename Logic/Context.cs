using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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

        public List<(int, int, int, int, char)> Spawn(int i , int j)
        {
            SetColor(' ');
            var path = DrawLineFromPosition(x, y, 1, 0, i, true, x, y);
            var action = path[path.Count - 1];
            path.AddRange(DrawLineFromPosition(action.Item3, action.Item4, 0, 1, j, true, action.Item3, action.Item4));
            return path;

        }

        public void SetColor(char nbruch)
        {
            this.color = nbruch;
        }
        public void SetAndMove(int i, int j, int nx, int ny, char color)
        {            //pinta i, j del color elegido y mueve a Wally a nx,ny
            if (Inside(i,j))
            {
                x = nx;
                y = ny;
                if (color == ' ')
                {
                    return;
                }
                
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

        public void DoAction((int, int, int, int, char ) action)
        {
            SetAndMove(action.Item1, action.Item2, action.Item3, action.Item4, action.Item5);
        }


        // Métodos de pintar

       
        public List<(int, int, int, int, char)> DrawLine(int dirx, int diry, int distance)
        {
            //Pinta en linea recta en la direccion indicada tantas pasos como la distancia 
            return DrawLineFromPosition(x, y, dirx, diry, distance, true, x, y); 
        }
        public List<(int, int, int, int, char)> DrawRectangle(int dirx, int diry, int distance, int width, int heigth)
        {
            char oldcolor = this.color;
            this.color = ' ';

            List<(int, int, int, int, char)> path = DrawLine(dirx, diry, distance);
            
            var action = path[path.Count - 1];
            var xi = action.Item3;
            var xj = action.Item4;

            int mw = width / 2 ;
            int mh = heigth / 2 ;

            this.color = oldcolor;

            int starti = xi - mh;
            int startj = xj - mw;
            path.AddRange(DrawLineFromPosition(starti, startj, 0, 1, width - 1, false, xi, xj));

            starti = path[path.Count - 1].Item1;
            startj = path[path.Count - 1].Item2;
            path.AddRange(DrawLineFromPosition(starti, startj, 1, 0, heigth - 1, false, xi, xj));

            starti = path[path.Count - 1].Item1;
            startj = path[path.Count - 1].Item2;
            path.AddRange(DrawLineFromPosition(starti, startj, 0, -1, width - 1, false, xi, xj));

            starti = path[path.Count - 1].Item1;
            startj = path[path.Count - 1].Item2;
            path.AddRange(DrawLineFromPosition(starti, startj, -1, 0, heigth - 1, false, xi, xj));

            return path;
        }

        public List<(int, int, int, int, char)> DrawCuadrado(int dirx, int diry, int distance, int m)
        {
            List<(int, int, int, int, char)> path = DrawRectangle(dirx, diry, distance, m, m);
            return path;
        }

        public List<(int, int, int, int, char)> DrawCircle(int dirx, int diry, int radio)
        {
            char oldcolor = this.color;
            this.color = ' ';

            List<(int, int, int, int, char)> path = DrawLine(dirx, diry, radio);

            var action = path[path.Count - 1];
            var xi = action.Item3;
            var xj = action.Item4;

            this.color = oldcolor;

            path.AddRange(DrawLineFromPosition(xi - radio, xj - radio, 0, 0, 0 , false, xi, xj));
            path.AddRange(DrawLineFromPosition(xi - radio - 1, xj - radio + 1, 0, 1, radio + 1, false, xi, xj));

            path.AddRange(DrawLineFromPosition(xi - radio, xj + radio, 0, 0, 0, false, xi, xj));
            path.AddRange(DrawLineFromPosition(xi - radio + 1, xj + radio + 1, 1, 0, radio +1, false, xi, xj));

            path.AddRange(DrawLineFromPosition(xi + radio, xj + radio, 0, 0, 0, false, xi, xj));
            path.AddRange(DrawLineFromPosition(xi + radio + 1, xj + radio - 1, 0, -1, radio +1, false, xi, xj));

            path.AddRange(DrawLineFromPosition(xi + radio, xj - radio, 0, 0, 0, false, xi, xj));
            path.AddRange(DrawLineFromPosition(xi + radio - 1, xj - radio - 1, -1, 0, radio + 1, false, xi, xj));


            return path;
        }

        public List<(int, int, int, int, char)> DrawTriangle(int dirx, int diry, int distance, int tdirx, int tdiry,  int b)
        {
            char oldcolor = this.color;
            this.color = ' ';


            List<(int, int, int, int, char)> path = DrawLine(dirx, diry, distance);

            var action = path[path.Count - 1];
            var xi = action.Item3;
            var xj = action.Item4;

            this.color = oldcolor;

            int[] dx = { tdirx, -tdirx, 0};
            int[] dy = { 0, tdiry, -tdiry };


            for (int k = 0; k < dx.Length; k++)
            {
                path.AddRange(DrawLineFromPosition(xi, xj, dx[k], dy[k], b, true, xi, xj));
                xi = path[path.Count - 1].Item3;
                xj = path[path.Count - 1].Item4;
            }

            return path;
        }
        public List<(int, int, int, int, char)> DrawAsterisco(int dirx, int diry, int distance)
        {
            char oldcolor = this.color;
            this.color = ' ';

            List<(int, int, int, int, char)> path = DrawLine(dirx, diry, distance/2 );

            var action = path[path.Count - 1];
            var xi = action.Item3;
            var xj = action.Item4;

            this.color = oldcolor;

            int[] dx = { -1, -1, 0, 1, 1, 1, 0, -1 };
            int[] dy = { 0, 1, 1, 1, 0, -1, -1, -1 };


            for (int k = 0; k < dx.Length; k++)
            {
                path.AddRange(DrawLineFromPosition(xi , xj , dx[k], dy[k], distance / 2, true, xi, xj));
                xi = path[path.Count - 1].Item3;
                xj = path[path.Count - 1].Item4;
                path.AddRange(DrawLineFromPosition(xi, xj, -1 * dx[k], -1*dy[k], distance / 2, true, xi, xj));
                xi = path[path.Count - 1].Item3;
                xj = path[path.Count - 1].Item4;
            }

            return path;
        }


        // Métodos de ayuda

        public bool Inside(int i, int j)
        {
            return (i >= 0 && j >= 0 && i < n && j < n);
        }
        public List<(int, int, int, int, char)> DrawLineFromPosition(int i, int j, int dirx, int diry, int distance, bool moveWally, int xx, int yy)
        {
            List<(int, int, int, int, char)> path = new List<(int, int, int, int, char)>();

            int step = 0;

            while (distance > 0)
            {
                int ni = i + dirx, nj = j + diry;

                //if(step > 0)
                path.Add((i, j, (moveWally) ? ni : xx, (moveWally) ? nj : yy, this.color));

                i = ni;
                j = nj;
                step++;
                distance--;
            }

            path.Add((i, j, (moveWally) ? i : xx, (moveWally) ? j : yy, this.color));
            return path;
        }

    }
}
