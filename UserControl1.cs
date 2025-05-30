using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using walleproyect;
using System.IO;

namespace walleproyect
{
    public partial class UserControl1 : UserControl 
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();

            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Configurar el filtro para mostrar solo archivos .pw
            openFileDialog.Filter = "Archivos PW (*.pw)|*.pw";
            openFileDialog.FilterIndex = 1; // Selecciona el primer filtro por defecto
            openFileDialog.RestoreDirectory = true; // Restaura el directorio al cerrar

            // Mostrar el diálogo y verificar si se hizo clic en "Abrir"
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Leer todo el contenido del archivo
                    string contenido = File.ReadAllText(openFileDialog.FileName);

                    // Cargar el contenido en el TextBox
                    //Form1.title.Text = contenido;
                    

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
    }
}
