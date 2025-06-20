﻿using System;
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
               
                    string colorName = ins.argument[0].Item2;

                    if (string.IsNullOrWhiteSpace(colorName))
                    {
                        context.logger.LogError("Exe", $"Color cant be empty.", ins.origin.b.line);
                        
                        return new List<(int, int, int, int, char)>();
                      
                    }
                if (!context.Colors.ContainsKey(colorName))
                {
                    context.logger.LogError("Exe", $"Invalid Color", ins.origin.b.line);

                    return new List<(int, int, int, int, char)>();

                }
                
                    var colorChar = colorName[0];

                    List<(int, int, int, int, char)> H = new List<(int, int, int, int, char)>();

                    context.SetColor(colorChar);

                    return H;

                
            }
            if (id == "Fill")
            {

                return context.Fill();

            }
            if (id == "FillB")
            {

                return context.FillB();

            }
            if (id == "Size")
            {
                int h = int.Parse(ins.argument[0].Item2);
                if (h <= 0)
                {
                    context.logger.LogError("Exe", $"Invalid size", ins.origin.b.line);

                    return new List<(int, int, int, int, char)>();
                }

                context.SetSize(h);
                return new List<(int, int, int, int, char)>();



            }
          
            context.logger.LogError("Exe", $"Missing method draw: {id}", ins.origin.b.line);

            return new List<(int, int, int, int, char)>();
        }
        public (string, string) GetRequestContext(ASTNode origin, List<(string, string)> args)
        {
            var id = origin.b.lexeme;
            if (id == "GetActualX")
            {
                return (GLOBALS.NUMBER_TYPE, context.GetActualX().ToString());
            }
            if (id == "GetActualY")
            {
                return (GLOBALS.NUMBER_TYPE, context.GetActualY().ToString());
            }
            if (id == "GetCanvasSize")
            {
                return (GLOBALS.NUMBER_TYPE, context.GetCanvasSize().ToString());
            }
            if (id == "IsCanvasColor")
            {
                string colorName = args[0].Item2;
                int x = int.Parse(args[1].Item2);
                int y = int.Parse(args[2].Item2);

                var colorChar = colorName[0];
                return (GLOBALS.NUMBER_TYPE, context.IsCanvasColor(colorChar, x, y).ToString());
            }
            if (id == "IsBrushSize")
            {
                int x = int.Parse(args[0].Item2);
                return (GLOBALS.NUMBER_TYPE, context.IsBrushSize(x).ToString());
            }
            if (id == "IsBrushColor")
            {
                string colorName = args[0].Item2;
                return (GLOBALS.NUMBER_TYPE, context.IsBrushColor(colorName).ToString());
            }
            if (id == "GetColorCount")
            {
                string name = args[0].Item2;
                char color = name[0];
                int x = int.Parse(args[1].Item2);
                int y = int.Parse(args[2].Item2);
                int z = int.Parse(args[3].Item2);
                int v = int.Parse(args[4].Item2);
                return (GLOBALS.NUMBER_TYPE, context.GetColorCount(color, x, y, z, v).ToString());
            }
          
            context.logger.LogError("Exe", $"Missing method Request: [{id}]", origin.b.line);
            return ("", "");
        }


    }
}
