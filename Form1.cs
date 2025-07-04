﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logic;
using System.IO;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Reflection;
using static System.Windows.Forms.LinkLabel;


namespace walleproyect
{

    public partial class Form1 : Form
    {
        Random rand = new Random();
       public Dictionary<char, Color> mappedChars;
       public Dictionary<string, char> invertedChars;
       public Dictionary<char, string> directChars;

        Dictionary<string, Color> highlight;

    public class ProgramExitException : Exception
        {
            public ProgramExitException(string message) : base(message) { }
        }

        class VisualLogguer : Ilogger
        {
            public bool HasError { get; set; }
            Form1 form;
            public VisualLogguer(Form1 form) {
                this.form = form;
            }
            

            public void Log(string prefix, string messagge, int line)
            {
                if(form.checkBox1.Checked)
                {
                    this.form.logText.AppendText($"INFO: [{prefix}] (line: {line}): {messagge}\r\n");
                }

            }

            public void LogError(string prefix, string messagge, int line)
            {

                HasError = true;
                
                this.form.logText.AppendText($"ERROR: [{prefix}] (line: {line}): {messagge}\r\n");
           

            }

            public void LogWarning(string prefix, string messagge, int line) 
            {
               
                this.form.logText.AppendText($"WARNING: [{prefix}] (line: {line}): {messagge}\r\n");

            }

            public void Clean()
            {
                this.form.logText.Text = "";
            }
        }

