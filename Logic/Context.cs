using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Context
    {
        public int Size { get; set; }
        public char Color { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        public int n { get; set; }
        public char[,] M { get; set; }
        
        public Context(int n)
        {
            Size = this.Size;
            Color = this.Color;
            x = this.x;
            y = this.y;
           this.M = new char[n, n];
            Matrix(n);
        }
         private void Matrix(int n)
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                   this.M[i, j] = '_';
        }
        public void SetWally(int i, int j)
        {
            this.x = i;
            this.y = j;
        }
        public void Set(int i, int j, char color)
        {
           this.M[i, j] = color;
        }

    }
}
