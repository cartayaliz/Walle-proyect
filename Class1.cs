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

        public ExeMemory exememory;

        public Ilogger logger;
        public Executer executer;

        public Dictionary<string, string> MappedLabel = new Dictionary<string, string>();
        public Interprete(string text, Ilogger logger, Executer executer)
        {
            this.text = text;
            this.logger = logger;
            this.executer = executer;

            exememory = new ExeMemory();
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

                tokens[i].orden = i;
         
            }

            var message = "";

            // stopper
            if (!logger.HasError)
            {
                message = "BEGIN TOKENS:\r\n";

                var node = tokens[0];
                while (node != null)
                {

                    message += node.orden.ToString() + node.ToString() + "\r\n";
                    node = node.next;
                }
                message += "END\r\n";
                logger.Log(GLOBALS.LOG_LEXER, message, 0);

            }
            else message = "ERROR on LEXER\r\n";

            this.parser = new Parser(logger, tokens[0], tokens[tokens.Count - 1]);

            // stopper
            if (!logger.HasError)
            {
                message = "\r\nBEGIN PARSER:\r\n";

                message += this.parser.root.ToString();

                message += "\r\nEND\r\n";

                logger.Log(GLOBALS.LOG_PARSER, message, 0);

            }



            if (parser.root.Childrens != null && !logger.HasError)
            {
                int n = parser.root.Childrens.Count;
                for(int i = 0; i < n; i++)
                {
                    var item = parser.root.Childrens[i];
                    var nodex = item as ASTId;
                    if (nodex != null)
                    {
                        nodex.isLabel = true;
                        if (MappedLabel.ContainsKey(nodex.b.lexeme))
                        {
                            logger.LogError(GLOBALS.LOG_PARSER, "Label duplicate", nodex.b.line);
                        }
                        MappedLabel[nodex.b.lexeme] = i.ToString();
                    }

                }
            }

        }

        public IEnumerable<Instruction> Run()
        {
            pc = 0;

            // stopper
            if (logger.HasError) yield break;

            var n = (parser.root.Childrens != null) ? parser.root.Childrens.Count: 0;
            
            var InstVisitor = new InstructionVisitor(this.logger, exememory, MappedLabel,executer);

            while(pc < n)
            {
                // stopper
                if (logger.HasError) yield break;

                actualline = parser.root.Childrens[pc].b.line - 1;
                var inst = parser.root.Childrens[pc].Visit(InstVisitor);

                yield return inst;
                pc++;
            }


        }
    }
   
    public class ExeMemory
    {
        public Dictionary<string, (string,string) > D { get; set; }
        public ExeMemory() { D = new Dictionary<string, (string, string)>(); }
    
        public bool hasKey(string key) {
            return D.ContainsKey(key);
        }

        public (string, string) getValue( string key) {
            return D[key];
        }

        public (string, string) setValue( string key, (string, string) value) {
            if (D.ContainsKey(key))
                D[key] = value;
            else
                D.Add(key, value);
            return D[key];
        }
    
    }
}