        Context context;
        private Image walleImage;
        public Form1()
        {

            mappedChars = new Dictionary<char, Color>()
            {
                { 'W', Color.White},
                { 'R', Color.Red },
                { 'B', Color.Blue },
                { 'G', Color.Green },
                { 'N', Color.Black },
                { 'Y', Color.Yellow },
                { 'O', Color.Orange },
                { 'P', Color.Purple },
                { '_', Color.Transparent },
                { 'A', Color.Aqua},
                { 'F', Color.Pink },
            };

            invertedChars = new Dictionary<string, char>();
            directChars = new Dictionary<char, string>();
           
            foreach (var c in mappedChars)
            {
                invertedChars.Add(c.Value.Name, c.Key);
                directChars.Add(c.Key, c.Value.Name);
            }
            context = new Context(30, new VisualLogguer(this));

            highlight = new System.Collections.Generic.Dictionary<string, Color>()
            {
                {"GoTo", Color.Red },
                {"<-", Color.Turquoise },
                { "\"", Color.Salmon }
            };

            foreach (var item in GLOBALS.ARGS_MAPPED.Keys)
            {
                highlight.Add(item, Color.BlueViolet);
            }

            walleImage = Properties.Resources.wally;
            InitializeComponent();
            // Cargar icono desde recursos embebidos
            using (var stream = Assembly.GetExecutingAssembly()
                                       .GetManifestResourceStream("walleproyect.Resources.Noctuline_Wall_E_Wall_E_512.ico"))
            {
                if (stream != null)
                    this.Icon = new Icon(stream);
            }
            EstablecerValoresPorDefecto();
            pictureBox1.Width = 600;
            pictureBox1.Height = 600;
            colors.Items.AddRange((mappedChars.Values.Select(c => c.Name).ToArray()));

            lector.Multiline = true;
            lector.WordWrap = false;
            lector.AcceptsTab = true;
            lector.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
  
            lector.SelectionFont = new Font("Courier New", 9, FontStyle.Regular);
            lector.SelectionColor = Color.Black;
            lineTexts.Clear();
            AddLineNumbers();
        }
        private void EstablecerValoresPorDefecto()
        {
            board.Value = 30;

            size.Value = 1;
            time.Value = 10;
            colors.Text = directChars[invertedChars[Color.Transparent.Name]];
            ExecuterTime.Value = 50;
            actual.Text = " ";
            checkBox1.Checked = false;
            checkBox2.Checked = true;
            checkBox3.Checked = true;

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

        private async Task InstruccionActions(Instruction inst)
        {

            if (!checkBox1.Checked)
            {
                return;
            }
            int waitinstr = (int)ExecuterTime.Value;
            if (waitinstr == 0) return; 
            foreach (var item in inst.pasos)
            {
                if (waitinstr != 0)
                {
                    await Task.Delay((int)waitinstr / 2);
                    actual.Text = item;
                }
                if (waitinstr != 0)
                {
                    await Task.Delay((int)waitinstr / 2);
                }
            }
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

            if(checkBox2.Checked)
            {

            for (int i = 0; i <= context.n; i++)
            {
                g.DrawLine(p, i * widthCell, 0, i * widthCell, height); //Dibujando lineas verticales
                g.DrawLine(p, 0, i * heightCell, width, i * heightCell);// Dibujando lineas horizontales
            }

            }


        }
        public void SRun()
        {
            button1.Enabled = false;
            button5.Enabled = false;
            lector.ReadOnly = true;
            context.x = 0;
            context.y = 0;
            pictureBox1.Refresh();

            var lines = lector.Lines;

            if (checkBox3.Checked)
            {
                
                lector.Text = "";

                foreach (var line in lines)
                {
                    ParseLine(line);
                }
            }

            lineTexts.Clear();
            AddLineNumbers();
        }
        public void ERun()
        {
            button1.Enabled = true;

            button5.Enabled = true;

            lector.ReadOnly = false;

        }


        private async void button1_Click(object sender, EventArgs e)
        {
            SRun();


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

            if (time.Value < 0)
            {
                error_time.SetError(time, "Ingrese un número entero mayor igual que cero");
            }
            
            context = new Context(n, new VisualLogguer(this));
            context.logger.Clean();

            var color = colors.SelectedItem?.ToString() ?? "Transparent";

            context.CreateEmptyMatrix();
            
            pictureBox1.Refresh();

            Executer exec = new Executer(context);
            Interprete inteprete = new Interprete(lector.Text, context.logger, exec);
            actual.Text = $"[{inteprete.actualline + 1}]: {inteprete.lines[inteprete.actualline]}";



            foreach (var inst in inteprete.Run())
            {
                await InstruccionActions(inst);

                actual.Text = $"[{inteprete.actualline + 1}]: {inteprete.lines[inteprete.actualline]}";

                if (inst.type == InstructionType.Draw)
                {
                    await PaintActions(exec.Run(inst));
                }
                
                if (inst.type == InstructionType.Request)
                {
                    
                   actual.Text = exec.GetRequestContext(inst.origin, inst.argument).Item2;
                }

                if(inst.type == InstructionType.GoTo)
                {
                    inteprete.pc = int.Parse(inst.argument[0].Item2);
                }
            }
            ERun();

        }

        void ParseLine(string line)
        {
            Regex r = new Regex("([ \\t{}():;\"])");
            string[] tokens = r.Split(line);

            foreach (string token in tokens)
            {
                lector.SelectionColor = Color.Black;
                lector.SelectionFont = new Font("Courier New", 9, FontStyle.Regular);
                if (highlight.ContainsKey(token))
                {
                    lector.SelectionColor = highlight[token];
                    lector.SelectionFont = new Font("Courier New", 9, FontStyle.Bold);
                }
                lector.SelectedText = token;
            }
            lector.SelectedText = "\n";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Restart();
            
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
                    string contenido =  lector.Text;

                    File.WriteAllText(saveFileDialog.FileName, contenido);

                    MessageBox.Show("Archivo guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) 
                {
                    MessageBox.Show($"Error al guardar el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }   

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Configurar el filtro para mostrar solo archivos .pw
            openFileDialog.Filter = "Archivos PW (*.pw)|*.pw";
            openFileDialog.FilterIndex = 1; // Selecciona el primer filtro por defecto
            openFileDialog.RestoreDirectory = true; // Restaura el directorio al cerrar
            // Mostrar el diálogo y verificar si se hizo clic en "Abrir"
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                lector.Text = "";
                try
                {
                    // Leer todo el contenido del archivo
                    string contenido = File.ReadAllText(openFileDialog.FileName);
                    string[] lineas = File.ReadAllLines(openFileDialog.FileName);
                    lector.Text = "";

                    //Cargar el contenido en el TextBox
                    for (int i = 0; i < lineas.Length; i++) { ParseLine(lineas[i]); }

                    lineTexts.Clear();
                    AddLineNumbers();

                    MessageBox.Show("Archivo cargado correctamente", "Éxito",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // Manejar errores (archivo corrupto, sin permisos, etc.)
                    MessageBox.Show($"Error al cargar el archivo:\n{ex.Message}", "Error",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void time_ValueChanged(object sender, EventArgs e)
        {

        }

        public int getWidth()
        {
            int w = 25;
            // get total lines of richTextBox1
            int line = lector.Lines.Length;

            
            w = 20 + (int)lector.Font.Size;
            
            return w;
        }
        public void AddLineNumbers()
        {
            // create & set Point pt to (0,0)
            Point pt = new Point(0, 0);
            // get First Index & First Line from richTextBox1
            int First_Index = lector.GetCharIndexFromPosition(pt);
            int First_Line = lector.GetLineFromCharIndex(First_Index);
            // set X & Y coordinates of Point pt to ClientRectangle Width & Height respectively
            pt.X = ClientRectangle.Width;
            pt.Y = ClientRectangle.Height;
            // get Last Index & Last Line from richTextBox1
            int Last_Index = lector.GetCharIndexFromPosition(pt);
            int Last_Line = lector.GetLineFromCharIndex(Last_Index);
            // set Center alignment to LineNumberTextBox
            lineTexts.SelectionAlignment = HorizontalAlignment.Center;
            // set LineNumberTextBox text to null & width to getWidth() function value
            lineTexts.Text = "";
            lineTexts.Width = getWidth();
            // now add each line number to LineNumberTextBox upto last line
            for (int i = First_Line; i <= Last_Line + 2; i++)
            {
                lineTexts.Text += i + 1 + "\r\n";
            }
        }

        public void lector_TextChanged(object sender, EventArgs e)
        {
            
        }

        public void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void lector_VScroll(object sender, EventArgs e)
        {
            lineTexts.Clear();
            AddLineNumbers();
            lineTexts.Invalidate();
        }

        private void logText_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }
    }
}

