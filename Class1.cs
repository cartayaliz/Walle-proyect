﻿using System;
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

        public int pc {  get; set; }
        
        public Scanner scannner;

        public Parser parser;

        public Ilogger logger;
        public Interprete(string text, Ilogger logger)
        {
            this.text = text;
            this.logger = logger;
            scannner = new Scanner(text, logger);

            lines = text.Split('\n');

            var tokens = scannner.scanTokens();


            for (int i = 0; i < tokens.Count; i++)
            {
                if (i > 0)
                {
                    tokens[i].back = tokens[i - 1];
                }
                if (i < tokens.Count - 1)
                {
                    tokens[i].next = tokens[i + 1];
                }
         
            }

            var message = "BEGIN TOKENS:\r\n";
          
            var node = tokens[0];
            while (node != null)
            {
                message += node.ToString() + "\r\n";
                node = node.next;
            }
            message += "END\r\n";
            logger.Log("Lexer", message, 0);

            this.parser = new Parser(logger, tokens[0], tokens[tokens.Count - 1]);

            message = "\r\nBEGIN PARSER:\r\n";

            message += this.parser.root.ToString();

            message += "\r\nEND\r\n";

            logger.Log("Lexer", message, 0);

        }

        public IEnumerable<Instruction> Run()
        {
            pc = 0;
            var n = (parser.root.Childrens != null) ? parser.root.Childrens.Count: 0;
            var InstVisitor = new InstructionVisitor(this.logger);

            while(pc < n)
            {

                var inst = parser.root.Childrens[pc].Visit(InstVisitor);
                yield return inst;
                pc++;
            }


        }
    }
}
