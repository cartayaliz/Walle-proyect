using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Executer
    {
        Context context;
        public Executer(Context context) {
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

            context.logger.LogError("Exe", $"Missing method: {id}", ins.origin.b.line);

            return new List<(int, int, int, int, char)>();
        }
    }
}
