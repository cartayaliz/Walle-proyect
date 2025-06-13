using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logic
{
    public class Executer
    {
        Context context;
        
        public Executer(Context context) 
        {
            this.context = context;
        }

        public List<(int, int, int, int, char)> Run(Instruction ins)
        {
            var id = ins.origin.b.lexeme;

            if(id == "Spawn")
            {
                int x = int.Parse(ins.argument[0].Item2);
                int y = int.Parse(ins.argument[1].Item2);
                return context.Spawn(x, y);
            }
            if(id == "DrawLine")
            {
               
                int x = int.Parse(ins.argument[0].Item2);
                
                int y = int.Parse(ins.argument[1].Item2);
              
                int z = int.Parse(ins.argument[2].Item2);


                return context.DrawLine(x, y, z);
            }
            if (id == "DrawCircle")
            {
                int x = int.Parse(ins.argument[0].Item2);
                int y = int.Parse(ins.argument[1].Item2);
                int radio = int.Parse(ins.argument[2].Item2);
                return context.DrawCircle(x, y, radio);
            }
            if (id == "DrawCuadrado")
            {
                int x = int.Parse(ins.argument[0].Item2);
                int y = int.Parse(ins.argument[1].Item2);
                int z = int.Parse(ins.argument[2].Item2);
                int h = int.Parse(ins.argument[3].Item2);
                return context.DrawCuadrado(x, y,z, h );
            }
            if (id == "DrawRectangle")
            {
                int x = int.Parse(ins.argument[0].Item2);
                int y = int.Parse(ins.argument[1].Item2);
                int z = int.Parse(ins.argument[2].Item2);
                int h = int.Parse(ins.argument[3].Item2);
                int i = int.Parse(ins.argument[4].Item2);
                return context.DrawRectangle(x, y, z, h, i);
            }
            if (id == "DrawCuadrado")
            {
                int x = int.Parse(ins.argument[0].Item2);
                int y = int.Parse(ins.argument[1].Item2);
                int z = int.Parse(ins.argument[2].Item2);
                int h = int.Parse(ins.argument[3].Item2);
                int i = int.Parse(ins.argument[4].Item2);
                int j = int.Parse(ins.argument[5].Item2);

                return context.DrawTriangle(x, y, z, h, i, j);
            }
            if (id == "DrawAsterisco")
            {
                int x = int.Parse(ins.argument[0].Item2);
                int y = int.Parse(ins.argument[1].Item2);
                int z = int.Parse(ins.argument[2].Item2);
              
                return context.DrawAsterisco(x,y,z);
            }
            if (id == "DrawRombo")
            {
                int x = int.Parse(ins.argument[0].Item2);
                int y = int.Parse(ins.argument[1].Item2);
                int z = int.Parse(ins.argument[2].Item2);

                return context.DrawRombo(x, y, z);
            }
            if(id == "DrawTriangle")
            {
                int x = int.Parse(ins.argument[0].Item2);
                int y = int.Parse(ins.argument[1].Item2);
                int z = int.Parse(ins.argument[2].Item2);
                int h = int.Parse(ins.argument[3].Item2);
                int i = int.Parse(ins.argument[4].Item2);
                int j = int.Parse(ins.argument[5].Item2);

                return context.DrawTriangle(x, y, z, h, i,j);
            }
            if (id == "Color")
            {
               
                if (ins.argument[0].Item2.Length != 1)
                {
                    string colorName = ins.argument[0].Item2;

                    if (string.IsNullOrWhiteSpace(colorName))
                    {
                        context.logger.LogError("Exe", $"Color no puede estar vacío.", ins.origin.b.line);
                        
                        return new List<(int, int, int, int, char)>();
                      
                    }

                    var colorChar = colorName[0];
                    List<(int, int, int, int, char)> H = new List<(int, int, int, int, char)>();
                    if (colorChar == 'W')
                    {
                        context.SetColor('_');
                    }
                    else
                    {
                        context.SetColor(colorChar);
                    }
                    return H;

            
                }
                
            }
            if (id == "Fill")
            {

                return context.Fill();

            }


            context.logger.LogError("Exe", $"Missing method draw: {id}", ins.origin.b.line);

            return new List<(int, int, int, int, char)>();
        }
        public (string, string) GetRequestContext(Instruction ins)
        {
            var id = ins.origin.b.lexeme;
            if (id == "GetActualX")
            {
                return ("int",context.GetActualX().ToString());
            }
            if (id == "GetActualY")
            {
                return ("int", context.GetActualY().ToString());
            }
            if (id == "GetCanvasSize")
            {
                return ("int", context.GetCanvasSize().ToString());
            }
            if (id == "IsCanvasColor")
            {
                string colorName = ins.argument[0].Item2;
                int x = int.Parse(ins.argument[1].Item2);
                int y = int.Parse(ins.argument[2].Item2);

                var colorChar = colorName[0];
                return ("int", context.IsCanvasColor(colorChar, x, y).ToString());
            }
            if (id == "IsBrushSize")
            {
                int x = int.Parse(ins.argument[0].Item2);
                return ("int", context.IsBrushSize(x).ToString());
            }
            if (id == "IsBrushColor")
            {
                string colorName = ins.argument[0].Item2;
                return ("int", context.IsBrushColor(colorName).ToString());
            }
            if (id == "GetColorCount")
            {
                string name = ins.argument[0].Item2;
                char color = name[0];
                int x = int.Parse(ins.argument[1].Item2);
                int y = int.Parse(ins.argument[2].Item2);
                int z = int.Parse(ins.argument[3].Item2);
                int v = int.Parse(ins.argument[4].Item2);
                return ("int", context.GetColorCount(color, x, y, z, v).ToString());
            }
            if( id == "Size")
            {
                int x = int.Parse (ins.argument[0].Item2);
          
                context.SetSize(x);

              
            }
          

            context.logger.LogError("Exe", $"Missing method Request: [{id}]", ins.origin.b.line);
            return ("", "");
        }


    }
}
