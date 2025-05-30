using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
 
    public class Lexel
    {
        public Lexel() 
        { 
          
        }
    }

    public class Scanner
    {
        private String source { get; set; }
        private List<Tokens> tokens { get; set; }
        Scanner(String source)
        {
            this.source = source;

        }
    }
}

