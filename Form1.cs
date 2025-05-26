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
using Logic;
using System.IO;
using System.Threading;


namespace walleproyect
{
    public partial class Form1 : Form
    {
        Random rand = new Random();
        Dictionary<char, Color> mappedChars;
        int n = 10;
        int PAINTING_TIME = 100;
        Context context;
        private Image walleImage;
        public Form1()
        {

            mappedChars = new Dictionary<char, Color>()
            {
                { '_', Color.White},
                { 'W', Color.Gray },
                { 'R', Color.Red },
                { 'B', Color.Blue },
                { 'G', Color.Green },
                { 'N', Color.Black },
                { 'Y', Color.Yellow },
                { 'O', Color.Orange },
                { 'P', Color.Purple },
                { ' ', Color.Transparent },
            };

            context = new Context(n);

            context.Spawn(1, 1);
            walleImage = Image.FromFile(@"C:\Users\liz\Desktop\Nueva carpeta (2)\walleproyect\Image.jpg");
            InitializeComponent();
            pictureBox1.Width = 500;
            pictureBox1.Height = 500;
            Console.WriteLine(context.n);

        }
        private void _Refresh()
        {

            // Aquí toda la información dependiente del contexto se debe actualizar

            //Actualizando en Board
            pictureBox1.Refresh();
        }

        private async Task PaintActions(List<(int, int, int, int, char)> path)
        {
            foreach (var item in path)
            {
                context.DoAction(item);
                if (PAINTING_TIME != 0) _Refresh();
                await Task.Delay(PAINTING_TIME);
            }
            if (PAINTING_TIME == 0) _Refresh();
        }
        private async void button1_Click(object sender, EventArgs e)
        {

            context.Spawn(2, 2);
            context.CreateEmptyMatrix();
            context.SetSize(1);


            context.SetColor('R');
            var path = context.DrawCircle(1, 1, 2);
            await PaintActions(path);

            context.SetColor('B');
            path = context.DrawCuadrado(1, 1, 0, 5);
            await PaintActions(path);

            context.SetColor('G');
            path = context.DrawRectangle(1, 1, 0, 7, 7);
            await PaintActions(path);

            //context.SetSize(3);
            //context.SetColor('G');
            //var path2 = context.DrawLine(1, 1, 1);
            //await PaintActions(path2);

            //context.SetColor('B');
            //path2 = context.DrawLine(0, -1, n);
            //await PaintActions(path2);

            //context.SetColor('Y');
            //path2 = context.DrawLine(-1, 0, n - 2);
            //await PaintActions(path2);

            //path2 = context.DrawLine(0, 1, n - 2);
            //await PaintActions(path2);

            //path2 = context.DrawLine(1, 0, n - 3);
            //await PaintActions(path2);

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
                    if (i == context.x && j == context.y)
                    {
                        //Pintando ubicacion de Walle
         
                        RectangleF destRect = new RectangleF(xx, yy, widthCell, heightCell);  // Crear un rectángulo del tamaño de la celda

                        g.DrawImage(walleImage, destRect); // Dibujar la imagen escalada

                        //g.FillRectangle(Brushes.Gray, xx, yy, widthCell, heightCell);
                        //g.DrawEllipse(new Pen(Brushes.Magenta, 2), xx, yy, widthCell, heightCell);
                        //Font fuente = new Font("Arial", 15, FontStyle.Bold);
                        //Brush pincel = Brushes.DarkViolet;
                        //g.DrawString("W", fuente, pincel, xx + widthCell/4, yy + heightCell / 4);

                    }
                    else
                    {
                        SolidBrush brush = new SolidBrush(mappedChars[context.M[i, j]]);
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

