using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using Logic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net;

namespace Logic
{
    public class Interprete 
    {
        public string[] lines {  get; set; }
        public string text { get; set; }
        public int actualline { get; set; }
        public Interprete(string text)
        {
            this.text = text;

            lines = text.Split('\n');

            Console.WriteLine(">>> lines");
            for (int i = 0; i < lines.Length; i++)
            {

                Console.WriteLine("[{0}]: {1}", i, lines[i]);
            }

            this.actualline = 0;

        }

    }
}
