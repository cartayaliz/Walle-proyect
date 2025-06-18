using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography.X509Certificates;
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
        public string titulo { get; set; }

        public int line { get; set; }


        public Ilogger logger;

        public Context(int n, Ilogger logger)
        {
            this.n = n;
            this.logger = logger;
            size = 1;
            color = '_';
            x = 0;
            y = 0;
            this.M = new char[n, n];
            CreateEmptyMatrix();
            titulo = this.titulo;
            line = this.line;

        }
        public Dictionary<string, Color> Colors = new Dictionary<string, Color>()
        {
            { "Transparent", Color.Transparent },
            { "Red", Color.Red },
            { "Blue", Color.Blue },
            { "Negro", Color.Black },
            { "White", Color.White}, 
            { "Green", Color.Green },
            { "Yellow", Color.Yellow },
            { "Orange", Color.Orange },
            { "Purple", Color.Purple },
            { "Aqua", Color.Aqua},
            { "Fushia", Color.Pink },
        };

        // Config methods
        public void CreateEmptyMatrix()
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    this.M[i, j] = 'W';
        }
        public List<(int, int, int, int, char)> Spawn(int i , int j)
        {
            var oldcolor = this.color;
            SetColor(' ');

            var b = j - y;
            var sigb = (b > 0) ? 1 : (b == 0) ? 0 : -1;
            var path = DrawLineFromPosition(x, y, 0, sigb, Math.Abs(b), true, x, y);
            var action = path[path.Count - 1];
            var c = i - x;
            var sigc = (c > 0) ? 1 : (c == 0) ? 0 : -1;
            path.AddRange(DrawLineFromPosition(action.Item3, action.Item4, sigc, 0, Math.Abs(c), true, action.Item3, action.Item4));
            SetColor(oldcolor);
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
                if(Inside(nx, ny))
                {
                    x = nx;
                    y = ny;
                }
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
                            this.M[ki, kj] = this.color;  
                        }
                    }
                }
            }
            else
                logger.LogWarning("", $"Tratando de dibujar {i} {j}", 0);
           
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

            path.AddRange(DrawLineFromPosition(xi - radio, xj - radio, 0, 0, 0 , true, xi, xj));
            path.AddRange(DrawLineFromPosition(xi - radio - 1, xj - radio + 1, 0, 1, radio * 2 - 2, true, xi, xj));

            path.AddRange(DrawLineFromPosition(xi - radio, xj + radio, 0, 0, 0, true, xi, xj));
            path.AddRange(DrawLineFromPosition(xi - radio + 1, xj + radio + 1, 1, 0, radio * 2 - 2, true, xi, xj));

            path.AddRange(DrawLineFromPosition(xi + radio, xj + radio, 0, 0, 0, true, xi, xj));
            path.AddRange(DrawLineFromPosition(xi + radio + 1, xj + radio - 1, 0, -1, radio * 2 - 2, true, xi, xj));

            path.AddRange(DrawLineFromPosition(xi + radio, xj - radio, 0, 0, 0, true, xi, xj));
            path.AddRange(DrawLineFromPosition(xi + radio - 1, xj - radio - 1, -1, 0, radio * 2 - 2, true, xi, xj));

            var hi = path[path.Count - 1].Item3;
            var hj = path[path.Count - 1].Item4;
            path.AddRange(DrawLineFromPosition(hi, hj, -1, 1, 1, true, xi, xj));

            this.color = ' ';
            var ki = path[path.Count - 1].Item3;
            var kj = path[path.Count - 1].Item4;
            path.AddRange(DrawLine(dirx, diry, radio));
            this.color = oldcolor;

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

        public List<(int, int, int, int, char)> DrawRombo(int dirx, int diry, int r)
        {


            List<(int, int, int, int, char)> path = new List<(int, int, int, int, char)>();



            path.AddRange(DrawTriangle(dirx + r, diry, 0, 1, -1, r));


            path.AddRange(DrawTriangle(dirx, diry +r, 0, -1, -1, r));


            path.AddRange(DrawTriangle(dirx - r, diry , 0, 1, 1, r));

            path.AddRange(DrawTriangle(0, 0, r, -1, 1, r));



            return path;

        }

        public List<(int, int)> ExpandB(int i, int j, char c)
        {
            List<(int, int)> result = new List<(int, int)>();
            bool[,] used = new bool[n, n];
            Queue<(int, int)> Q = new Queue<(int, int)>();
            Q.Enqueue((i, j));
            int[] A = { 0, 1, 0, -1 };
            int[] B = { 1, 0, -1, 0 };
           

            while (Q.Count >0)
            {
              (i, j) = Q.Dequeue();
                result.Add((i, j));
                for (int k = 0; k < A.Length; k++)
                {
                    int ni = i + A[k];
                    int nj = j + B[k];

                    if (Inside(ni, nj) && M[ni, nj] == c && !used[ni, nj])
                    {
                        Q.Enqueue((ni, nj));
                        used[ni, nj] = true;
                    }
                }

            }
            return result;

        }
        public List<(int, int)> Expand(int x, int y, char c)
        {
            List<(int, int)>  result = new List<(int, int)>();
            bool[,] used = new bool[n, n];

            void ExpandMask(int i, int j, char color)
            {

                result.Add((i, j));
                used[i, j] = true;

                int[] A = { 0, 1, 0, -1 };
                int[] B = { 1, 0, -1, 0 };
                for (int k = 0; k < A.Length; k++)
                {
                    int ni = i + A[k];
                    int nj = j + B[k];

                    if (Inside(ni, nj) && M[ni, nj] == color &&  !used[ni, nj])
                    {
                        ExpandMask(ni, nj, color);
                       
                    }
                }

            }
            ExpandMask(x, y, c);
            return result;
         
        }
        public List<(int, int, int, int, char)> Fill()
        {
            var result = new List<(int, int, int, int, char)>();

            var exp = Expand(x, y, M[x, y]);

            foreach (var item in exp)
            {
                result.Add((item.Item1, item.Item2, x, y, this.color));
            }

            return result;
        }
        public List<(int, int, int, int, char)> FillB()
        {

            var result = new List<(int, int, int, int, char)>();

            var exp = ExpandB(x, y, M[x, y]);

            foreach (var item in exp)
            {
                result.Add((item.Item1, item.Item2, x, y, this.color));
            }

            return result;
        }



        // Métodos de informacion

        public int IsBrushSize(int size)
        {
            if (size == this.size) return 1;
            return 0;
           
        }

        public int IsBrushColor(string color)
        {
            char c = color[0];
            if (string.IsNullOrEmpty(color))
                return 0;
            if (c  == this.color ) return 1; return 0;
        }
  
        public int GetActualX()
        {
            return this.x;
        }
      
        public int GetActualY()
        {
            return this.y;
        }

        public int GetCanvasSize()
        {
            return this.n;
        }

        public int IsCellColor(int i, int j, char color)
        {
            if (!Inside(i, j) )
                return 0;
            return (M[i, j] == color) ? 1 : 0;
        }
      
        public int IsCanvasColor(char color, int vertical, int horizontal)
        {
            int xi = this.x + horizontal;
            int yi = this.y + vertical;
           
            return IsCellColor(xi, yi, color); 
                
        }

        public int GetColorCount(char color, int x1, int y1, int x2, int y2)
        {
            if (!Inside(x1, y1) || !Inside(x2, y2))
            {
                return 0;
            }
            int c  = 0;
            for (int i = x1; i <= x2; i++)
            {
                for (int j = y1; j <= y2; j++)
                {
                    if (IsCellColor(i, j, color) == 1)
                    {
                        c++;
                    }
                }
            }
            return c;
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
