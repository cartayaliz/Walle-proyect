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
        Dictionary<string, char> invertedChars;
        Dictionary<char, string> directChars;



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

            invertedChars = new Dictionary<string, char>();
            directChars = new Dictionary<char, string>();
            foreach (char c in mappedChars.Keys)
            {
                invertedChars.Add(mappedChars[c].Name, c);
                directChars.Add(c, mappedChars[c].Name);
            }

            context = new Context(20);
            walleImage = Image.FromFile(@"C:\Users\liz\Desktop\Nueva carpeta (2)\walleproyect\Image.jpg");
            InitializeComponent();
            EstablecerValoresPorDefecto();
            pictureBox1.Width = 600;
            pictureBox1.Height = 600;
            Console.WriteLine(context.n);

        }
        private void EstablecerValoresPorDefecto()
        {
            board.Value = 20;
            size.Value = 1;
            time.Value = 50;
            colors.Text = directChars[invertedChars[Color.Black.Name]];
            title.Text = " ";
            actual.Text = " ";
        }
        private void _Refresh()
        {

            // Aquí toda la información dependiente del contexto se debe actualizar
            size.Value = context.size;
            colors.Text = directChars[context.color];
            //Actualizando en Board
            pictureBox1.Refresh();
        }

        private async Task PaintActions(List<(int, int, int, int, char)> path)
        {
            foreach (var item in path)
            {
                context.DoAction(item);
                if (time.Value != 0) _Refresh();

                await Task.Delay((int)time.Value);
            }
            if (time.Value == 0) _Refresh();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Restart();
            
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            // Examples

            if (string.IsNullOrWhiteSpace(board.Text))
            {
                MessageBox.Show("¡Debe ingresar un número!", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (board.Value <= 0)
            {
                error_board.SetError(board, "Ingrese un número entero mayor que cero");
            }
            int n = (int)board.Value;
           
            if (size.Value <= 0)
            {
                error_size.SetError(size, "Ingrese un número entero mayor que cero");
            }
            if (string.IsNullOrWhiteSpace(colors.Text))
            {
                MessageBox.Show("¡Debe escoger un color !", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (time.Value <= 0)
            {
                error_time.SetError(time, "Ingrese un número entero mayor que cero");
            }
            Interprete inteprete = new Interprete(lector.Text);

           
            var color = colors.Text;
            actual.Text = $"[{inteprete.actualline + 1}]: {inteprete.lines[inteprete.actualline]}";

            context = new Context(n);
            context.CreateEmptyMatrix();

            var path = context.Spawn(5,5);
            await PaintActions(path);

            context.SetSize((int)size.Value);
            context.SetColor(invertedChars[color]);

            path = context.DrawCircle(1, 1, 3);
            await PaintActions(path);

        }


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int width = pictureBox1.Width;
            int height = pictureBox1.Height;

            int widthCell = width / context.n;
            int heightCell = height / context.n;

            Pen p = new Pen(Color.Black);    // Para pintar bordes en negro


            for (int i = 0; i < context.n; i++)
            {
                for (int j = 0; j < context.n; j++)
                {
                    float xx = j * widthCell;
                    float yy = i * heightCell;
                    if (i == context.x && j == context.y)
                    {
                        //Pintando ubicacion de Walle
         
                        RectangleF destRect = new RectangleF(xx, yy, widthCell, heightCell);  // Crear un rectángulo del tamaño de la celda

                        g.DrawImage(walleImage, destRect); // Dibujar la imagen escalada

                      

                    }
                    else
                    {
                        SolidBrush brush = new SolidBrush(mappedChars[context.M[i, j]]);
                        g.FillRectangle(brush, xx, yy, widthCell, heightCell);
                    }
                }

            }


            for (int i = 0; i <= context.n; i++)
            {
                g.DrawLine(p, i * widthCell, 0, i * widthCell, height); //Dibujando lineas verticales
                g.DrawLine(p, 0, i * heightCell, width, i * heightCell);// Dibujando lineas horizontales
            }


        }


        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
       
        

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivos PW (*.pw)|*.pw";
            saveFileDialog.FilterIndex = 1; // Por defecto se selecciona el primer filtro
            saveFileDialog.RestoreDirectory = true; // Restaura el directorio actual al cerrar el diálogo
            if (saveFileDialog.ShowDialog(Owner) == DialogResult.OK)
            {
                try
                {

                    
                    File.WriteAllText(saveFileDialog.FileName, lector.Text);

                    MessageBox.Show("Archivo guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) 
                {
                    MessageBox.Show($"Error al guardar el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

       
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void time_ValueChanged(object sender, EventArgs e)
        {

        }

        public void lector_TextChanged(object sender, EventArgs e)
        {

        }

        public void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        //private void button5_Click(object sender, EventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();

        //    // Configurar el filtro para mostrar solo archivos .pw
        //    openFileDialog.Filter = "Archivos PW (*.pw)|*.pw";
        //    openFileDialog.FilterIndex = 1; // Selecciona el primer filtro por defecto
        //    openFileDialog.RestoreDirectory = true; // Restaura el directorio al cerrar

        //    // Mostrar el diálogo y verificar si se hizo clic en "Abrir"
        //    if (openFileDialog.ShowDialog() == DialogResult.OK)
        //    {
        //        try
        //        {
        //            // Leer todo el contenido del archivo
        //            string contenido = File.ReadAllText(openFileDialog.FileName);

        //            // Cargar el contenido en el TextBox
        //            //Form1.title.Text = contenido;


        //            MessageBox.Show("Archivo cargado correctamente", "Éxito",
        //                           MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //        catch (Exception ex)
        //        {
        //            // Manejar errores (archivo corrupto, sin permisos, etc.)
        //            MessageBox.Show($"Error al cargar el archivo:\n{ex.Message}", "Error",
        //                           MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //}
    }
}

