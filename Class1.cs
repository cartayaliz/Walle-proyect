using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;
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
        
        public Scanner scannner;
        public Interprete(string text)
        {
            this.text = text;
            scannner = new Scanner(text);

            lines = text.Split('\n');

            var tokens = scannner.scanTokens();

            foreach (var item in tokens)
            {
                Console.WriteLine(item);
            }

        }

    }
}
