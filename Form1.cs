using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace walleproyect
{
    public partial class Form1 : Form
    {
        Random rand = new Random();
        Dictionary<char, Color> mappedChars = new Dictionary<char, Color>();
        Color[,] colors;
        int n = 50;
        int x = 5; int y = 6;
        public Form1()
        {
            colors = new Color[n, n];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    colors[i, j] = Color.White;

            InitializeComponent();
            pictureBox1.Width = 500;
            pictureBox1.Height = 500;

            //
        }

        private void button1_Click(object sender, EventArgs e)
        {
          // Con cada click pinta de un color aleatorio una casilla aleatoria
            Color color = Color.FromArgb(
                rand.Next(256), // Rojo
                rand.Next(256), // Verde
                rand.Next(256)  // Azul
            );
            int i = rand.Next(n);
            int j = rand.Next(n);
            colors[i, j] = color;
            pictureBox1.Refresh();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            
            int width = pictureBox1.Width;
            int height = pictureBox1.Height;
            Console.WriteLine(width);
            Console.WriteLine(height);


            int widthCell = width / n;   
            int heightCell = height / n;

            Pen p = new Pen(Color.Black);    // Para pintar bordes en negro
           
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    float xx = j * widthCell;
                    float yy = i * heightCell;
                    if(i == x && j == y)
                    {
                        //Pintando ubicacion de Walle
                        g.FillRectangle(Brushes.Gray, xx, yy, widthCell, heightCell);
                        g.DrawEllipse(new Pen(Brushes.Magenta, 2), xx, yy, widthCell, heightCell);
                        Font fuente = new Font("Arial", 15, FontStyle.Bold);
                        Brush pincel = Brushes.DarkViolet;
                        g.DrawString("W", fuente, pincel, xx + widthCell/4, yy + heightCell / 4);

                    }
                    else
                    {
                        SolidBrush brush = new SolidBrush(colors[i, j]);
                        g.FillRectangle(brush, xx, yy, widthCell, heightCell);
                    }
                }
            }
            
            for (int i = 0; i <= n; i++)
            {
                g.DrawLine(p, i * widthCell, 0, i * widthCell, height); //Dibujando lineas verticales
                g.DrawLine(p, 0, i * heightCell, width, i * heightCell);// Dibujando lineas horizontales
            }
        }
    }
}
